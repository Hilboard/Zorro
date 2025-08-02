using System.Linq.Expressions;
using Zorro.Data;
using Zorro.Data.Interfaces;
using Zorro.Middlewares;

namespace Zorro.Query.Essentials.ModelRepository;

public static class FindByIdsQuery
{
    public static ArgHttpQueryContext<IEnumerable<TEntity>> FindByIds<TEntity>(this HttpQueryContext context, int[] ids)
        where TEntity : class, IEntity
    {
        var repo = context.GetService<ModelRepository<TEntity>>();

        var entities = repo.FindByIds(ids, context.inclusion);
        if (entities.Count() != ids.Length)
        {
            var idsNotFound = ids.Where(id => entities.Any(e => e.Id == id) is false);
            var errorField = ("ids", idsNotFound.Select(id => $"Entity with id {id} not found.").ToArray());
            throw new QueryException(statusCode: StatusCodes.Status404NotFound, fields: [errorField]);
        }

        context.TryLogElapsedTime(nameof(FindByIdsQuery));
        return context.PassArg(entities);
    }

    public static ArgHttpQueryContext<IEnumerable<TPartialEntity>> FindByIds<TEntity, TPartialEntity>(
        this HttpQueryContext context, 
        int[] ids,
        Expression<Func<TEntity, TPartialEntity>> selector
    )
        where TEntity : class, IEntity
        where TPartialEntity : class, IEntity
    {
        var repo = context.GetService<ModelRepository<TEntity>>();

        var entities = repo.FindByIds(ids, selector, context.inclusion);
        if (entities.Count() != ids.Length)
        {
            var idsNotFound = ids.Where(id => entities.Any(e => e.Id == id) is false);
            var errorField = ("ids", idsNotFound.Select(id => $"Entity with id {id} not found.").ToArray());
            throw new QueryException(statusCode: StatusCodes.Status404NotFound, fields: [errorField]);
        }

        context.TryLogElapsedTime(nameof(FindByIdsQuery));
        return context.PassArg(entities);
    }
}