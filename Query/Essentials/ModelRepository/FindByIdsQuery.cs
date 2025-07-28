using Zorro.Data;
using Zorro.Data.Interfaces;
using Zorro.Middlewares;

namespace Zorro.Query.Essentials.ModelRepository;

public static class FindByIdsQuery
{
    public static ArgQueryContext<IEnumerable<TEntity>> FindByIds<TEntity>(this QueryContext context, int[] ids)
        where TEntity : class, IEntity
    {
        var repo = context.GetService<ModelRepository<TEntity>>();
        if (repo is null)
        {
            throw new Exception();
        }

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
}