using Microsoft.AspNetCore.Mvc;
using CashCanvas.Common.ConstantHandler;
using Microsoft.AspNetCore.Authorization;
using CashCanvas.Web.Attributes;
using CashCanvas.Core.ViewModel;
using CashCanvas.Services.Interfaces;
using CashCanvas.Core.Beans;
using CashCanvas.Web.Middlewares;

namespace CashCanvas.Web.Controllers;

public class HomeController(IDashboardService dashboardService) : Controller
{
    private readonly IDashboardService _dashboardService = dashboardService;

    [CustomAuthorize]
    public async Task<IActionResult> Index(PaginationDetails paginationDetails)
    {
        Guid userId = User.GetUserId();
        DashboardViewModel dashboardViewModel = await _dashboardService.GetAnalysisData(paginationDetails,userId);
        return View(dashboardViewModel);
    }

    [CustomAuthorize]
    public IActionResult ChangePassword()
    {
        TempData[Constants.LAYOUT_VARIABLE_NAME] = Constants.LOGIN_LAYOUT;
        return View();
    }
    [HttpPost]
    [CustomAuthorize]
    public IActionResult ChangePassword(ChangePasswordViewModel changePasswordRequest)
    {
        return View(changePasswordRequest);
    }
}
