using System.Linq.Expressions;
using Zorro.Data;
using Zorro.Data.Interfaces;
using Zorro.Middlewares;

namespace Zorro.Query.Essentials.ModelRepository;

public static class FindByIdQuery
{
    public static ArgHttpQueryContext<TEntity> FindById<TEntity>(this HttpQueryContext context, int id)
        where TEntity : class, IEntity
    {
        var repo = context.GetService<ModelRepository<TEntity>>();

        var entity = repo.FindById(id, context.inclusion);
        if (entity is null)
        {
            throw new QueryException(statusCode: StatusCodes.Status404NotFound);
        }

        context.TryLogElapsedTime(nameof(FindByIdQuery));
        return context.PassArg(entity);
    }

    public static ArgHttpQueryContext<TPartialEntity> FindById<TEntity, TPartialEntity>(
        this HttpQueryContext context, 
        int id,
        Expression<Func<TEntity, TPartialEntity>> selector
    )
        where TEntity : class, IEntity
        where TPartialEntity : class, IEntity
    {
        var repo = context.GetService<ModelRepository<TEntity>>();

        var entity = repo.FindById(id, selector, context.inclusion);
        if (entity is null)
        {
            throw new QueryException(statusCode: StatusCodes.Status404NotFound);
        }

        context.TryLogElapsedTime(nameof(FindByIdQuery));
        return context.PassArg(entity);
    }
}