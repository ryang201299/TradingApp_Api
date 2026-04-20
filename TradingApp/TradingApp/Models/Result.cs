namespace TradingApp.Models;

/// <summary>
/// Represents responses from methods
/// </summary>
/// <typeparam name="T">Generic object of any type</typeparam>
public class Result<T>
{
    /// <summary>
    /// Represents whether the result is successful
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// The value being returned if successful
    /// </summary>
    public T? Value { get; set; }

    /// <summary>
    /// The error message if unsuccessful
    /// </summary>
    public string? Error { get; set; }

    private Result(bool isSuccess, T? value, string? error)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }

    /// <summary>
    /// Helper method for instantiating successful responses
    /// </summary>
    /// <param name="value">Generic object of any type</param>
    /// <returns>Successful result object</returns>
    public static Result<T> Success(T value) => new Result<T>(true, value, null);

    /// <summary>
    /// Helper method for instantiating unsuccessful responses
    /// </summary>
    /// <param name="error">Error message</param>
    /// <returns>Unsuccessful result object</returns>
    public static Result<T> Failure(string error) => new Result<T>(false, default, error);
}

public class Result
{
    public bool IsSuccess { get; set; }

    public string? Error { get; set; }

    private Result(bool isSuccess, string? error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new Result(true, null);

    public static Result Failure(string error) => new Result(false, error);
}
