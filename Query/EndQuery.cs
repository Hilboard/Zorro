using Microsoft.AspNetCore.Mvc;

namespace Zorro.Query;

public static class EndQuery
{
    public static IActionResult End(this QueryContext input)
    {
        return new OkResult();
    }
}
