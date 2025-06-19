using CashCanvas.Common.ConstantHandler;
using CashCanvas.Core.Beans;
using CashCanvas.Core.Beans.Enums;
using CashCanvas.Core.Entities;
using CashCanvas.Core.ViewModel;
using CashCanvas.Data.BillRepository;
using CashCanvas.Data.UnitOfWork;
using CashCanvas.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CashCanvas.Services.Services;

public class BillService(IUnitOfWork unitOfWork, IBillRepository billRepository, INotificationService notificationService) : IBillService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IBillRepository _billRepository = billRepository;
    private readonly INotificationService _notificationService = notificationService;


    /// <summary>
    /// Retrieves all bills for the specified user with pagination, computing payment and period status for each bill.
    /// </summary>
    /// <param name="userId">The user's unique identifier.</param>
    /// <param name="pagination">Pagination details for filtering bills.</param>
    /// <returns>List of BillViewModel with current status information.</returns>

    public async Task<List<BillViewModel>> GetAllBillsAsync(Guid userId, PaginationDetails pagination)
    {
        try
        {
            IEnumerable<Bill> bills = await _billRepository.GetFilteredBillsAsync(userId, pagination);
            if (!bills.Any())
            {
                return [];
            }

            List<BillViewModel> result = [.. bills.Select(bill =>
                {
                    (DateTime periodStart, DateTime periodEnd) = GetCurrentBillingPeriod(bill.Frequency, bill.DueDate, DateTime.UtcNow);

                        List<(DateTime Start, DateTime End)> periods = GeneratePeriods(bill.DueDate, bill.Frequency, DateTime.UtcNow);
                        HashSet<DateTime> paidPeriods = AssignPaymentsToPeriods(bill.Payments, periods);
                        int missedIntervals = periods.Count(period => !paidPeriods.Contains(period.Start));

                    bool isPaid = bill.Payments
                        .Any(payment =>
                            payment.PaymentDate >= periodStart &&
                            payment.PaymentDate < periodEnd &&
                            payment.Status == PaymentStatus.Complete );
                    DateTime nextDueDate = missedIntervals == 0
                        ? (isPaid ? GetPeriodEnd(periodEnd.AddDays(1), bill.Frequency) : periodEnd)
                        : periods.First(period => !paidPeriods.Contains(period.Start)).Start;

                        return new BillViewModel(){
                                        BillId = bill.BillId,
                                        Amount = bill.Amount,
                                        Title = bill.Title,
                                        PaymentMethod = bill.PaymentMethod,
                                        Frequency = bill.Frequency,
                                        DueDate = bill.NextDueDate ?? bill.DueDate,
                                        ReminderDay = bill.ReminderDay,
                                        Notes = bill.Notes ?? string.Empty,
                                        CreatedAt = bill.CreatedAt,
                                        IsContinued = bill.IsContinued,
                                        IsPaid = isPaid,
                                        LastUnpaidDueDate = nextDueDate,
                                        TotalIntervals = periods.Count,
                                        MissedIntervals = missedIntervals
                         };
                })];
            return result;
        }
        catch (Exception ex)
        {
            throw new Exception(MessageHelper.GetErrorMessageForFeatchOperation(Constants.BILL_LIST), ex);
        }
    }


    /// <summary>
    /// Retrieves the payment history view models for a bill, mapping payments to billing periods.
    /// </summary>
    /// <param name="billId">Unique identifier of the bill.</param>
    /// <returns>List of PaymentHistoryViewModel for each period.</returns>

    public async Task<List<PaymentHistoryViewModel>> GetPaymentHistory(Guid billId)
    {
        try
        {
            Bill? bill = await _unitOfWork.Bills.GetFirstOrDefaultAsync(t => t.BillId == billId && t.IsContinued,
                                                                          q => q.Include(b => b.Payments))
                                                                        ?? throw new Exception(MessageHelper.GetErrorMessageForFeatchOperation(Constants.BILL));

            List<Payment> payments = [.. bill.Payments.Where(p => p.PaymentDate.Date >= bill.DueDate.Date)];
            List<(DateTime StartDate, DateTime EndDate)> periods = GeneratePeriods(bill.DueDate, bill.Frequency, DateTime.UtcNow);

            (DateTime StartDate, DateTime EndDate) currentPeriod = GetCurrentPeriod(bill.DueDate, bill.Frequency, DateTime.UtcNow);
            if (periods.Count == 0 || periods.Last().EndDate < currentPeriod.EndDate)
                periods.Add(currentPeriod);

            return MapPaymentsToPeriodsAdvanced(periods, payments, billId);
        }
        catch (Exception ex)
        {
            throw new Exception(MessageHelper.GetErrorMessageForFeatchOperation(Constants.PAYMENT_LIST), ex);
        }
    }

    /// <summary>
    /// Retrieves detailed bill information for the specified bill ID.
    /// </summary>
    /// <param name="billId">Unique identifier of the bill.</param>
    /// <returns>BillViewModel if found, otherwise throws exception.</returns>
    public async Task<BillViewModel?> GetBillByIdAsync(Guid billId)
    {
        try
        {
            Bill? bill = await _unitOfWork.Bills.GetFirstOrDefaultAsync(t => t.BillId == billId && t.IsContinued)
                                                ?? throw new Exception(MessageHelper.GetErrorMessageForFeatchOperation(Constants.BILL));

            return new BillViewModel()
            {
                BillId = bill.BillId,
                UserId = bill.UserId,
                Amount = bill.Amount,
                PaymentMethod = bill.PaymentMethod,
                Notes = bill.Notes,
                Title = bill.Title,
                DueDate = bill.NextDueDate ?? bill.DueDate,
                Frequency = bill.Frequency,
                ReminderDay = bill.ReminderDay,
                IsContinued = bill.IsContinued,
                CreatedAt = bill.CreatedAt
            };
        }
        catch (Exception ex)
        {
            throw new Exception(MessageHelper.GetErrorMessageForFeatchOperation(Constants.BILL_LIST), ex);
        }
    }

    /// <summary>
    /// Creates a new bill and synchronizes notifications.
    /// </summary>
    /// <param name="billViewModel">The bill details to create.</param>
    /// <returns>ResponseResult with the result of the operation.</returns>
    /// <exception cref="Exception">Throws with a fetch error message if an error occurs.</exception>
    public async Task<ResponseResult<int>> CreateBillAsync(BillViewModel billViewModel)
    {
        try
        {
            ResponseResult<int> response = new();
            Bill bill = new()
            {
                BillId = Guid.NewGuid(),
                UserId = billViewModel.UserId,
                Amount = billViewModel.Amount,
                PaymentMethod = billViewModel.PaymentMethod,
                Notes = billViewModel.Notes,
                Title = billViewModel.Title,
                DueDate = billViewModel.DueDate,
                Frequency = billViewModel.Frequency,
                ReminderDay = billViewModel.ReminderDay,
                IsContinued = billViewModel.IsContinued,
                CreatedAt = DateTime.UtcNow,

            };
            bill.DueDate = billViewModel.DueDate.ToUniversalTime();

            await _unitOfWork.Bills.AddAsync(bill);

            int result = await _unitOfWork.CompleteAsync();
            if (result > 0)
            {
                await _notificationService.SyncBillWithNotificationsAsync(bill.UserId, bill, Constants.DATABASE_ACTION_CREATE);
                response.Status = ResponseStatus.Success;
                response.Message = MessageHelper.GetSuccessMessageForAddOperation(Constants.BILL);
                response.Data = result;
            }
            else
            {
                response.Status = ResponseStatus.Failed;
                response.Message = MessageHelper.GetErrorMessageForAddOperation(Constants.BILL);
                response.Data = result;

            }
            return response;
        }
        catch (Exception ex)
        {
            throw new Exception(MessageHelper.GetErrorMessageForFeatchOperation(Constants.BILL_LIST), ex);
        }
    }

    /// <summary>
    /// Soft deletes a bill and its reminders by marking them as not continued, then synchronizes notifications.
    /// </summary>
    /// <param name="billId">The unique identifier of the bill to move to trash.</param>
    /// <returns>ResponseResult indicating success or failure of the operation.</returns>
    /// <exception cref="Exception">Throws with a fetch error message if operation fails.</exception>
    public async Task<ResponseResult<bool>> MoveToTrashAsync(Guid billId)
    {
        try
        {
            ResponseResult<bool> response = new();
            Bill? bill = await _unitOfWork.Bills.GetFirstOrDefaultAsync(t => t.BillId == billId);
            if (bill == null)
            {
                response.Status = ResponseStatus.Failed;
                response.Message = MessageHelper.GetErrorMessageForFeatchOperation(Constants.TRANSACTION);
                response.Data = false;
                return response;
            }
            bill.IsContinued = false;
            bill.ModifiedAt = DateTime.UtcNow;
            _unitOfWork.Bills.Update(bill);


            IEnumerable<Reminder> reminders = await _unitOfWork.Reminders.GetListAsync(t => t.BillId == billId);

            if (reminders.Any())
            {
                foreach (Reminder reminder in reminders)
                {
                    reminder.IsContinued = false;
                    reminder.ModifiedAt = DateTime.UtcNow;
                }
                _unitOfWork.Reminders.UpdateRange(reminders);
            }
            int result = await _unitOfWork.CompleteAsync();
            if (result > 0)
            {
                await _notificationService.SyncBillWithNotificationsAsync(bill.UserId, bill, Constants.DATABASE_ACTION_DELETE);
                response.Status = ResponseStatus.Success;
                response.Message = MessageHelper.GetSuccessMessageForDeleteOperation(Constants.BILL);
                response.Data = true;
            }
            else
            {
                response.Status = ResponseStatus.Failed;
                response.Message = MessageHelper.GetErrorMessageForDeleteOperation(Constants.BILL);
                response.Data = false;
            }
            return response;

        }
        catch (Exception ex)
        {
            throw new Exception(MessageHelper.GetErrorMessageForFeatchOperation(Constants.BILL_LIST), ex);
        }
    }

    /// <summary>
    /// Updates an existing bill with new details and synchronizes notifications.
    /// </summary>
    /// <param name="billViewModel">The updated bill data.</param>
    /// <returns>ResponseResult indicating the outcome of the update operation.</returns>
    /// <exception cref="Exception">Throws with a fetch error message if the update fails.</exception>
    public async Task<ResponseResult<bool>> UpdateBillAsync(BillViewModel billViewModel)
    {
        try
        {
            ResponseResult<bool> response = new();
            Bill? bill = await _unitOfWork.Bills.GetFirstOrDefaultAsync(t => t.BillId == billViewModel.BillId && t.IsContinued);
            if (bill == null)
            {
                response.Status = ResponseStatus.Failed;
                response.Message = MessageHelper.GetErrorMessageForFeatchOperation(Constants.BILL);
                response.Data = false;
                return response;
            }
            bill.Amount = billViewModel.Amount;
            bill.PaymentMethod = billViewModel.PaymentMethod;
            bill.Notes = billViewModel.Notes;
            bill.Title = billViewModel.Title;
            bill.DueDate = billViewModel.DueDate.ToUniversalTime();
            bill.Frequency = billViewModel.Frequency;
            bill.ReminderDay = billViewModel.ReminderDay;
            bill.IsContinued = billViewModel.IsContinued;
            bill.ModifiedAt = DateTime.UtcNow;

            _unitOfWork.Bills.Update(bill);

            int result = await _unitOfWork.CompleteAsync();
            if (result > 0)
            {
                await _notificationService.SyncBillWithNotificationsAsync(bill.UserId, bill, Constants.DATABASE_ACTION_UPDATE);
                response.Status = ResponseStatus.Success;
                response.Message = MessageHelper.GetSuccessMessageForUpdateOperation(Constants.BILL);
                response.Data = true;
            }
            else
            {
                response.Status = ResponseStatus.Failed;
                response.Message = MessageHelper.GetErrorMessageForUpdateOperation(Constants.BILL);
                response.Data = false;
            }
            return response;

        }
        catch (Exception ex)
        {
            throw new Exception(MessageHelper.GetErrorMessageForFeatchOperation(Constants.BILL_LIST), ex);
        }
    }

    /// <summary>
    /// Pays a bill by creating a payment for the earliest unpaid period (or as late if overdue),
    /// updates the bill's next due date, and persists changes.
    /// </summary>
    /// <param name="paymentViewModel">The payment details to apply.</param>
    /// <returns>
    /// ResponseResult indicating success or failure of the pay operation.
    /// </returns>
    /// <exception cref="Exception">Throws if update operation fails, including the underlying exception.</exception>
    public async Task<ResponseResult<bool>> PayBillAsync(PaymentCreationViewModel paymentViewModel)
    {
        Bill? bill = await _unitOfWork.Bills.GetFirstOrDefaultAsync(
            t => t.BillId == paymentViewModel.BillId && t.IsContinued,
            q => q.Include(b => b.Payments));

        if (bill == null)
            return new ResponseResult<bool>
            {
                Status = ResponseStatus.Failed,
                Message = MessageHelper.GetErrorMessageForFeatchOperation(Constants.BILL),
                Data = false
            };

        List<(DateTime Start, DateTime End)> periods = GeneratePeriods(bill.DueDate, bill.Frequency, DateTime.UtcNow);
        HashSet<DateTime> paidPeriods = AssignPaymentsToPeriods(bill.Payments, periods);
        (DateTime start, DateTime _) = FindEarliestUnpaidPeriod(periods, paidPeriods);

        PaymentStatus status = start < (periods.Count > 0 ? periods[^1].Start : DateTime.UtcNow) ? PaymentStatus.Late : PaymentStatus.Complete;
        DateTime appliedPeriodStart = status == PaymentStatus.Late ? start : (periods.Count > 0 ? periods[^1].Start : DateTime.UtcNow);

        bill.NextDueDate = GetPeriodEnd(periods.OrderByDescending(p => p.End).First().End.AddDays(1), bill.Frequency);
        Payment payment = BuildPayment(paymentViewModel, status);

        _unitOfWork.Bills.Update(bill);
        await _unitOfWork.Payments.AddAsync(payment);

        int result = await _unitOfWork.CompleteAsync();
        return new ResponseResult<bool>
        {
            Status = result > 0 ? ResponseStatus.Success : ResponseStatus.Failed,
            Message = result > 0 ?
                MessageHelper.GetSuccessMessageForUpdateOperation(Constants.BILL) :
                MessageHelper.GetErrorMessageForUpdateOperation(Constants.BILL),
            Data = result > 0
        };
    }

    /// <summary>
    /// Retrieves and exports the user's continued bills from the last 3 months.
    /// </summary>
    /// <param name="userId">The user's unique identifier.</param>
    /// <returns>List of BillExportViewModel for export.</returns>
    /// <exception cref="Exception">Thrown if no continued bills are found or on fetch error.</exception>
    public async Task<List<BillExportViewModel>> GetExortBillsAsync(Guid userId)
    {
        PaginationDetails pagination = new()
        {
            ToDate = DateTime.UtcNow,
            FromDate = DateTime.UtcNow.AddMonths(-3)
        };
        List<Bill> bills = [.. (await _billRepository.GetFilteredBillsAsync(userId, pagination)).Where(b => b.IsContinued)];

        if (bills.Count == 0)
            throw new Exception(MessageHelper.GetInfoMessageForNoRecordsFound(Constants.BILL_LIST));

        return CreateBillExportList(bills);
    }


    #region  Helpers

    /// <summary>
    /// Creates a list of export view models from the provided bills, summarizing payment history and bill details.
    /// </summary>
    /// <param name="bills">A collection of Bill objects to export.</param>
    /// <returns>List of BillExportViewModel containing export-ready bill data.</returns>
    private static List<BillExportViewModel> CreateBillExportList(IEnumerable<Bill> bills)
    {
        List<BillExportViewModel> exportList = [];

        foreach (Bill bill in bills)
        {
            ICollection<Payment> payments = bill.Payments;
            IEnumerable<Payment> completePayments = payments.Where(p => p.Status == PaymentStatus.Complete);

            Payment? lastPaidPayment = completePayments.OrderByDescending(p => p.PaymentDate).FirstOrDefault();
            decimal totalPaid = completePayments.Sum(p => p.AmountPaid);

            int totalPeriods = GeneratePeriods(bill.DueDate, bill.Frequency, DateTime.UtcNow).Count;
            int skippedCount = payments.Count(p => p.Status == PaymentStatus.Skipped);
            int completedOrLateCount = payments.Count(p => p.Status == PaymentStatus.Complete || p.Status == PaymentStatus.Late);
            int missedCount = totalPeriods - skippedCount - completedOrLateCount;

            exportList.Add(new BillExportViewModel
            {
                Title = bill.Title,
                Amount = bill.Amount,
                DueDate = bill.DueDate,
                Frequency = bill.Frequency.ToString(),
                PaymentMethod = bill.PaymentMethod.ToString(),
                LastPaidDate = lastPaidPayment?.PaymentDate,
                TotalPaid = totalPaid,
                MissedCount = missedCount,
                SkippedCount = skippedCount
            });
        }

        return exportList;
    }

    /// <summary>
    /// Generates a list of periods, each represented by a start and end date, 
    /// from the given due date up to (and including) the specified date using the given frequency.
    /// </summary>
    /// <param name="dueDate">The starting date for the first period.</param>
    /// <param name="frequency">The billing frequency (e.g., monthly, weekly).</param>
    /// <param name="upTo">Generate periods up to this date.</param>
    /// <returns>List of (Start, End) tuples for each period within the range.</returns>
    private static List<(DateTime Start, DateTime End)> GeneratePeriods(
        DateTime dueDate, BillFrequency frequency, DateTime upTo)
    {
        List<(DateTime Start, DateTime End)> periods = [];
        DateTime start = dueDate;

        while (true)
        {
            DateTime end = GetPeriodEnd(start, frequency);
            if (end > upTo.AddDays(1)) break;
            periods.Add((start, end));
            start = end;
        }
        return periods;
    }

    /// <summary>
    /// Determines which periods are considered paid based on a collection of payments.
    /// First, periods covered by Complete payments are marked as paid.
    /// Then, remaining unpaid periods are assigned one-by-one to any available Late payments (in chronological order).
    /// Each late payment fills at most one unpaid period.
    /// </summary>
    /// <param name="payments">A collection of Payment objects, each with a date and status.</param>
    /// <param name="periods">A list of periods, each defined by a start and end date.</param>
    /// <returns>
    /// A HashSet of DateTime values corresponding to the start dates of periods that are considered paid
    /// (by either Complete or Late payment).
    /// </returns>
    private static HashSet<DateTime> AssignPaymentsToPeriods(ICollection<Payment> payments,
                                                                List<(DateTime Start, DateTime End)> periods)
    {
        HashSet<DateTime> paidPeriods = [];

        // Assign periods paid by COMPLETE payments
        foreach (Payment payment in payments)
        {
            if (payment.Status == PaymentStatus.Complete)
            {
                foreach ((DateTime Start, DateTime End) period in periods)
                {
                    if (payment.PaymentDate.Date >= period.Start.Date && payment.PaymentDate.Date <= period.End.Date)
                    {
                        paidPeriods.Add(period.Start);
                        break;
                    }
                }
            }
        }

        // Assign LATE payments to remaining periods, one by one
        List<(DateTime Start, DateTime End)> unpaidPeriods = [.. periods
                                                                    .Where(p => !paidPeriods.Contains(p.Start))
                                                                    .OrderBy(p => p.Start)];

        List<Payment> latePayments = [.. payments
                                                .Where(x => x.Status == PaymentStatus.Late)
                                                .OrderBy(x => x.PaymentDate)];

        for (int i = 0; i < latePayments.Count && i < unpaidPeriods.Count; i++)
        {
            paidPeriods.Add(unpaidPeriods[i].Start);
        }

        return paidPeriods;
    }


    /// <summary>
    /// Returns the earliest period whose start date is not present in the set of paid periods.
    /// If all periods are paid, returns the latest period; if no periods exist, returns (DateTime.MinValue, DateTime.MinValue).
    /// </summary>
    /// <param name="periods">A list of periods, each with a start and end date.</param>
    /// <param name="paidPeriods">A set of period start dates that are already paid.</param>
    /// <returns>The earliest unpaid period, the latest period if all are paid, or MinValue tuple if none exist.</returns>
    private static (DateTime Start, DateTime End) FindEarliestUnpaidPeriod(
        List<(DateTime Start, DateTime End)> periods, HashSet<DateTime> paidPeriods)
    {
        // Return first unpaid period, or latest if all are paid, or MinValue if list is empty
        return periods.FirstOrDefault(p => !paidPeriods.Contains(p.Item1),
            periods.Count > 0 ? periods[^1] : (DateTime.MinValue, DateTime.MinValue));
    }

    /// <summary>
    /// Builds a Payment entity from the view model and status.
    /// </summary>
    /// <param name="paymentViewModel">The payment view model.</param>
    /// <param name="status">The payment status.</param>
    /// <returns>Constructed Payment object.</returns>
    private static Payment BuildPayment(PaymentCreationViewModel paymentViewModel, PaymentStatus status)
    {
        Payment payment = new()
        {
            BillId = paymentViewModel.BillId,
            AmountPaid = paymentViewModel.AmountPaid,
            PaymentDate = paymentViewModel.PaymentDate.ToUniversalTime(),
            Status = status,
            CreatedAt = DateTime.UtcNow,
            Notes = paymentViewModel.Notes,
            UserId = paymentViewModel.UserId,
        };
        return payment;
    }

    /// <summary>
    /// Gets the start and end date of the current billing period for a bill.
    /// </summary>
    /// <param name="frequency">Billing frequency.</param>
    /// <param name="dueDate">Bill's initial due date.</param>
    /// <param name="referenceDate">Reference date to calculate period.</param>
    /// <returns>Tuple of periodStart and periodEnd.</returns>
    private static (DateTime periodStart, DateTime periodEnd) GetCurrentBillingPeriod(BillFrequency frequency, DateTime dueDate, DateTime referenceDate)
    {
        int periodCount = GetPeriodsSinceDueDate(frequency, dueDate, referenceDate);
        DateTime periodStart = GetPeriodStart(frequency, dueDate, periodCount);
        DateTime periodEnd = GetPeriodEnd(periodStart, frequency);
        return (periodStart, periodEnd);
    }

    /// <summary>
    /// Gets the number of periods since the bill's due date up to the reference date.
    /// </summary>
    /// <param name="frequency">Billing frequency.</param>
    /// <param name="dueDate">Bill's initial due date.</param>
    /// <param name="referenceDate">Reference date.</param>
    /// <returns>Number of periods elapsed.</returns>
    private static int GetPeriodsSinceDueDate(BillFrequency frequency, DateTime dueDate, DateTime referenceDate)
    {
        switch (frequency)
        {
            case BillFrequency.Daily:
                return (referenceDate.Date - dueDate.Date).Days;

            case BillFrequency.Weekly:
                return (int)((referenceDate.Date - dueDate.Date).TotalDays / 7);

            case BillFrequency.BiWeekly:
                return (int)((referenceDate.Date - dueDate.Date).TotalDays / 14);

            case BillFrequency.Monthly:
                int months = ((referenceDate.Year - dueDate.Year) * 12) + (referenceDate.Month - dueDate.Month);
                if (referenceDate.Day < dueDate.Day)
                    months -= 1;
                return months;

            case BillFrequency.Quarterly:
                int totalMonths = ((referenceDate.Year - dueDate.Year) * 12) + (referenceDate.Month - dueDate.Month);
                return totalMonths / 3;

            case BillFrequency.HalfYearly:
                int monthsSince = ((referenceDate.Year - dueDate.Year) * 12) + (referenceDate.Month - dueDate.Month);
                return monthsSince / 6;

            case BillFrequency.Yearly:
                int years = referenceDate.Year - dueDate.Year;
                if (referenceDate.Month < dueDate.Month || (referenceDate.Month == dueDate.Month && referenceDate.Day < dueDate.Day))
                    years -= 1;
                return years;

            default:
                throw new ArgumentOutOfRangeException(nameof(frequency), frequency, "Unsupported frequency");
        }
    }

    /// <summary>
    /// Gets the start date of a specific period based on the frequency and period count.
    /// </summary>
    /// <param name="frequency">Billing frequency.</param>
    /// <param name="dueDate">Bill's initial due date.</param>
    /// <param name="periodCount">Number of periods since due date.</param>
    /// <returns>Start date of the period.</returns>
    private static DateTime GetPeriodStart(BillFrequency frequency, DateTime dueDate, int periodCount)
    {
        return frequency switch
        {
            BillFrequency.Daily => dueDate.Date.AddDays(periodCount),
            BillFrequency.Weekly => dueDate.Date.AddDays(periodCount * 7),
            BillFrequency.BiWeekly => dueDate.Date.AddDays(periodCount * 14),
            BillFrequency.Monthly => dueDate.AddMonths(periodCount),
            BillFrequency.Quarterly => dueDate.AddMonths(periodCount * 3),
            BillFrequency.HalfYearly => dueDate.AddMonths(periodCount * 6),
            BillFrequency.Yearly => dueDate.AddYears(periodCount),
            _ => throw new ArgumentOutOfRangeException(nameof(frequency), frequency, "Unsupported frequency"),
        };
    }

    /// <summary>
    /// Gets the end date of a billing period based on the start date and frequency.
    /// </summary>
    /// <param name="start">Start date of the period.</param>
    /// <param name="frequency">Billing frequency.</param>
    /// <returns>End date of the period.</returns>
    private static DateTime GetPeriodEnd(DateTime start, BillFrequency frequency)
    {
        return frequency switch
        {
            BillFrequency.Daily => start.AddDays(1),
            BillFrequency.Weekly => start.AddDays(7),
            BillFrequency.BiWeekly => start.AddDays(14),
            BillFrequency.Monthly => start.AddMonths(1),
            BillFrequency.Quarterly => start.AddMonths(3),
            BillFrequency.HalfYearly => start.AddMonths(6),
            BillFrequency.Yearly => start.AddYears(1),
            _ => throw new ArgumentOutOfRangeException()
        };
    }


    /// <summary>
    /// Gets the current billing period (start and end) that contains the reference date.
    /// </summary>
    /// <param name="dueDate">Bill's initial due date.</param>
    /// <param name="frequency">Billing frequency.</param>
    /// <param name="now">Reference date.</param>
    /// <returns>Tuple of StartDate and EndDate of the current period.</returns>
    private static (DateTime StartDate, DateTime EndDate) GetCurrentPeriod(DateTime dueDate, BillFrequency frequency, DateTime now)
    {
        DateTime start = dueDate;
        DateTime end = GetPeriodEnd(start, frequency);

        while (end <= now)
        {
            start = end;
            end = GetPeriodEnd(start, frequency);
        }
        return (start, end);
    }

    /// <summary>
    /// Maps payments (Complete, Late, Skipped) to their corresponding periods, returning a list of payment history view models.
    /// Skipped payments are matched by date; Complete/Late payments are assigned to the earliest unmatched period.
    /// Periods with no payment/skipped are marked as missed.
    /// </summary>
    /// <param name="periods">List of periods (start and end dates).</param>
    /// <param name="payments">List of payments to assign.</param>
    /// <returns>Ordered list of PaymentHistoryViewModel, with payment/skipped/missed status for each period.</returns>
    public static List<PaymentHistoryViewModel> MapPaymentsToPeriodsAdvanced(
        List<(DateTime Start, DateTime End)> periods,
        List<Payment> payments, Guid billId)
    {
        periods = [.. periods.OrderBy(p => p.Start)];
        List<Payment> orderedPayments = [.. payments
            .Where(p => p.Status is PaymentStatus.Complete or PaymentStatus.Late or PaymentStatus.Skipped)
            .OrderBy(p => p.PaymentDate)];

        List<PaymentHistoryViewModel> result = [];
        HashSet<int> assigned = [];

        // 1. Assign Skipped payments by period matching
        for (int i = 0; i < periods.Count; i++)
        {
            Payment? skip = orderedPayments.FirstOrDefault(p =>
                p.Status == PaymentStatus.Skipped &&
                p.PaymentDate.Date >= periods[i].Start.Date &&
                p.PaymentDate.Date < periods[i].End.Date);

            if (skip != null)
            {
                result.Add(new PaymentHistoryViewModel
                {
                    PeriodStart = periods[i].Start,
                    PeriodEnd = periods[i].End,
                    IsPaid = false,
                    IsSkipped = true,
                    PaidAmount = null,
                    PaidDate = skip.PaymentDate,
                    PaymentId = skip.PaymentId,
                    BillId = billId
                });
                assigned.Add(i);
            }
        }

        // 2. Assign Complete/Late payments to earliest unassigned period
        int paymentIdx = 0;
        for (int i = 0; i < periods.Count && paymentIdx < orderedPayments.Count; i++)
        {
            if (assigned.Contains(i))
                continue;

            Payment payment = orderedPayments[paymentIdx];
            if (payment.Status is PaymentStatus.Complete or PaymentStatus.Late)
            {
                result.Add(new PaymentHistoryViewModel
                {
                    PeriodStart = periods[i].Start,
                    PeriodEnd = periods[i].End,
                    IsPaid = true,
                    IsSkipped = false,
                    PaidAmount = payment.AmountPaid,
                    PaidDate = payment.PaymentDate,
                    PaymentId = payment.PaymentId,
                    BillId = billId
                });
                paymentIdx++;
            }
        }

        // 3. Mark remaining as missed
        for (int i = 0; i < periods.Count; i++)
        {
            if (!result.Any(r => r.PeriodStart == periods[i].Start && r.PeriodEnd == periods[i].End))
            {
                result.Add(new PaymentHistoryViewModel
                {
                    PeriodStart = periods[i].Start,
                    PeriodEnd = periods[i].End,
                    IsPaid = false,
                    IsSkipped = false,
                    PaidAmount = null,
                    PaidDate = null,
                    PaymentId = null,
                    BillId = billId
                });
            }
        }

        return [.. result.OrderBy(r => r.PeriodStart)];
    }


    #endregion
}
