using CashCanvas.Common.ConstantHandler;
using CashCanvas.Core.Beans;
using CashCanvas.Core.Beans.Enums;
using CashCanvas.Core.Entities;
using CashCanvas.Core.ViewModel;
using CashCanvas.Data.BillRepository;
using CashCanvas.Data.UnitOfWork;
using CashCanvas.Services.Interfaces;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using SelectPdf;

namespace CashCanvas.Services.Services;

public class BillService(IUnitOfWork unitOfWork, IBillRepository billRepository) : IBillService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IBillRepository _billRepository = billRepository;

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

            List<BillViewModel> result = [.. bills.Select(t => new BillViewModel()
            {
                BillId = t.BillId,
                Amount = t.Amount,
                Title = t.Title,
                PaymentMethod = t.PaymentMethod,
                DueDate = t.DueDate,
                Frequency = t.Frequency,
                ReminderDay = t.ReminderDay,
                Notes = t.Notes?? string.Empty,
                CreatedAt = t.CreatedAt,
                IsContinued = t.IsContinued
            })];
            return result;
        }
        catch (Exception ex)
        {
            throw new Exception(MessageHelper.GetErrorMessageForFeatchOperation(Constants.BILL_LIST), ex);
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

 public (byte[], string) CreatePdf(string BillId, string partialView)
        {
            try
            {
                // Convert the HTML string to a PDF
                HtmlToPdf converter = new();
                PdfDocument pdf = converter.ConvertHtmlString(partialView);

                using MemoryStream stream = new();
                pdf.Save(stream);
                pdf.Close();

                // Generate a file name for the PDF
                string fileName = $"Bill_{BillId}.pdf";

                // Return the PDF as a byte array and the file name
                return (stream.ToArray(), fileName);
            }
            catch
            {
                throw new Exception(Constants.EXPORT_FILE_GENERATION_ERROR);
            }
        }
        public async Task<ResponseResult<byte[]>> ExportOrderList(string orderSearch, string status, string dateRange ,Guid UserId)
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

}
