using Zorro.Data;
using Zorro.Data.Interfaces;
using Zorro.Middlewares;

namespace Zorro.Query.Essentials.ModelRepository;

public static class RemoveQuery
{
    public static QueryContext Remove<TEntity>(this ArgQueryContext<TEntity> context)
        where TEntity : class, IEntity
    {
        var repo = context.GetService<ModelRepository<TEntity>>();
        if (repo is null)
        {
            throw new Exception();
        }
        repo.context.ChangeTracker.Clear();

        var removeStatus = repo.Remove(context.arg);
        if (removeStatus is false)
        {
            throw new QueryException(statusCode: StatusCodes.Status500InternalServerError);
        }

        context.TryLogElapsedTime(nameof(RemoveQuery));

        return context;
    }

    public static QueryContext Remove<TEntity>(this QueryContext context, int id)
        where TEntity : class, IEntity
    {
        var repo = context.GetService<ModelRepository<TEntity>>();
        if (repo is null)
        {
            throw new Exception();
        }
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