namespace Plex.Primitives;

public readonly record struct Error(string Code, string Message)
{
    public static readonly Error None = new(string.Empty, string.Empty);

    public bool IsNone => string.IsNullOrWhiteSpace(Code);

    public override string ToString() => IsNone ? "None" : $"{Code}: {Message}";
}