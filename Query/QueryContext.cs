global using ANY_TUPLE = (Zorro.Query.QueryContext context, object? lastValue);

namespace Zorro.Query;

public class QueryContext
{
    public IDictionary<string, bool?> inclusionContext { get; private set; }

    public HttpContext http { get; init; } = null!;

    public QueryContext(HttpContext http)
    {
        this.http = http;
        inclusionContext = new Dictionary<string, bool?>();
    }
}