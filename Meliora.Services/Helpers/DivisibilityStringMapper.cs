namespace Meliora.Services.Helpers;

public static class DivisibilityStringMapper
{
    /// <summary>
    /// Maps an integer value to a string based on its divisibility.
    /// </summary>
    /// <param name="value">The integer value to be mapped.</param>
    /// <returns>The mapped string value.</returns>
    public static string MapToString(this int value)
    {
        return (value % 3) switch
        {
            0 when value % 7 == 0 => "Nursing Meliora",
            0 => "Nursing",
            _ => value % 7 == 0 ? "Meliora" : value.ToString()
        };
    }
}
