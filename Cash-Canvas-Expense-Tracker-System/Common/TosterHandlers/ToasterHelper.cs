using CashCanvas.Core.Beans.Enums;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace CashCanvas.Common.TosterHandlers;

public class ToasterHelper
    {
       public static void SetToastMessage(ITempDataDictionary tempData, string message, ResponseStatus status)
        {
            tempData["ToastMessage"] = message;
            tempData["ToastStatus"] = status.ToString(); // Convert Enum to String
        }
    }