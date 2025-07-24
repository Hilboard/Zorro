using System.Collections.Concurrent;
using System.Security.Claims;

namespace Zorro.Query;

public class QueryContext
{
    public InclusionContext inclusion { get; private set; }

    protected HttpContext http { get; init; } = null!;

    public ClaimsPrincipal User => http.User;

    private readonly ConcurrentDictionary<Type, object> _serviceCache = new();

    public QueryContext(HttpContext http, InclusionContext inclusion)
    {
        this.http = http;
        this.inclusion = inclusion;
    }

    public TService GetService<TService>() where TService : notnull
    {
        return (TService)_serviceCache.GetOrAdd(typeof(TService), http.RequestServices.GetRequiredService);
    }

    public ArgQueryContext<TOutArg> PassArg<TOutArg>(TOutArg outArg)
    {
        return new ArgQueryContext<TOutArg>(outArg, http, inclusion);
    }
}