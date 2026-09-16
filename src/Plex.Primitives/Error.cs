namespace Plex.Primitives;

/// <summary>
/// Represents a structured domain or operational error with an identification code and message.
/// </summary>
/// <param name="Code">The unique identifier code for the error.</param>
/// <param name="Message">The human-readable description of the error.</param>
public readonly record struct Error(string Code, string Message)
{
    /// <summary>
    /// Represents an empty error indicating no error occurred.
    /// </summary>
    public static readonly Error None = new(string.Empty, string.Empty);

    /// <summary>
    /// Gets a value indicating whether this error represents an empty/none error.
    /// </summary>
    public bool IsNone => string.IsNullOrWhiteSpace(Code);

    /// <summary>
    /// Returns a string representation of the error.
    /// </summary>
    public override string ToString() => IsNone ? "None" : $"{Code}: {Message}";
}