using Microsoft.AspNetCore.Mvc;
using CashCanvas.Common.ConstantHandler;
using Microsoft.AspNetCore.Authorization;
using CashCanvas.Web.Attributes;

namespace CashCanvas.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [CustomAuthorize]
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult ChangePassword()
    {
        TempData[Constants.LAYOUT_VARIABLE_NAME] = Constants.LOGIN_LAYOUT;
        return View();
    }

}
