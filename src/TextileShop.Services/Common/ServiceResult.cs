namespace TextileShop.Services.Common;

public class ServiceResult
{
    public bool Succeeded { get; protected init; }

    public IReadOnlyList<string> Errors { get; protected init; } = Array.Empty<string>();

    public string ErrorMessage => string.Join(" ", Errors);

    public static ServiceResult Success() => new() { Succeeded = true };

    public static ServiceResult Failure(params string[] errors) => new() { Errors = errors };

    public static ServiceResult Failure(IEnumerable<string> errors) => new() { Errors = errors.ToList() };
}

public class ServiceResult<T> : ServiceResult
{
    public T? Value { get; private init; }

    public static ServiceResult<T> Success(T value) => new() { Succeeded = true, Value = value };

    public new static ServiceResult<T> Failure(params string[] errors) => new() { Errors = errors };

    public new static ServiceResult<T> Failure(IEnumerable<string> errors) => new() { Errors = errors.ToList() };
}
