using Microsoft.AspNetCore.Identity;
using Zorro.Data;
using Zorro.Data.Interfaces;
using Zorro.Middlewares;

namespace Zorro.Query.Essentials.Auth;

public static class GetUserQuery
{
    public static (QueryContext, TUser) GetUser<TUser, TKey>(this ANY_TUPLE carriage)
        where TUser : IdentityUser<TKey>, IEntity, new()
        where TKey : IEquatable<TKey>, IParsable<TKey>
    {
        if (carriage.context.http.User is null)
        {
            throw new QueryException(statusCode: StatusCodes.Status500InternalServerError);
        }

        var userManager = carriage.context.http.RequestServices.GetRequiredService<UserManager<TUser>>();
        bool parseStatus = TKey.TryParse(userManager.GetUserId(carriage.context.http.User), null, out TKey? userId);
        if (parseStatus is false || userId is null)
        {
            throw new QueryException(statusCode: StatusCodes.Status401Unauthorized);
        }

        var userRepo = carriage.context.http.RequestServices.GetRequiredService<ModelRepository<TUser>>();
        var user = userRepo.Find(u => u.Id.Equals(userId));
        if (user is null)
        {
            throw new Exception();
        }

        return (carriage.context, user);
    }
}