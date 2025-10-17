namespace Mottu.Shared.Results;

public sealed class Error
{
    public string Code { get; }
    public string Message { get; }
    public Dictionary<string, string[]>? ValidationErrors { get; }

    private Error(string code, string message, Dictionary<string, string[]>? validationErrors = null)
    {
        Code = code;
        Message = message;
        ValidationErrors = validationErrors;
    }

    public static Error None => new(string.Empty, string.Empty);
    public static Error NullValue => new("NULL_VALUE", "Null value was provided");

    public static Error Create(string code, string message) => new(code, message);

    public static Error Validation(string code, string message, Dictionary<string, string[]>? errors = null)
        => new(code, message, errors);

    public static Error NotFound(string entity, object id)
        => new("NOT_FOUND", $"{entity} with id '{id}' was not found");

    public static Error Conflict(string message)
        => new("CONFLICT", message);

    public static Error BadRequest(string message)
        => new("BAD_REQUEST", message);

    public static Error Unauthorized(string message = "Unauthorized access")
        => new("UNAUTHORIZED", message);
}

