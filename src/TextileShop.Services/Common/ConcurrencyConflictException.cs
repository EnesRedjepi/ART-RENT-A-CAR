namespace TextileShop.Services.Common;

public class ConcurrencyConflictException : Exception
{
    public ConcurrencyConflictException(Exception innerException)
        : base("The data was changed by another operation.", innerException)
    {
    }
}
