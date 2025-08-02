using System.Linq.Expressions;
using Zorro.Data;
using Zorro.Data.Interfaces;
using Zorro.Middlewares;

namespace Zorro.Query.Essentials.ModelRepository;

public static class FindQuery
{
    public static ArgHttpQueryContext<TEntity> Find<TEntity>(
        this HttpQueryContext context, 
        Expression<Func<TEntity, bool>> predicate
    )
        where TEntity : class, IEntity
    {
        var repo = context.GetService<ModelRepository<TEntity>>();

        var entity = repo.Find(predicate, context.inclusion);
        if (entity is null)
        {
            throw new QueryException(statusCode: StatusCodes.Status404NotFound);
        }

        context.TryLogElapsedTime(nameof(FindQuery));
        return context.PassArg(entity);
    }

    public static ArgHttpQueryContext<TPartialEntity> Find<TEntity, TPartialEntity>(
        this HttpQueryContext context, 
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TPartialEntity>> selector
    )
        where TEntity : class, IEntity
        where TPartialEntity : class, IEntity
    {
        var repo = context.GetService<ModelRepository<TEntity>>();

        var entity = repo.Find(predicate, selector, context.inclusion);
        if (entity is null)
        {
            throw new QueryException(statusCode: StatusCodes.Status404NotFound);
        }

        context.TryLogElapsedTime(nameof(FindQuery));
        return context.PassArg(entity);
    }
}