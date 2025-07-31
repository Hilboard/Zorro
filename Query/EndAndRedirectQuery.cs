using Microsoft.AspNetCore.Mvc;

namespace Zorro.Query;

public static class EndAndRedirectQuery
{
    public static IActionResult EndAndDirectTo(this HttpQueryContext context, string URL)
    {
        return new OkObjectResult(new { next = URL });
    }
}