namespace Plex.Primitives;

/// <summary>
/// Represents the outcome of an operation, containing success status and optional error details.
/// </summary>
public class Result
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Result"/> class.
    /// </summary>
    /// <param name="isSuccess">Indicates whether the result is successful.</param>
    /// <param name="error">The error associated with a failure result.</param>
    protected Result(bool isSuccess, Error error)
    {
        if (isSuccess && !error.IsNone)
            throw new ArgumentException("A successful result cannot contain an error.", nameof(error));

        if (!isSuccess && error.IsNone)
            throw new ArgumentException("A failed result must contain an error.", nameof(error));

        IsSuccess = isSuccess;
        Error = error;
    }

    /// <summary>
    /// Gets a value indicating whether the operation succeeded.
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Gets a value indicating whether the operation failed.
    /// </summary>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// Gets the error details if the operation failed.
    /// </summary>
    public Error Error { get; }

    /// <summary>
    /// Creates a successful result without a payload.
    /// </summary>
    public static Result Success() => new(true, Error.None);

    /// <summary>
    /// Creates a failed result with the specified error.
    /// </summary>
    /// <param name="error">The error detailing the failure.</param>
    public static Result Failure(Error error) => new(false, error);

    /// <summary>
    /// Creates a successful typed result with the specified payload value.
    /// </summary>
    /// <typeparam name="T">The payload type.</typeparam>
    /// <param name="value">The payload value.</param>
    public static Result<T> Success<T>(T value) => new(value, true, Error.None);

    /// <summary>
    /// Creates a failed typed result with the specified error.
    /// </summary>
    /// <typeparam name="T">The expected payload type.</typeparam>
    /// <param name="error">The error detailing the failure.</param>
    public static Result<T> Failure<T>(Error error) => new(default, false, error);
}

/// <summary>
/// Represents the outcome of an operation that produces a value of type <typeparamref name="T"/> on success.
/// </summary>
/// <typeparam name="T">The value type.</typeparam>
public sealed class Result<T> : Result
{
    private readonly T? _value;

    internal Result(T? value, bool isSuccess, Error error)
        : base(isSuccess, error)
    {
        _value = value;
    }

    /// <summary>
    /// Gets the successful result payload value.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when attempting to access value on a failed result.</exception>
    public T Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("The value of a failed result cannot be accessed.");

    /// <summary>
    /// Executes either the success or failure transformation function based on the result state.
    /// </summary>
    /// <typeparam name="TResult">The output return type.</typeparam>
    /// <param name="onSuccess">Function invoked if the result succeeded.</param>
    /// <param name="onFailure">Function invoked if the result failed.</param>
    public TResult Match<TResult>(Func<T, TResult> onSuccess, Func<Error, TResult> onFailure) =>
        IsSuccess ? onSuccess(Value) : onFailure(Error);

    /// <summary>
    /// Transforms the success value using the specified mapper function.
    /// </summary>
    /// <typeparam name="TResult">The new target value type.</typeparam>
    /// <param name="mapper">The mapping function to transform the value.</param>
    public Result<TResult> Map<TResult>(Func<T, TResult> mapper) =>
        IsSuccess ? Result.Success(mapper(Value)) : Result.Failure<TResult>(Error);
}