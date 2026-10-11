using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using TextileShop.Domain.Enums;
using TextileShop.Services.Common;
using TextileShop.Services.Models;

namespace TextileShop.Web.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string GetUserId(this ClaimsPrincipal user) => user.FindFirstValue(ClaimTypes.NameIdentifier)!;
}

public static class UrlHelperExtensions
{
    public static string ProductImage(this IUrlHelper url, string? fileName) =>
        string.IsNullOrEmpty(fileName)
            ? url.Content("~/images/placeholder.svg")
            : url.Content($"~/uploads/products/{fileName}");

    public static string WithPage(this IUrlHelper url, int page)
    {
        var request = url.ActionContext.HttpContext.Request;
        var query = QueryHelpers.ParseQuery(request.QueryString.Value);
        query["page"] = new StringValues(page.ToString());
        return QueryHelpers.AddQueryString($"{request.PathBase}{request.Path}", query);
    }
}

public static class FormFileExtensions
{
    public static IReadOnlyList<ImageUpload> ToImageUploads(this IFormFileCollection? files) =>
        files is null
            ? Array.Empty<ImageUpload>()
            : files.Where(file => file.Length > 0)
                .Select(file => new ImageUpload(file.FileName, file.Length, file.OpenReadStream()))
                .ToList();
}

public static class StatusStyles
{
    public static string BadgeClass(OrderStatus status) => status switch
    {
        OrderStatus.Pending => "badge-soft badge-warning",
        OrderStatus.Confirmed => "badge-soft badge-info",
        OrderStatus.Processing => "badge-soft badge-info",
        OrderStatus.Shipped => "badge-soft badge-primary",
        OrderStatus.Delivered => "badge-soft badge-success",
        _ => "badge-soft badge-danger"
    };

    public static string BadgeClass(PaymentStatus status) => status switch
    {
        PaymentStatus.Paid => "badge-soft badge-success",
        PaymentStatus.Refunded => "badge-soft badge-danger",
        _ => "badge-soft badge-warning"
    };
}
