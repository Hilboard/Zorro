using System.Collections.Concurrent;
using System.Diagnostics;
using System.Security.Claims;

namespace Zorro.Query;

public class QueryContext
{
    public InclusionContext inclusion { get; private set; }

    protected HttpContext http { get; init; } = null!;
    protected ILogger? logger { get; set; }

    public ClaimsPrincipal User => http.User;
    public IRequestCookieCollection RequestCookies => http.Request.Cookies;
    public IResponseCookies ResponseCookies => http.Response.Cookies;

    private readonly ConcurrentDictionary<Type, object> _serviceCache = new();
    private readonly Stopwatch _stopwatch;

    public QueryContext(HttpContext http, InclusionContext inclusion, ILogger? logger = null)
    {
        this.http = http;
        this.inclusion = inclusion;
        this.logger = logger;
        _stopwatch = Stopwatch.StartNew();
    }

    public TService GetService<TService>() where TService : notnull
    {
        return (TService)_serviceCache.GetOrAdd(typeof(TService), http.RequestServices.GetRequiredService);
    }

    public void TryLogElapsedTime(string queryName)
    {
        logger?.LogInformation($"Zorro query {queryName} executed: {_stopwatch.ElapsedMilliseconds} ms.");
    }

    public ArgQueryContext<TOutArg> PassArg<TOutArg>(TOutArg outArg)
    {
        return new ArgQueryContext<TOutArg>(outArg, http, inclusion)
        {
            logger = logger
        };
    }
}