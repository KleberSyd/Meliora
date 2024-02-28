using Meliora.Services.CookiesKristen.Interfaces;
using Meliora.Services.Helpers;

namespace Meliora.Services.CookiesKristen;

public class MelioraService : IMelioraService
{
    /// <summary>
    /// Gets the divisibility string for the given value.
    /// </summary>
    /// <param name="value">The value to check divisibility for.</param>
    /// <returns>The divisibility string.</returns>
    public async Task<string> GetDivisibilityString(int value)
    {
        return await Task.FromResult(value.MapToString());
    }

    /// <summary>
    /// Counts from the startFrom value up to the maxValue, invoking the onNumberGenerated action for each number generated.
    /// </summary>
    /// <param name="maxValue">The maximum value to count up to.</param>
    /// <param name="startFrom">The starting value.</param>
    /// <param name="onNumberGenerated">The action to invoke for each number generated.</param>
    /// <param name="cancellationToken">The cancellation token to stop the counting process.</param>
    public async Task CountAsync(int maxValue, int startFrom, Action<string, int> onNumberGenerated,
        CancellationToken cancellationToken)
    {
        for (var i = startFrom; i <= maxValue; i++)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                break;
            }

            onNumberGenerated(i.MapToString(), i);

            await Task.Delay(150, cancellationToken);
        }
    }
}
