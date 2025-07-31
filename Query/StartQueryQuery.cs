using Microsoft.AspNetCore.Mvc;

namespace Zorro.Query;

public static class StartQueryQuery
{
    public static ArgHttpQueryContext<object?> StartQuery(this ControllerBase controller, ILogger? logger = null)
    {
        return new HttpQueryContext(controller.HttpContext, new InclusionContext(), logger).PassArg<object?>(null);
    }
}