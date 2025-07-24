using Microsoft.AspNetCore.Mvc;

namespace Zorro.Query;

public static class StartQueryQuery
{
    public static ArgQueryContext<object?> StartQuery(this ControllerBase controller)
    {
        return new QueryContext(controller.HttpContext, new Dictionary<string, bool?>()).PassArg<object?>(null);
    }
}