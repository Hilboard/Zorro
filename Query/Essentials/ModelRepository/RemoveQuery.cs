using Zorro.Data;
using Zorro.Data.Interfaces;
using Zorro.Middlewares;

namespace Zorro.Query.Essentials.ModelRepository;

public static class RemoveQuery
{
    public static HttpQueryContext Remove<TEntity>(this ArgHttpQueryContext<TEntity> context)
        where TEntity : class, IEntity
    {
        var repo = context.GetService<ModelRepository<TEntity>>();
        repo.context.ChangeTracker.Clear();

        var removeStatus = repo.Remove(context.arg);
        if (removeStatus is false)
        {
            throw new QueryException(statusCode: StatusCodes.Status500InternalServerError);
        }

        context.TryLogElapsedTime(nameof(RemoveQuery));

        return context;
    }

    public static HttpQueryContext Remove<TEntity>(this HttpQueryContext context, int id)
        where TEntity : class, IEntity
    {
        var repo = context.GetService<ModelRepository<TEntity>>();
        repo.context.ChangeTracker.Clear();

        var entity = repo.FindById(id);
        if (entity is null)
        {
            throw new QueryException(statusCode: StatusCodes.Status404NotFound);
        }

        var removeStatus = repo.Remove(entity);
        if (removeStatus is false)
        {
            throw new QueryException(statusCode: StatusCodes.Status500InternalServerError);
        }

        context.TryLogElapsedTime(nameof(RemoveQuery));

        return context;
    }
}