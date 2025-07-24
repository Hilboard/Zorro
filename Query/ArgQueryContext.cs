namespace Zorro.Query;

public class ArgQueryContext<TArg> : QueryContext
{
    public TArg arg { get; init; } = default!;

    public ArgQueryContext(TArg arg, HttpContext http, InclusionContext inclusion) : base(http, inclusion)
    {
        this.arg = arg;
    }
}