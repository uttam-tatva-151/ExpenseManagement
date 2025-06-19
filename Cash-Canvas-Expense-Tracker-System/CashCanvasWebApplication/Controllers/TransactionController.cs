using CashCanvas.Common.ConstantHandler;
using CashCanvas.Common.DocumentConverter;
using CashCanvas.Common.TosterHandlers;
using CashCanvas.Core.Beans;
using CashCanvas.Core.Beans.Enums;
using CashCanvas.Core.DTOs;
using CashCanvas.Core.ViewModel;
using CashCanvas.Services.Interfaces;
using CashCanvas.Web.Extensions;
using CashCanvas.Web.Hubs;
using CashCanvas.Web.Middlewares;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace CashCanvas.Web.Controllers;

public class TransactionController(ITransactionService transactionService, IHubContext<TransactionHub> hubContext) : Controller
{
    private readonly IHubContext<TransactionHub> _hubContext = hubContext;

    private readonly ITransactionService _transactionService = transactionService;
    public async Task<IActionResult> Index()
    {
        Guid userId = User.GetUserId();
        if (userId == Guid.Empty)
        {
            ToasterHelper.SetToastMessage(TempData, MessageHelper.GetNotFoundMessage(Constants.USER), ResponseStatus.Warning);
            return RedirectToAction("Index", "Home");
        }
        List<TransactionViewModel> transactions = await _transactionService.GetTransactionListAsync(userId);
        return View(transactions);
    }

    [HttpGet]
    [Route("Transaction/AddTransaction")]
    public async Task<IActionResult> AddTransaction()
    {
        Guid userId = User.GetUserId();
        List<CategoryViewModel> categories = await _transactionService.GetCategoryListAsync(userId);
        return View(categories);
    }

    [HttpPost]
    public async Task<IActionResult> AddTransaction(TransactionViewModel newTransaction)
    {
        ResponseResult<List<CategoryViewModel>> response = new();
        newTransaction.UserId = User.GetUserId();
        if (newTransaction == null)
        {
            ToasterHelper.SetToastMessage(TempData, Messages.WARNING_INVALID_INPUT, ResponseStatus.Warning);
            return View(newTransaction);
        }
        response = await _transactionService.AddTransactionAsync(newTransaction);
        ToasterHelper.SetToastMessage(TempData, response.Message, response.Status);
        // 🔴 Broadcast via SignalR
        if (response.Status == ResponseStatus.Success)
        {
            Guid userId = User.GetUserId();
            await _hubContext.Clients.User(userId.ToString()).SendAsync("TransactionAdded", newTransaction);
        }
        return RedirectToAction("Index");
    }

    [HttpGet]
    [Route("Transaction/EditTransaction/{id}")]
    public async Task<IActionResult> EditTransaction(Guid id)
    {
        if (id == Guid.Empty)
        {
            ToasterHelper.SetToastMessage(TempData, Messages.WARNING_INVALID_INPUT, ResponseStatus.Warning);
            return RedirectToAction("Index");
        }
        TransactionViewModel transaction = await _transactionService.GetTransactionByIdAsync(id);
        if (transaction == null)
        {
            ToasterHelper.SetToastMessage(TempData, MessageHelper.GetNotFoundMessage(Constants.TRANSACTION), ResponseStatus.Warning);
            return RedirectToAction("Index");
        }
        // List<CategoryViewModel> categories = await _transactionService.GetCategoryListAsync(User.GetUserId());
        return View(transaction);

    }

    [HttpPost]
    public async Task<IActionResult> UpdateTransaction(TransactionViewModel transaction)
    {
        if (transaction == null || transaction.TransactionId == Guid.Empty)
        {
            ToasterHelper.SetToastMessage(TempData, Messages.WARNING_INVALID_INPUT, ResponseStatus.Warning);
            return View(transaction);
        }
        ResponseResult<bool> response = await _transactionService.UpdateTransactionAsync(transaction);
        ToasterHelper.SetToastMessage(TempData, response.Message, response.Status);
        // 🔴 Broadcast update
        if (response.Status == ResponseStatus.Success)
        {
            Guid userId = User.GetUserId();
            await _hubContext.Clients.User(userId.ToString()).SendAsync("TransactionUpdated", transaction);
        }
        return View("EditTransaction", transaction);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteTransaction(Guid transactionId)
    {
        if (transactionId == Guid.Empty)
        {
            ToasterHelper.SetToastMessage(TempData, Messages.WARNING_INVALID_INPUT, ResponseStatus.Warning);
            return RedirectToAction("Index");
        }
        ResponseResult<bool> response = await _transactionService.DeleteTransactionAsync(transactionId);
        ToasterHelper.SetToastMessage(TempData, response.Message, response.Status);
        // 🔴 Broadcast delete
        if (response.Status == ResponseStatus.Success)
        {
            await _hubContext.Clients.All.SendAsync("TransactionDeleted", transactionId);
        }
        return Json(new
        {
            status = response.Status.ToString(),
            message = response.Message
        });
    }

    [HttpGet]
    public async Task<IActionResult> ExportPdf()
    {

        Guid UserId = User.GetUserId();
        if (UserId == Guid.Empty)
        {
            ToasterHelper.SetToastMessage(TempData, MessageHelper.GetNotFoundMessage(Constants.USER), ResponseStatus.Warning);
            return RedirectToAction("Index", "Home");
        }
        List<TransactionExportViewModel> transactions = await _transactionService.GetExortTransactionAsync(UserId);

        string partialView = await this.RenderPartialViewToString(Constants.EXPORT_TRANSACTION_VIEW, transactions);

        byte[] pdfArray = StringToPdfConverter.CreatePdf(partialView);
        string fileName = $"Transactions_{DateTime.Now:dd-MM-yy}.pdf";

        return File(pdfArray, Constants.PDF_CONTENT_TYPE, fileName);
    }
}
