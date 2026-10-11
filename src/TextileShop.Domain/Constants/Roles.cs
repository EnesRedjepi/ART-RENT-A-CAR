using TextileShop.Domain.Enums;

namespace TextileShop.Domain.Constants;

public static class Roles
{
    public const string Administrator = "Administrator";
    public const string RetailCustomer = "RetailCustomer";
    public const string WholesaleCustomer = "WholesaleCustomer";
    public const string Customers = RetailCustomer + "," + WholesaleCustomer;

    public static string FromAccountType(AccountType accountType) => accountType switch
    {
        AccountType.Administrator => Administrator,
        AccountType.Wholesale => WholesaleCustomer,
        _ => RetailCustomer
    };

    public static AccountType ToAccountType(string role) => role switch
    {
        Administrator => AccountType.Administrator,
        WholesaleCustomer => AccountType.Wholesale,
        _ => AccountType.Retail
    };
}
