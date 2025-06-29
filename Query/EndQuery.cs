using Microsoft.AspNetCore.Mvc;

namespace Zorro.Query;

public static class EndQuery
{
    public static IActionResult End(this ANY_TUPLE input)
    {
        return new OkResult();
    }
}
