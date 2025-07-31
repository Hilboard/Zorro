using Microsoft.AspNetCore.Mvc;

namespace Zorro.Query;

public static class EndQuery
{
    public static IActionResult End(this HttpQueryContext input)
    {
        return new OkResult();
    }
}
