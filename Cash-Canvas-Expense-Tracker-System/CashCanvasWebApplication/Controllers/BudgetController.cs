using CashCanvas.Common.ConstantHandler;
using CashCanvas.Common.TosterHandlers;
using CashCanvas.Core.Beans;
using CashCanvas.Core.Beans.Enums;
using CashCanvas.Core.DTOs;
using CashCanvas.Core.ViewModel;
using CashCanvas.Services.Interfaces;
using CashCanvas.Web.Middlewares;
using Microsoft.AspNetCore.Mvc;

namespace CashCanvas.Web.Controllers;

public class BudgetController(IBudgetService budgetService) : Controller
{
    private readonly IBudgetService _budgetService = budgetService;

    public async Task<IActionResult> Index()
    {
        Guid UserId = User.GetUserId();
        if (UserId == Guid.Empty)
        {
            ToasterHelper.SetToastMessage(TempData, MessageHelper.GetNotFoundMessage(Constants.USER), ResponseStatus.Warning);
            return RedirectToAction("Index", "Home");
        }
        List<BudgetViewModel> budgets = await _budgetService.GetBudgetListAsync(UserId);
        return View(budgets);
    }
    [HttpGet]
    [Route("Budget/Create")]
    public async Task<IActionResult> CreateBudget()
    {
        Guid userId = User.GetUserId();
        if (userId == Guid.Empty)
        {
            ToasterHelper.SetToastMessage(TempData, MessageHelper.GetNotFoundMessage(Constants.USER), ResponseStatus.Warning);
            return RedirectToAction("Index", "Home");
        }
        ViewBag.Categories = await _budgetService.GetCategoryListAsync(userId);
        return View(new BudgetViewModel { UserId = userId });
    }

    [HttpPost]
    public async Task<IActionResult> AddBudget(BudgetViewModel newBuget)
    {
        ResponseResult<List<CategoryViewModel>> response = new();
        newBuget.UserId = User.GetUserId();
        if (newBuget == null)
        {
            ToasterHelper.SetToastMessage(TempData, Messages.WARNING_INVALID_INPUT, ResponseStatus.Warning);
            return View(newBuget);
        }
        response = await _budgetService.AddBudgetAsync(newBuget);
        ToasterHelper.SetToastMessage(TempData, response.Message, response.Status);
        return RedirectToAction("Index");
    }

    [HttpGet]
    [Route("Budget/Edit/{budgetId}")]
    public async Task<IActionResult> EditBudget(Guid budgetId)
    {
        Guid userId = User.GetUserId();
        if (userId == Guid.Empty)
        {
            ToasterHelper.SetToastMessage(TempData, MessageHelper.GetNotFoundMessage(Constants.USER), ResponseStatus.Warning);
            return RedirectToAction("Index", "Home");
        }
        BudgetViewModel budget = await _budgetService.GetBudgetByIdAsync(budgetId);
        if (budget == null)
        {
            ToasterHelper.SetToastMessage(TempData, MessageHelper.GetNotFoundMessage(Constants.BUDGET), ResponseStatus.Warning);
            return RedirectToAction("Index");
        }
        ViewBag.Categories = await _budgetService.GetCategoryListAsync(userId);
        return View(budget);
    }
    [HttpPost]
    public async Task<IActionResult> UpdateBudget(BudgetViewModel budget)
    {
        if (budget == null || budget.BudgetId == Guid.Empty)
        {
            ToasterHelper.SetToastMessage(TempData, Messages.WARNING_INVALID_INPUT, ResponseStatus.Warning);
            return RedirectToAction("Index");
        }
        ResponseResult<bool> response = await _budgetService.UpdateBudgetAsync(budget);
        ToasterHelper.SetToastMessage(TempData, response.Message, response.Status);
        return View("EditBudget", budget);
    }
    [HttpPost]
    [Route("Budget/MoveToTrash/{budgetId}")]
    public async Task<IActionResult> MoveToTrash(Guid budgetId)
    {
        if (budgetId == Guid.Empty)
        {
            ToasterHelper.SetToastMessage(TempData, Messages.WARNING_INVALID_INPUT, ResponseStatus.Warning);
            return RedirectToAction("Index");
        }
        ResponseResult<bool> response = await _budgetService.DeleteBudgetAsync(budgetId);
        ToasterHelper.SetToastMessage(TempData, response.Message, response.Status);
        return RedirectToAction("Index");

    }

}
