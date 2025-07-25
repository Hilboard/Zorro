using Microsoft.AspNetCore.Identity;
using Zorro.Data;
using Zorro.Data.Interfaces;
using Zorro.Middlewares;

namespace Zorro.Query.Essentials.Auth;

public static class GetUserQuery
{
    public static ArgQueryContext<TUser> GetUser<TUser, TKey>(this QueryContext context)
        where TUser : IdentityUser<TKey>, IEntity, new()
        where TKey : IEquatable<TKey>, IParsable<TKey>
    {
        if (context.User is null)
        {
            throw new QueryException(statusCode: StatusCodes.Status500InternalServerError);
        }

        var userManager = context.GetService<UserManager<TUser>>();
        bool parseStatus = TKey.TryParse(userManager.GetUserId(context.User), null, out TKey? userId);
        if (parseStatus is false || userId is null)
        {
            throw new QueryException(statusCode: StatusCodes.Status401Unauthorized);
        }

        var userRepo = context.GetService<ModelRepository<TUser>>();
        var user = userRepo.Find(u => u.Id.Equals(userId));
        if (user is null)
        {
            throw new Exception();
        }

        return context.PassArg(user);
    }
}