using CashCanvas.Common.ConstantHandler;
using CashCanvas.Common.ExceptionHandler;
using Microsoft.AspNetCore.Mvc;

namespace CashCanvas.Web.Controllers;

public class ErrorHandlerController : Controller
{
    [Route("ErrorHandler/HttpStatusCodeHandler/{encodedData}")]
    public IActionResult HttpStatusCodeHandler(string encodedData)
    {
        try
        {
            // Decode the encoded data
            string decodedData = DataProtectionHelper.Decode(encodedData);
            string[] parts = decodedData.Split('|');

            // Validate the decoded data
            if (parts.Length != 2 || !int.TryParse(parts[0], out int statusCode))
            {
                throw new FormatException("Invalid encoded data format.");
            }

            SetErrorViewData(statusCode, parts[1]);
        }
        catch (FormatException ex)
        {
            // Log format-related errors
            Console.WriteLine($"Error decoding data: {ex.Message}");
            ViewData["StatusCode"] = StatusCodes.Status400BadRequest; // Bad Request
            ViewData["Message"] = ExceptionHelper.GetErrorMessage(StatusCodes.Status400BadRequest);
        }
        catch (Exception ex)
        {
            // Log unexpected errors
            Console.WriteLine($"Unexpected error: {ex.Message}");
            ViewData["StatusCode"] = StatusCodes.Status500InternalServerError; // Internal Server Error
            ViewData["Message"] = ExceptionHelper.GetErrorMessage(StatusCodes.Status500InternalServerError);
        }


        TempData[Constants.LAYOUT_VARIABLE_NAME] = string.Empty;

        return View("Error");
    }

    [Route("ErrorHandler/PageNotFound")]
    public IActionResult PageNotFound()
    {
        try
        {
            SetErrorViewData(StatusCodes.Status404NotFound);
        }
        catch (FormatException ex)
        {
            // Log format-related errors
            Console.WriteLine($"Error decoding data: {ex.Message}");
            ViewData["StatusCode"] = StatusCodes.Status400BadRequest; // Bad Request
            ViewData["Message"] = ExceptionHelper.GetErrorMessage(StatusCodes.Status400BadRequest);
        }
        catch (Exception ex)
        {
            // Log unexpected errors
            Console.WriteLine($"Unexpected error: {ex.Message}");
            ViewData["StatusCode"] = StatusCodes.Status500InternalServerError; // Internal Server Error
            ViewData["Message"] = ExceptionHelper.GetErrorMessage(StatusCodes.Status500InternalServerError);
        }


        TempData[Constants.LAYOUT_VARIABLE_NAME] = string.Empty;

        return View("Error");
    }

    private void SetErrorViewData(int statusCode, string message = null)
    {
        ViewData["StatusCode"] = statusCode;
        ViewData["Message"] = string.IsNullOrWhiteSpace(message)
            ? ExceptionHelper.GetErrorMessage(statusCode)
            : message;
        TempData[Constants.LAYOUT_VARIABLE_NAME] = string.Empty;
    }
}
