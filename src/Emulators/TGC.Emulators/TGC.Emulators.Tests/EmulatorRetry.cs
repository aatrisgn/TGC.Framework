namespace TGC.Emulators.Tests;

/// <summary>
/// Bounded retry for emulator readiness checks — first-run container startup
/// (SQL Edge, cert generation, Azurite) can take up to ~30-60s.
/// </summary>
internal static class EmulatorRetry
{
    public static async Task<T> UntilSuccessAsync<T>(Func<Task<T>> action, TimeSpan? timeout = null, TimeSpan? interval = null)
    {
        var deadline = DateTime.UtcNow + (timeout ?? TimeSpan.FromSeconds(60));
        var delay = interval ?? TimeSpan.FromSeconds(2);
        Exception? lastError = null;

        while (DateTime.UtcNow < deadline)
        {
            try
            {
                return await action();
            }
            catch (Exception ex)
            {
                lastError = ex;
                await Task.Delay(delay);
            }
        }

        throw new TimeoutException("Emulator did not become ready in time.", lastError);
    }
}
