using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TextileShop.Domain.Constants;
using TextileShop.Services.Common;
using TextileShop.Web.Localization;

namespace TextileShop.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = Roles.Administrator)]
public abstract class AdminControllerBase : Controller
{
    /// <summary>Sets the flash message: <paramref name="successKey"/> is an English resource key, formatted with <paramref name="arguments"/>.</summary>
    protected void SetResultMessage(ServiceResult result, string successKey, params object[] arguments)
    {
        TempData[result.Succeeded ? "Success" : "Error"] = result.Succeeded ? this.Tr(successKey, arguments) : this.ErrorText(result);
    }
}
