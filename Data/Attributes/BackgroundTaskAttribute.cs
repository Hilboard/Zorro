namespace Zorro.Data.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class BackgroundTaskAttribute : Attribute
{
    public readonly TimeSpan DELAY;

    public BackgroundTaskAttribute(TimeSpan DELAY)
    {
        this.DELAY = DELAY;
    }

    /// <summary>
    /// All arguments are additive
    /// </summary>
    public BackgroundTaskAttribute(
        double MILLISECONDS = 0,
        double SECONDS = 0,
        double MINUTES = 0,
        double HOURS = 0,
        double DAYS = 0
    )
    {
        DELAY = TimeSpan.FromMilliseconds(MILLISECONDS) +
            TimeSpan.FromSeconds(SECONDS) +
            TimeSpan.FromMinutes(MINUTES) +
            TimeSpan.FromHours(HOURS) +
            TimeSpan.FromDays(DAYS);
    }
}