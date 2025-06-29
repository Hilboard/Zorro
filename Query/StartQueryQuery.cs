using Microsoft.AspNetCore.Mvc;

namespace Zorro.Query;

public static class StartQueryQuery
{
    public static ANY_TUPLE StartQuery(this ControllerBase controller)
    {
        QueryContext context = new QueryContext(controller.HttpContext);

        return (context, null);
    }
}