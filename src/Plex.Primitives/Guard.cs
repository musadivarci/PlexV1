namespace Plex.Primitives;

/// <summary>
/// Provides defensive precondition check helpers for domain entities and arguments.
/// </summary>
public static class Guard
{
    /// <summary>
    /// Ensures that a string is not null, empty, or whitespace.
    /// </summary>
    /// <param name="value">The string value to evaluate.</param>
    /// <param name="parameterName">The name of the parameter being guarded.</param>
    /// <returns>The verified non-empty string value.</returns>
    /// <exception cref="ArgumentException">Thrown when the string is null or whitespace.</exception>
    public static string NotNullOrWhiteSpace(string? value, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Value cannot be null, empty or whitespace.", parameterName);

        return value;
    }

    /// <summary>
    /// Ensures that an object reference is not null.
    /// </summary>
    /// <typeparam name="T">The reference type.</typeparam>
    /// <param name="value">The value to validate.</param>
    /// <param name="parameterName">The name of the parameter.</param>
    /// <returns>The verified non-null instance.</returns>
    /// <exception cref="ArgumentNullException">Thrown when value is null.</exception>
    public static T NotNull<T>(T? value, string parameterName) where T : class =>
        value ?? throw new ArgumentNullException(parameterName);

    /// <summary>
    /// Ensures that an integer is greater than zero.
    /// </summary>
    /// <param name="value">The numeric value.</param>
    /// <param name="parameterName">The name of the parameter.</param>
    /// <returns>The verified positive integer.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when value is less than or equal to zero.</exception>
    public static int Positive(int value, string parameterName)
    {
        if (value <= 0)
            throw new ArgumentOutOfRangeException(parameterName, value, "Value must be greater than zero.");

        return value;
    }
}