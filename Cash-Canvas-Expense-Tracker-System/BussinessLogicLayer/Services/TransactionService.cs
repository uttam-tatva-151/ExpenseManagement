using CashCanvas.Core.Entities;
using CashCanvas.Core.ViewModel;
using CashCanvas.Data.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using CashCanvas.Services.Interfaces;
using CashCanvas.Common.ConstantHandler;
using CashCanvas.Core.Beans;
using CashCanvas.Core.Beans.Enums;
using CashCanvas.Core.DTOs;
using OfficeOpenXml;
using System.Drawing;
using OfficeOpenXml.Drawing;
using System.Text;
using OfficeOpenXml.Style;

namespace CashCanvas.Services.Services;

public class TransactionService(IUnitOfWork unitOfWork, ICategoryService categoryService, INotificationService notificationService) : ITransactionService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICategoryService _categoryService = categoryService;
    private readonly INotificationService _notificationService = notificationService;
    public async Task<List<TransactionViewModel>> GetTransactionListAsync(Guid userId)
    {
        try
        {
            IEnumerable<Transaction> transactions = await _unitOfWork.Transactions.GetListAsync(
                                                                    t => (t.IsContinued ||
                                                                            (!t.IsContinued && t.ModifiedAt >= DateTime.UtcNow.AddDays(-30))) &&
                                                                        t.UserId == userId,
                                                                    q => q.Include(x => x.Category));
            List<TransactionViewModel> result = [.. transactions.Select(t => new TransactionViewModel()
            {
                TransactionId = t.TransactionId,
                Amount = t.Amount,
                TransactionType = t.TransactionType,
                PaymentMethod = t.PaymentMethod,
                Description = t.Description?? string.Empty,
                CategoryId = t.CategoryId,
                CategoryName = t.Category.CategoryName,
                CreatedAt = t.CreatedAt,
                TransactionDate = t.TransactionDate,
                IsContinued = t.IsContinued
            })];
            return result;
        }
        catch (Exception ex)
        {
            throw new Exception(MessageHelper.GetErrorMessageForFeatchOperation(Constants.TRANSACTION_LIST), ex);
        }
    }

    public async Task<List<CategoryViewModel>> GetCategoryListAsync(Guid userId)
    {
        return await _categoryService.GetCategoryListAsync(userId);
    }
    public async Task<TransactionViewModel> GetTransactionByIdAsync(Guid transactionId)
    {
        try
        {
            Transaction? transaction = await _unitOfWork.Transactions.GetFirstOrDefaultAsync(t => t.TransactionId == transactionId && t.IsContinued,
                                                                    q => q.Include(x => x.Category));

            if (transaction == null)
            {
                throw new Exception(MessageHelper.GetErrorMessageForFeatchOperation(Constants.TRANSACTION));
            }
            List<CategoryViewModel> categories = await GetCategoryListAsync(transaction.UserId);
            return new TransactionViewModel()
            {
                TransactionId = transaction.TransactionId,
                Amount = transaction.Amount,
                TransactionType = transaction.TransactionType,
                PaymentMethod = transaction.PaymentMethod,
                Description = transaction.Description ?? string.Empty,
                CategoryId = transaction.CategoryId,
                CategoryName = transaction.Category.CategoryName,
                CreatedAt = transaction.CreatedAt,
                TransactionDate = transaction.TransactionDate,
                IsContinued = transaction.IsContinued,
                Categories = categories
            };
        }
        catch (Exception ex)
        {
            throw new Exception(MessageHelper.GetErrorMessageForFeatchOperation(Constants.TRANSACTION_LIST), ex);
        }
    }

    public async Task<ResponseResult<List<CategoryViewModel>>> AddTransactionAsync(TransactionViewModel transactionViewModel)
    {
        try
        {
            ResponseResult<List<CategoryViewModel>> response = new();
            Transaction transaction = new()
            {
                TransactionId = Guid.NewGuid(),
                UserId = transactionViewModel.UserId,
                Amount = transactionViewModel.Amount,
                TransactionType = transactionViewModel.TransactionType,
                PaymentMethod = transactionViewModel.PaymentMethod,
                Description = transactionViewModel.Description,
                CategoryId = transactionViewModel.CategoryId,
                CreatedAt = DateTime.UtcNow,
                IsContinued = transactionViewModel.IsContinued
            };
            transaction.TransactionDate = DateTime.SpecifyKind(transactionViewModel.TransactionDate, DateTimeKind.Local).ToUniversalTime();
            await _unitOfWork.Transactions.AddAsync(transaction);

            int result = await _unitOfWork.CompleteAsync();
            {
                if (result > 0)
                {
                    await _notificationService.SyncTransactionAffectingBudgetNotificationsAsync(transaction.UserId, transaction);
                    response.Status = ResponseStatus.Success;
                    response.Message = MessageHelper.GetSuccessMessageForAddOperation(Constants.TRANSACTION);
                    response.Data = await GetCategoryListAsync(transactionViewModel.UserId);
                }
                else
                {
                    {
                        response.Status = ResponseStatus.Failed;
                        response.Message = MessageHelper.GetErrorMessageForAddOperation(Constants.TRANSACTION);
                        response.Data = await GetCategoryListAsync(transactionViewModel.UserId);
                    }
                }
                return response;

            }
        }
        catch (Exception ex)
        {
            throw new Exception(MessageHelper.GetErrorMessageForFeatchOperation(Constants.TRANSACTION_LIST), ex);
        }
    }

    public async Task<byte[]> ExportTransactionsToCsv(Guid userID)
    {
        List<Transaction> transactionsData = await _unitOfWork.Transactions.GetListAsync(
                                                                                            t => t.IsContinued &&
                                                                                                t.UserId == userID &&
                                                                                                t.TransactionDate >= DateTime.UtcNow.AddMonths(-3),
                                                                                            q => q.Include(x => x.Category)
                                                                                        );

        StringBuilder sb = new();

        // "Designed" informational header (these will appear as plain text rows at the top)
        sb.AppendLine("Cash Canvas");
        sb.AppendLine("Expense tracker system");
        sb.AppendLine($"Export Date: {DateTime.Now:dd-MM-yy HH:mm}");
        sb.AppendLine();

        // CSV column headers
        sb.AppendLine("Sr.No,CategoryName,Amount,Date,Description,Type,PaymentMethod");

        // Data rows
        for (int i = 0; i < transactionsData.Count; i++)
        {
            // Properly escape commas, quotes, and newlines in the description
            string description = transactionsData[i].Description?.Replace("\"", "\"\"") ?? "";
            if (description.Contains(",") || description.Contains("\"") || description.Contains("\n"))
                description = $"\"{description}\"";

            sb.AppendLine($"{1 + 1},{transactionsData[i].Category.CategoryName},{transactionsData[i].Amount},{transactionsData[i].TransactionDate:dd-MM-yy},{description},{transactionsData[i].TransactionType},{transactionsData[i].PaymentMethod}");
        }


        sb.AppendLine();
        sb.AppendLine("Note :- This is a report of transactions of the last 3 months.");

        return Encoding.UTF8.GetBytes(sb.ToString());
    }
    public async Task<byte[]> ExportTransactionsToExcel(Guid userID)
    {
        List<Transaction> transactionsData = await _unitOfWork.Transactions.GetListAsync(
            t => t.IsContinued &&
                 t.UserId == userID &&
                 t.TransactionDate >= DateTime.UtcNow.AddMonths(-3),
            q => q.Include(x => x.Category)
        );

        using ExcelPackage package = new ExcelPackage();
        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Transactions");

        SetupHeaderAndLogo(worksheet);
        AddTableHeader(worksheet);

        int dataStartRow = 8;
        AddTransactionDataRows(worksheet, transactionsData, dataStartRow);

        SetColumnWidths(worksheet);

        using MemoryStream stream = new MemoryStream();
        await package.SaveAsAsync(stream);
        return stream.ToArray();
    }

    public async Task<ResponseResult<bool>> ImportTransactionsFromCsvAsync(Stream csvStream, Guid userId)
    {
        List<Transaction> transactions = [];
        ResponseResult<bool> response = new();
        using (StreamReader reader = new(csvStream))
        {
            string headerLine = await reader.ReadLineAsync() ?? throw new InvalidDataException("CSV file is empty.");
            string[] headers = ParseCsvLine(headerLine);
            ValidateCsvHeaders(headers);

            string? line;
            int rowNumber = 1;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                rowNumber++;
                string[] fields = ParseCsvLine(line);

                if (fields.Length != headers.Length)
                    throw new InvalidDataException($"Row {rowNumber}: Expected {headers.Length} columns but found {fields.Length}.");

                Transaction transaction = await CreateTransactionFromCsvRowAsync(fields, userId, rowNumber);
                transactions.Add(transaction);
            }
        }

        response = await AddTransactionsToDatabaseAsync(transactions);
        return response;
    }

    public async Task<ResponseResult<bool>> UpdateTransactionAsync(TransactionViewModel transactionViewModel)
    {
        try
        {
            ResponseResult<bool> response = new();
            Transaction? transaction = await _unitOfWork.Transactions.GetFirstOrDefaultAsync(t => t.TransactionId == transactionViewModel.TransactionId);
            if (transaction == null)
            {
                response.Status = ResponseStatus.Failed;
                response.Message = MessageHelper.GetErrorMessageForFeatchOperation(Constants.TRANSACTION);
                response.Data = false;
                return response;
            }
            transaction.Amount = transactionViewModel.Amount;
            transaction.TransactionType = transactionViewModel.TransactionType;
            transaction.PaymentMethod = transactionViewModel.PaymentMethod;
            transaction.Description = transactionViewModel.Description;
            transaction.CategoryId = transactionViewModel.CategoryId;
            transaction.IsContinued = transactionViewModel.IsContinued;
            transaction.ModifiedAt = DateTime.UtcNow;
            transaction.TransactionDate = DateTime.SpecifyKind(transactionViewModel.TransactionDate, DateTimeKind.Local).ToUniversalTime();

            _unitOfWork.Transactions.Update(transaction);

            int result = await _unitOfWork.CompleteAsync();
            if (result > 0)
            {
                await _notificationService.SyncTransactionAffectingBudgetNotificationsAsync(transaction.UserId, transaction);
                response.Status = ResponseStatus.Success;
                response.Message = MessageHelper.GetSuccessMessageForUpdateOperation(Constants.TRANSACTION);
                response.Data = true;
            }
            else
            {
                response.Status = ResponseStatus.Failed;
                response.Message = MessageHelper.GetErrorMessageForUpdateOperation(Constants.TRANSACTION);
                response.Data = false;
            }
            return response;

        }
        catch (Exception ex)
        {
            throw new Exception(MessageHelper.GetErrorMessageForFeatchOperation(Constants.TRANSACTION_LIST), ex);
        }
    }

    public async Task<ResponseResult<bool>> DeleteTransactionAsync(Guid transactionId)
    {
        try
        {
            ResponseResult<bool> response = new();
            Transaction? transaction = await _unitOfWork.Transactions.GetFirstOrDefaultAsync(t => t.TransactionId == transactionId);
            if (transaction == null)
            {
                response.Status = ResponseStatus.Failed;
                response.Message = MessageHelper.GetErrorMessageForFeatchOperation(Constants.TRANSACTION);
                response.Data = false;
                return response;
            }
            transaction.IsContinued = false; // Soft delete
            transaction.ModifiedAt = DateTime.UtcNow; // Update the transaction date to now
            _unitOfWork.Transactions.Update(transaction);

            int result = await _unitOfWork.CompleteAsync();
            if (result > 0)
            {
                await _notificationService.SyncTransactionAffectingBudgetNotificationsAsync(transaction.UserId, transaction);
                response.Status = ResponseStatus.Success;
                response.Message = MessageHelper.GetSuccessMessageForDeleteOperation(Constants.TRANSACTION);
                response.Data = true;
            }
            else
            {
                response.Status = ResponseStatus.Failed;
                response.Message = MessageHelper.GetErrorMessageForDeleteOperation(Constants.TRANSACTION);
                response.Data = false;
            }
            return response;

        }
        catch (Exception ex)
        {
            throw new Exception(MessageHelper.GetErrorMessageForFeatchOperation(Constants.TRANSACTION_LIST), ex);
        }
    }

    public async Task<List<TransactionExportViewModel>> GetExortTransactionAsync(Guid userId)
    {
        try
        {
            IEnumerable<Transaction> transactions = await _unitOfWork.Transactions.GetListAsync(
                                                                    t => t.IsContinued &&
                                                                             t.ModifiedAt >= DateTime.UtcNow.AddMonths(-3) &&
                                                                        t.UserId == userId,
                                                                    q => q.Include(x => x.Category));
            List<TransactionExportViewModel> result = [.. transactions.Select(t => new TransactionExportViewModel()
            {
                Amount = t.Amount,
                TransactionType = t.TransactionType,
                PaymentMethod = t.PaymentMethod.ToString(),
                Description = t.Description ?? string.Empty,
                CategoryName = t.Category.CategoryName,
                CreatedAt = t.CreatedAt,
                TransactionDate = t.TransactionDate,
            })];
            return result;
        }
        catch (Exception ex)
        {
            throw new Exception(MessageHelper.GetErrorMessageForFeatchOperation(Constants.TRANSACTION_LIST), ex);
        }
    }

    private static string[] ParseCsvLine(string line)
    {
        return line.Split(',').Select(field => field.Trim()).ToArray();
    }

    private static void ValidateCsvHeaders(string[] headers)
    {
        string[] requiredHeaders = ["CategoryName", "Amount", "TransactionType", "PaymentMethod", "Description", "TransactionDate"];
        for (int i = 0; i < requiredHeaders.Length; i++)
        {
            if (!headers[i].Equals(requiredHeaders[i], StringComparison.OrdinalIgnoreCase))
                throw new InvalidDataException($"Missing or invalid column: {requiredHeaders[i]} at position {i + 1}");
        }
    }

    private async Task<Transaction> CreateTransactionFromCsvRowAsync(string[] fields, Guid userId, int rowNumber)
    {
        try
        {
            string categoryName = fields[0];
            decimal amount = decimal.Parse(fields[1]);
            string transactionType = fields[2];
            string paymentMethodStr = fields[3];
            string description = fields[4];
            DateTime transactionDate = DateTime.Parse(fields[5]);

            PaymentMethod paymentMethod = ParsePaymentMethod(paymentMethodStr, rowNumber);

            Category category = await GetOrCreateCategoryAsync(categoryName, transactionType, userId);

            Transaction transaction = new()
            {
                UserId = userId,
                Amount = amount,
                TransactionType = transactionType,
                PaymentMethod = paymentMethod,
                Description = description,
                CategoryId = category.CategoryId,
                CreatedAt = DateTime.UtcNow,
                TransactionDate = transactionDate.ToUniversalTime(),
                IsContinued = true
            };
            return transaction;
        }
        catch (Exception ex)
        {
            throw new InvalidDataException($"Invalid data at row {rowNumber}: {ex.Message}");
        }
    }

    private static PaymentMethod ParsePaymentMethod(string value, int rowNumber)
    {
        if (Enum.TryParse<PaymentMethod>(value, true, out PaymentMethod paymentMethod))
            return paymentMethod;
        throw new InvalidDataException($"Invalid PaymentMethod '{value}' at row {rowNumber}.");
    }

    private async Task<Category> GetOrCreateCategoryAsync(string categoryName, string type, Guid userId)
    {
        Category? category = await _unitOfWork.Categories
            .GetFirstOrDefaultAsync(c => c.CategoryName == categoryName && c.UserId == userId);

        if (category != null)
            return category;

        category = new Category
        {
            CategoryId = Guid.NewGuid(),
            UserId = userId,
            CategoryName = categoryName,
            Type = type,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Categories.AddAsync(category);
        await _unitOfWork.CompleteAsync();

        return category;
    }

    private async Task<ResponseResult<bool>> AddTransactionsToDatabaseAsync(List<Transaction> transactions)
    {
        try
        {
            ResponseResult<bool> response = new();
            foreach (Transaction transaction in transactions)
            {
                await _unitOfWork.Transactions.AddAsync(transaction);
            }
            int result = await _unitOfWork.CompleteAsync();
            if (result > 0)
            {
                response.Message = Constants.IMPORT_FILE_GENERATION_SUCCESS;
                response.Status = ResponseStatus.Success;
            }
            else
            {
                response.Message = Constants.IMPORT_FILE_GENERATION_ERROR;
                response.Status = ResponseStatus.Failed;
            }
            return response;
        }
        catch (Exception ex)
        {
            // Log ex.ToString() or ex.Message
            Console.WriteLine(ex.ToString());
            throw; // or handle as needed
        }
    }
    private static void SetupHeaderAndLogo(ExcelWorksheet worksheet)
    {
        worksheet.Cells["E2:F5"].Merge = true;
        worksheet.Column(5).Width = 32;
        worksheet.Column(6).Width = 16;
        for (int i = 2; i <= 5; i++)
            worksheet.Row(i).Height = 30;

        string logoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "images", "Logo_for_Cash_Canvas.png");
        if (File.Exists(logoPath))
        {
            FileInfo logoImage = new FileInfo(logoPath);
            ExcelPicture picture = worksheet.Drawings.AddPicture("Logo", logoImage);

            double totalWidth = (worksheet.Column(5).Width + worksheet.Column(6).Width) * 7.5;
            double totalHeight = 0;
            for (int i = 2; i <= 5; i++)
                totalHeight += worksheet.Row(i).Height * 1.33;
            int logoWidth = (int)(totalWidth * 0.9);
            int logoHeight = (int)(totalHeight * 0.9);
            int offsetX = (int)((totalWidth - logoWidth) / 2);
            int offsetY = (int)((totalHeight - logoHeight) / 2);
            picture.SetSize(logoWidth, logoHeight);
            picture.SetPosition(1, offsetY, 4, offsetX);
            picture.EditAs = OfficeOpenXml.Drawing.eEditAs.OneCell;
        }

        worksheet.Cells["A2:D2"].Merge = true;
        worksheet.Cells["A2"].Value = "Cash Canvas";
        worksheet.Cells["A2"].Style.Font.Size = 22;
        worksheet.Cells["A2"].Style.Font.Bold = true;
        worksheet.Cells["A2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        worksheet.Row(2).Height = 32;

        worksheet.Cells["A3:D3"].Merge = true;
        worksheet.Cells["A3"].Value = "Expense tracker system";
        worksheet.Cells["A3"].Style.Font.Size = 14;
        worksheet.Cells["A3"].Style.Font.Bold = true;
        worksheet.Cells["A3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

        worksheet.Cells["A4:D4"].Merge = true;
        worksheet.Cells["A4"].Value = $"Export Date: {DateTime.Now:dd-MM-yy HH:mm}";
        worksheet.Cells["A4"].Style.Font.Italic = true;
        worksheet.Cells["A4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

        worksheet.Cells["A5:D5"].Merge = true;
        worksheet.Cells["A5"].Value = "This is a report of transactions of the last 3 months.";
        worksheet.Cells["A5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

        worksheet.Row(6).Height = 10;
    }

    private static void AddTableHeader(ExcelWorksheet worksheet)
    {
        int headerRow = 7;
        string[] headers = { "Sr.No", "Category Name", "Amount", "Date", "Description", "Type" };
        int colCount = headers.Length;

        for (int i = 0; i < colCount; i++)
        {
            ExcelRange cell = worksheet.Cells[headerRow, i + 1];
            cell.Value = headers[i];
            cell.Style.Font.Bold = true;
            cell.Style.Font.Size = 12;
            cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
            cell.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(220, 230, 241));
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            cell.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            cell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            cell.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            cell.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            cell.Style.WrapText = true;
        }
        worksheet.Row(headerRow).Height = 25;
    }

    private static void AddTransactionDataRows(ExcelWorksheet worksheet, List<Transaction> transactionsData, int startRow)
    {
        int headerRow = startRow - 1;
        int colCount = 6;
        int row = startRow;
        foreach (Transaction transaction in transactionsData)
        {
            worksheet.Cells[row, 1].Value = row - headerRow;
            worksheet.Cells[row, 2].Value = transaction.Category.CategoryName;
            worksheet.Cells[row, 3].Value = transaction.Amount;
            worksheet.Cells[row, 4].Value = transaction.TransactionDate;
            worksheet.Cells[row, 4].Style.Numberformat.Format = "dd-MM-yy";
            worksheet.Cells[row, 5].Value = transaction.Description;
            worksheet.Cells[row, 6].Value = transaction.TransactionType.ToString();

            Color color = transaction.TransactionType.ToLower() == "income"
                ? Color.FromArgb(198, 239, 206)
                : Color.FromArgb(255, 199, 206);
            worksheet.Cells[row, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[row, 3].Style.Fill.BackgroundColor.SetColor(color);

            for (int col = 1; col <= colCount; col++)
            {
                ExcelRange cell = worksheet.Cells[row, col];
                cell.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                cell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                cell.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                cell.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                cell.Style.HorizontalAlignment = (col == 1 || col == 3 || col == 4)
                    ? ExcelHorizontalAlignment.Center
                    : ExcelHorizontalAlignment.Left;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 11;
            }
            worksheet.Row(row).Height = 22;
            row++;
        }
        worksheet.Cells[headerRow, 1, row - 1, colCount].AutoFilter = true;
        worksheet.Row(row).Height = 18;
    }

    private static void SetColumnWidths(ExcelWorksheet worksheet)
    {
        int[] columnWidths = { 9, 22, 14, 15, 44, 13 };
        for (int i = 0; i < columnWidths.Length; i++)
            worksheet.Column(i + 1).Width = columnWidths[i];
    }



}


