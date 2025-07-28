using Microsoft.AspNetCore.Mvc;

namespace Zorro.Query;

public static class StartQueryQuery
{
    public static ArgQueryContext<object?> StartQuery(this ControllerBase controller, ILogger? logger = null)
    {
        return new QueryContext(controller.HttpContext, new InclusionContext(), logger).PassArg<object?>(null);
    }
}