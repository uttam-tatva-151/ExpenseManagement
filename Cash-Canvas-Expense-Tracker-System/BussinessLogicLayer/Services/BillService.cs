using CashCanvas.Common.ConstantHandler;
using CashCanvas.Core.Beans;
using CashCanvas.Core.Beans.Enums;
using CashCanvas.Core.Entities;
using CashCanvas.Core.ViewModel;
using CashCanvas.Data.BillRepository;
using CashCanvas.Data.UnitOfWork;
using CashCanvas.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace CashCanvas.Services.Services;

public class BillService(IUnitOfWork unitOfWork, IBillRepository billRepository, INotificationService notificationService) : IBillService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IBillRepository _billRepository = billRepository;
    private readonly INotificationService _notificationService = notificationService;

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
            {
                if (result > 0)
                {
                    await _notificationService.SyncBillWithNotificationsAsync(bill.UserId, bill, Constants.DATABASE_ACTION_CREATE);
                    response.Status = ResponseStatus.Success;
                    response.Message = MessageHelper.GetSuccessMessageForAddOperation(Constants.BILL);
                    response.Data = result;
                }
                else
                {
                    {
                        response.Status = ResponseStatus.Failed;
                        response.Message = MessageHelper.GetErrorMessageForAddOperation(Constants.BILL);
                        response.Data = result;
                    }
                }
                return response;

            }
        }
        catch (Exception ex)
        {
            throw new Exception(MessageHelper.GetErrorMessageForFeatchOperation(Constants.BILL_LIST), ex);
        }
    }

    public async Task<List<BillViewModel>> GetAllBillsAsync(Guid userId, PaginationDetails pagination)
    {
        try
        {
            IEnumerable<Bill> bills = await _billRepository.GetFilteredBillsAsync(userId, pagination);
            if (bills.Count() == 0)
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
                    DateTime nextDueDate;
                        if (missedIntervals == 0)
                        {
                            if(isPaid){
                                nextDueDate = GetPeriodEnd(periodEnd.AddDays(1), bill.Frequency);
                            }else{
                            nextDueDate = periodEnd;
                            }
                        }
                        else
                        {
                            nextDueDate = periods.First(period => !paidPeriods.Contains(period.Start)).Start;
                        }
                    return new BillViewModel()
                    {
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

    public async Task<List<PaymentHistoryViewModel>> GetPaymentHistory(Guid billId)
    {
        try
        {
            Bill? bill = await _unitOfWork.Bills.GetFirstOrDefaultAsync(t => t.BillId == billId && t.IsContinued,
                                                                          q => q.Include(b => b.Payments))
                                                                        ?? throw new Exception(MessageHelper.GetErrorMessageForFeatchOperation(Constants.BILL));

            List<Payment> payments = [.. bill.Payments.Where(p => p.PaymentDate.Date >= bill.DueDate.Date)];

            List<(DateTime StartDate, DateTime EndDate)> periods = GeneratePeriods(bill.DueDate, bill.Frequency, DateTime.UtcNow);

            (DateTime StartDate, DateTime EndDate) = periods.LastOrDefault();
            (DateTime StartDate, DateTime EndDate) currentPeriod;

            currentPeriod = GetCurrentPeriod(bill.DueDate, bill.Frequency, DateTime.UtcNow);

            if (EndDate < currentPeriod.EndDate)
            {
                periods.Add(currentPeriod);
            }
            List<PaymentHistoryViewModel> paymentHistories = MapPaymentsToPeriodsAdvanced(periods, payments);

            return paymentHistories;
        }
        catch (Exception ex)
        {
            throw new Exception(MessageHelper.GetErrorMessageForFeatchOperation(Constants.PAYMENT_LIST), ex);
        }
    }
    public async Task<BillViewModel?> GetBillByIdAsync(Guid billId)
    {
        try
        {
            Bill? bill = await _unitOfWork.Bills.GetFirstOrDefaultAsync(t => t.BillId == billId && t.IsContinued);

            if (bill == null)
            {
                throw new Exception(MessageHelper.GetErrorMessageForFeatchOperation(Constants.BILL));
            }
            return new BillViewModel()
            {
                BillId = bill.BillId,
                UserId = bill.UserId,
                Amount = bill.Amount,
                PaymentMethod = bill.PaymentMethod,
                Notes = bill.Notes,
                Title = bill.Title,
                DueDate = bill.DueDate,
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
            bill.IsContinued = false; // Soft delete
            bill.ModifiedAt = DateTime.UtcNow; // Update the transaction date to now
            _unitOfWork.Bills.Update(bill);

            //If Bill have pre set reminders then delete it
            IEnumerable<Reminder> reminders = await _unitOfWork.Reminders.GetListAsync(t => t.BillId == billId);

            if (reminders.Any())
            {
                foreach (Reminder reminder in reminders)
                {
                    reminder.IsContinued = false; // Soft delete
                    reminder.ModifiedAt = DateTime.UtcNow; // Update the transaction date to now
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

    public async Task<ResponseResult<bool>> PayBillAsync(PaymentCreationViewModel paymentViewModel)
    {
        try
        {
            ResponseResult<bool> response = new();

            Bill? bill = await _unitOfWork.Bills.GetFirstOrDefaultAsync(t => t.BillId == paymentViewModel.BillId && t.IsContinued,
                                                                         q => q.Include(b => b.Payments));
            if (bill == null)
            {
                response.Status = ResponseStatus.Failed;
                response.Message = MessageHelper.GetErrorMessageForFeatchOperation(Constants.BILL);
                response.Data = false;
                return response;
            }

            List<(DateTime Start, DateTime End)> periods = GeneratePeriods(bill.DueDate, bill.Frequency, DateTime.UtcNow);
            HashSet<DateTime> paidPeriods = AssignPaymentsToPeriods(bill.Payments, periods);

            (DateTime Start, DateTime End) = FindEarliestUnpaidPeriod(periods, paidPeriods);

            PaymentStatus status = PaymentStatus.Complete;
            DateTime appliedPeriodStart = periods.Count > 0 ? periods[^1].Start : DateTime.UtcNow;

            if (Start < appliedPeriodStart)
            {
                status = PaymentStatus.Late;
                appliedPeriodStart = Start;
            }

            bill.NextDueDate = GetPeriodEnd(periods.OrderByDescending(p => p.End).First().End.AddDays(1), bill.Frequency);
            Payment payment = BuildPayment(paymentViewModel, status);

             _unitOfWork.Bills.Update(bill);
            await _unitOfWork.Payments.AddAsync(payment);
            int result = await _unitOfWork.CompleteAsync();
            if (result > 0)
            {
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
            throw new Exception(MessageHelper.GetErrorMessageForUpdateOperation(Constants.BILL), ex);
        }
    }

    public async Task<ResponseResult<byte[]>> ExportOrderList(string orderSearch, string status, string dateRange, Guid UserId)
    {
        ResponseResult<byte[]> result = new();
        try
        {
            PaginationDetails paginationDetails = new()
            {
                PageSize = 0,
                SearchTerm = orderSearch
            };
            List<BillViewModel> orderList = await GetAllBillsAsync(Guid.Empty, paginationDetails);
            result.Data = GenerateExcelFile(orderSearch, status, dateRange, paginationDetails.TotalRecords, orderList);
            if (result.Data == null)
            {
                result.Message = MessageHelper.GetInfoMessageForNoRecordsFound(Constants.BILL_LIST);
                result.Status = ResponseStatus.NotFound;
            }
            else
            {
                result.Message = Constants.EXPORT_FILE_GENERATION_SUCCESS;
                result.Status = ResponseStatus.Success;
            }
        }
        catch (Exception ex)
        {
            result.Message = ex.Message;
            result.Status = ResponseStatus.Failed;
        }
        return result;
    }

    public async Task<List<BillExportViewModel>> GetExortBillsAsync(Guid userId)
    {
        try
        {
            PaginationDetails pagination = new()
            {
                ToDate = DateTime.UtcNow,
                FromDate = DateTime.UtcNow.AddMonths(-3)
            };
            List<Bill> bills = await _billRepository.GetFilteredBillsAsync(userId, pagination);
            bills = [.. bills.Where(b => b.IsContinued)];
            if (bills.Count == 0)
            {
                throw new Exception(MessageHelper.GetInfoMessageForNoRecordsFound(Constants.BILL_LIST));
            }
            
            List<BillExportViewModel> exportedBills = CreateBillExportList(bills);
            return exportedBills;
        }
        catch (Exception ex)
        {
            throw new Exception(MessageHelper.GetErrorMessageForFeatchOperation(Constants.BILL_LIST), ex);
        }
    }
    #region  Helpers
    public static List<BillExportViewModel> CreateBillExportList(IEnumerable<Bill> bills)
    {
        List<BillExportViewModel> exportList = [];

        foreach (Bill bill in bills)
        {
            ICollection<Payment> payments = bill.Payments;

            Payment? lastPaidPayment = payments
                .Where(p => p.Status == PaymentStatus.Complete)
                .OrderByDescending(p => p.PaymentDate)
                .FirstOrDefault();

            decimal totalPaid = payments
                .Where(p => p.Status == PaymentStatus.Complete)
                .Sum(p => p.AmountPaid);

            int totalPamentCounts = GeneratePeriods(bill.DueDate, bill.Frequency, DateTime.UtcNow).Count;
            int skippedCount = payments.Count(p => p.Status == PaymentStatus.Skipped);
            int missedCount = totalPamentCounts - skippedCount - payments.Count(p => p.Status == PaymentStatus.Complete || p.Status == PaymentStatus.Late);

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
    private static byte[] GenerateExcelFile(string billSearch, string status, string dateRange, int noOfRecords, List<BillViewModel> bills)
    {
        try
        {
            string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Templates", Constants.BILL_LIST_EXCEL_FORMAT_FILE);
            FileInfo fileInfo = new(templatePath);
            if (!fileInfo.Exists)
            {
                throw new FileNotFoundException(Constants.TEMPLATE_NOT_FOUND);
            }
            using ExcelPackage package = new(fileInfo);
            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

            worksheet.Cells["C2:F3"].Value = status;
            worksheet.Cells["J2:M3"].Value = billSearch;
            worksheet.Cells["C5:F6"].Value = dateRange;
            worksheet.Cells["J5:M6"].Value = noOfRecords;
            worksheet.Cells[1, 1, 8, 15].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[1, 1, 8, 15].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            int startRow = 9;

            foreach (BillViewModel bill in bills)
            {
                worksheet.Cells[startRow, 1].Value = bill.BillId.ToString();
                worksheet.Cells[startRow, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                worksheet.Cells[startRow, 2, startRow, 4].Merge = true;
                worksheet.Cells[startRow, 2, startRow, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                worksheet.Cells[startRow, 2].Value = bill.DueDate.ToString();
                worksheet.Cells[startRow, 5, startRow, 7].Merge = true;
                worksheet.Cells[startRow, 5, startRow, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                worksheet.Cells[startRow, 5].Value = bill.Title;
                worksheet.Cells[startRow, 8, startRow, 10].Merge = true;
                worksheet.Cells[startRow, 8, startRow, 10].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                worksheet.Cells[startRow, 8].Value = bill.PaymentMethod;
                worksheet.Cells[startRow, 11, startRow, 12].Merge = true;
                worksheet.Cells[startRow, 11, startRow, 12].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                worksheet.Cells[startRow, 11].Value = bill.Frequency;
                worksheet.Cells[startRow, 13, startRow, 14].Merge = true;
                worksheet.Cells[startRow, 13, startRow, 14].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                worksheet.Cells[startRow, 13].Value = bill.ReminderDay;
                worksheet.Cells[startRow, 15, startRow, 16].Merge = true;
                worksheet.Cells[startRow, 15, startRow, 16].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                worksheet.Cells[startRow, 15].Value = bill.Amount.ToString("C2");

                worksheet.Cells[startRow, 1, startRow, 15].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[startRow, 1, startRow, 15].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                startRow++;
            }
            return package.GetAsByteArray();
        }
        catch
        {
            throw;
        }
    }

    private static List<(DateTime Start, DateTime End)> GeneratePeriods(DateTime dueDate, BillFrequency frequency, DateTime upTo)
    {
        List<(DateTime, DateTime)> periods = [];
        DateTime start = dueDate;
        DateTime now = upTo;
        DateTime end = GetPeriodEnd(start, frequency);

        while (end <= now.AddDays(1))
        {
            periods.Add((start, end));
            start = end;
            end = GetPeriodEnd(start, frequency);
        }
        return periods;
    }

    private static HashSet<DateTime> AssignPaymentsToPeriods(ICollection<Payment> payments, List<(DateTime Start, DateTime End)> periods)
    {
        HashSet<DateTime> paidPeriods = [];
        foreach (Payment payment in payments.Where(x => x.Status == PaymentStatus.Complete || x.Status == PaymentStatus.Late))
        {
            foreach ((DateTime Start, DateTime End) in periods)
            {
                if (payment.PaymentDate.Date >= Start.Date && payment.PaymentDate.Date <= End.Date)
                {
                    paidPeriods.Add(Start);
                    break;
                }
            }
        }
        return paidPeriods;
    }
    private static (DateTime Start, DateTime End) FindEarliestUnpaidPeriod(List<(DateTime Start, DateTime End)> periods, HashSet<DateTime> paidPeriods)
    {
        foreach ((DateTime Start, DateTime End) period in periods)
        {
            if (!paidPeriods.Contains(period.Start))
            {
                return period;
            }
        }
        // If all paid, return the latest period
        return periods.Count > 0 ? periods[^1] : (DateTime.MinValue, DateTime.MinValue);
        // periods[periods.Count - 1] == ^1(Use Index Operator)
    }

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

    private static (DateTime periodStart, DateTime periodEnd) GetCurrentBillingPeriod(BillFrequency frequency, DateTime dueDate, DateTime referenceDate)
    {
        int periodCount = GetPeriodsSinceDueDate(frequency, dueDate, referenceDate);
        DateTime periodStart = GetPeriodStart(frequency, dueDate, periodCount);
        DateTime periodEnd = GetPeriodEnd(periodStart, frequency);
        return (periodStart, periodEnd);
    }
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

    private static DateTime GetPeriodStart(BillFrequency frequency, DateTime dueDate, int periodCount)
    {
        switch (frequency)
        {
            case BillFrequency.Daily:
                return dueDate.Date.AddDays(periodCount);
            case BillFrequency.Weekly:
                return dueDate.Date.AddDays(periodCount * 7);
            case BillFrequency.BiWeekly:
                return dueDate.Date.AddDays(periodCount * 14);
            case BillFrequency.Monthly:
                return dueDate.AddMonths(periodCount);
            case BillFrequency.Quarterly:
                return dueDate.AddMonths(periodCount * 3);
            case BillFrequency.HalfYearly:
                return dueDate.AddMonths(periodCount * 6);
            case BillFrequency.Yearly:
                return dueDate.AddYears(periodCount);
            default:
                throw new ArgumentOutOfRangeException(nameof(frequency), frequency, "Unsupported frequency");
        }
    }
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

    public static List<PaymentHistoryViewModel> MapPaymentsToPeriodsAdvanced(
    List<(DateTime Start, DateTime End)> periods,
    List<Payment> payments)
    {
        // Sort periods and payments by date
        periods = periods.OrderBy(p => p.Start).ToList();
        var orderedPayments = payments
            .Where(p => p.Status == PaymentStatus.Complete || p.Status == PaymentStatus.Late || p.Status == PaymentStatus.Skipped)
            .OrderBy(p => p.PaymentDate)
            .ToList();

        // Prepare mapping: null = missed
        var result = new List<PaymentHistoryViewModel>();
        var assignedPeriods = new HashSet<int>();

        // 1. Assign Skipped payments directly if date falls in a period
        for (int i = 0; i < periods.Count; i++)
        {
            var skip = orderedPayments.FirstOrDefault(p =>
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
                    PaymentId = skip.PaymentId
                });
                assignedPeriods.Add(i);
            }
        }

        // 2. Assign normal and late payments to earliest unmatched (unpaid/unskipped) period
        int paymentIdx = 0;
        for (int i = 0; i < periods.Count && paymentIdx < orderedPayments.Count; i++)
        {
            // Skip already assigned skipped periods
            if (assignedPeriods.Contains(i))
                continue;

            var payment = orderedPayments[paymentIdx];

            if (payment.Status == PaymentStatus.Complete || payment.Status == PaymentStatus.Late)
            {
                result.Add(new PaymentHistoryViewModel
                {
                    PeriodStart = periods[i].Start,
                    PeriodEnd = periods[i].End,
                    IsPaid = true,
                    IsSkipped = false,
                    PaidAmount = payment.AmountPaid,
                    PaidDate = payment.PaymentDate,
                    PaymentId = payment.PaymentId
                });
                paymentIdx++;
            }
        }

        // 3. Mark any remaining periods as missed
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
                    PaymentId = null
                });
            }
        }

        // Final sort by period
        return result.OrderBy(r => r.PeriodStart).ToList();
    }


    #endregion
}
