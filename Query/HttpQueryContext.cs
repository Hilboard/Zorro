using System.Collections.Concurrent;
using System.Diagnostics;
using System.Security.Claims;

namespace Zorro.Query;

public class HttpQueryContext
{
    public InclusionContext inclusion { get; private set; }

    protected HttpContext http { get; init; } = null!;
    protected ILogger? logger { get; set; }

    public ContextScopeGetter<ClaimsPrincipal> userGetter = DefaultUserGetter;
    public ClaimsPrincipal User => userGetter(this);

    public ContextScopeGetter<IRequestCookieCollection> requestCookiesGetter = DefaultRequestCookieGetter;
    public IRequestCookieCollection RequestCookies => requestCookiesGetter(this);

    public ContextScopeGetter<IResponseCookies> responseCookiesGetter = DefaultResponseCookieGetter;
    public IResponseCookies ResponseCookies => responseCookiesGetter(this);

    public ContextScopeGetter<IServiceProvider> serviceProviderGetter = DefaultServiceProviderGetter;
    protected IServiceProvider ServiceProvider => serviceProviderGetter(this);

    private readonly Stopwatch _stopwatch = Stopwatch.StartNew();
    private ConcurrentDictionary<Type, object> _serviceCache { get; init; } = new();

    public HttpQueryContext(HttpContext http, InclusionContext inclusion, ILogger? logger = null)
    {
        this.inclusion = inclusion;
        this.http = http;
        this.logger = logger;
    }

    public TService GetService<TService>() where TService : notnull
    {
        return (TService)_serviceCache.GetOrAdd(typeof(TService), ServiceProvider.GetRequiredService);
    }

    public void TryLogElapsedTime(string queryName)
    {
        _stopwatch.Stop();
        logger?.LogInformation($"Executed query {queryName} in {_stopwatch.ElapsedMilliseconds}ms");
        _stopwatch.Restart();
    }

    public ArgHttpQueryContext<TOutArg> PassArg<TOutArg>(TOutArg outArg)
    {
        return new ArgHttpQueryContext<TOutArg>(outArg, http, inclusion)
        {
            logger = logger,
            userGetter = userGetter,
            requestCookiesGetter = requestCookiesGetter,
            responseCookiesGetter = responseCookiesGetter,
            serviceProviderGetter = serviceProviderGetter,
            _serviceCache = _serviceCache,
        };
    }

    private static ClaimsPrincipal DefaultUserGetter(HttpQueryContext context)
    {
        return context.http.User;
    }

    private static IRequestCookieCollection DefaultRequestCookieGetter(HttpQueryContext context)
    {
        return context.http.Request.Cookies;
    }

    private static IResponseCookies DefaultResponseCookieGetter(HttpQueryContext context)
    {
        return context.http.Response.Cookies;
    }

    private static IServiceProvider DefaultServiceProviderGetter(HttpQueryContext context)
    {
        return context.http.RequestServices;
    }

    public delegate TType ContextScopeGetter<TType>(HttpQueryContext context);
}