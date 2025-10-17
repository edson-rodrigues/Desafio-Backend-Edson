namespace Mottu.Shared.Extensions;

public static class StringExtensions
{
    public static string ToSnakeCase(this string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        return string.Concat(
            input.Select((c, i) => i > 0 && char.IsUpper(c)
                ? "_" + c.ToString().ToLower()
                : c.ToString().ToLower()));
    }

    public static string OnlyDigits(this string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        return new string(input.Where(char.IsDigit).ToArray());
    }
}

