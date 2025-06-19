using System.Threading.Tasks;
using CashCanvas.Common.ConstantHandler;
using CashCanvas.Common.DocumentConverter;
using CashCanvas.Common.TosterHandlers;
using CashCanvas.Core.Beans;
using CashCanvas.Core.Beans.Enums;
using CashCanvas.Core.ViewModel;
using CashCanvas.Services.Interfaces;
using CashCanvas.Web.Extensions;
using CashCanvas.Web.Middlewares;
using Microsoft.AspNetCore.Mvc;

namespace CashCanvas.Web.Controllers;

public class BillController(IBillService billService) : Controller
{
    private readonly IBillService _billService = billService;
    public async Task<IActionResult> Index()
    {
        Guid UserId = User.GetUserId();
        if (UserId == Guid.Empty)
        {
            ToasterHelper.SetToastMessage(TempData, MessageHelper.GetNotFoundMessage(Constants.USER), ResponseStatus.Warning);
            return RedirectToAction("Index", "Home");
        }
        PaginationDetails pagination = new();
        List<BillViewModel> bills = await _billService.GetAllBillsAsync(UserId, pagination);
        ViewBag.Pagination = pagination;
        return View(bills);
    }
    public async Task<IActionResult> GetPaymentHistoryTable(Guid billId)
    {
        List<PaymentHistoryViewModel> history = await _billService.GetPaymentHistory(billId);
        return PartialView("PartialViews/_BillHistoryTableBody", history);
    }
    [HttpGet]
    [Route("Bill/Create")]
    public IActionResult CreateBill()
    {
        Guid userId = User.GetUserId();
        if (userId == Guid.Empty)
        {
            ToasterHelper.SetToastMessage(TempData, MessageHelper.GetNotFoundMessage(Constants.USER), ResponseStatus.Warning);
            return RedirectToAction("Index", "Home");
        }
        return View(new BillViewModel { UserId = userId });
    }

    [HttpPost]
    [Route("Bill/Create")]
    public async Task<IActionResult> CreateBill(BillViewModel billViewModel)
    {
        if (billViewModel == null)
        {
            ToasterHelper.SetToastMessage(TempData, Messages.WARNING_INVALID_INPUT, ResponseStatus.Warning);
            return View(billViewModel);
        }
        billViewModel.UserId = User.GetUserId();
        ResponseResult<int> response = await _billService.CreateBillAsync(billViewModel);
        ToasterHelper.SetToastMessage(TempData, response.Message, response.Status);
        return RedirectToAction("Index");
    }

    [HttpGet]
    [Route("Bill/EditBill/{id}")]
    public async Task<IActionResult> EditBill(Guid id)
    {
        if (id == Guid.Empty)
        {
            ToasterHelper.SetToastMessage(TempData, Messages.WARNING_INVALID_INPUT, ResponseStatus.Warning);
            return RedirectToAction("Index");
        }
        BillViewModel? bill = await _billService.GetBillByIdAsync(id);
        if (bill == null)
        {
            ToasterHelper.SetToastMessage(TempData, MessageHelper.GetNotFoundMessage(Constants.BILL), ResponseStatus.Warning);
            return RedirectToAction("Index");
        }
        return View(bill);
    }

    [HttpPost]
    [Route("Bill/UpdateBill")]
    public async Task<IActionResult> UpdateBill(BillViewModel bill)
    {
        if (bill == null || bill.BillId == Guid.Empty)
        {
            ToasterHelper.SetToastMessage(TempData, Messages.WARNING_INVALID_INPUT, ResponseStatus.Warning);
            return View(bill);
        }
        ResponseResult<bool> response = await _billService.UpdateBillAsync(bill);
        ToasterHelper.SetToastMessage(TempData, response.Message, response.Status);
        return View("EditBill", bill);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("Bill/PayBill")]
    public async Task<IActionResult> BillPaid(PaymentCreationViewModel paymentViewModel)
    {
        if (paymentViewModel == null || paymentViewModel.BillId == Guid.Empty)
        {
            ToasterHelper.SetToastMessage(TempData, Messages.WARNING_INVALID_INPUT, ResponseStatus.Warning);
            return RedirectToAction("Index");
        }
        paymentViewModel.UserId = User.GetUserId();
        ResponseResult<bool> response = await _billService.PayBillAsync(paymentViewModel);
        ToasterHelper.SetToastMessage(TempData, response.Message, response.Status);
        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteBill(Guid id)
    {
        if (id == Guid.Empty)
        {
            ToasterHelper.SetToastMessage(TempData, Messages.WARNING_INVALID_INPUT, ResponseStatus.Warning);
            return RedirectToAction("Index");
        }
        ResponseResult<bool> response = await _billService.MoveToTrashAsync(id);
        ToasterHelper.SetToastMessage(TempData, response.Message, response.Status);
        return RedirectToAction("Index");
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
        List<BillExportViewModel> bills = await _billService.GetExortBillsAsync(UserId);

        string partialView = await this.RenderPartialViewToString(Constants.EXPORT_BILLS_VIEW, bills);

        byte[] pdfArray = StringToPdfConverter.CreatePdf(partialView);
        string fileName = $"Bills_{DateTime.Now:dd-MM-yy}.pdf";

        return File(pdfArray, Constants.PDF_CONTENT_TYPE, fileName);
    }
}
