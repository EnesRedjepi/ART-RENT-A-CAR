using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TextileShop.Services.Models;

namespace TextileShop.Web.ViewModels;

public class CheckoutViewModel
{
    public AddressViewModel Address { get; set; } = new();

    [StringLength(500)]
    [Display(Name = "Order notes")]
    public string? Notes { get; set; }

    [BindNever]
    public CartModel Cart { get; set; } = new(Array.Empty<CartLine>());

    public CheckoutInput ToInput() => new() { Address = Address.ToData(), Notes = Notes };
}
