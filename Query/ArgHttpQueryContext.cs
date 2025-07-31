namespace Zorro.Query;

public class ArgHttpQueryContext<TArg> : HttpQueryContext
{
    public TArg arg { get; init; } = default!;

    public ArgHttpQueryContext(TArg arg, HttpContext http, InclusionContext inclusion) : base(http, inclusion)
    {
        this.arg = arg;
    }
}