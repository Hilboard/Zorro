using System.Linq.Expressions;
using Zorro.Data;
using Zorro.Data.Interfaces;
using Zorro.Middlewares;

namespace Zorro.Query.Essentials.ModelRepository;

public static class FindQuery
{
    public static ArgQueryContext<TEntity> Find<TEntity>(this QueryContext context, Expression<Func<TEntity, bool>> predicate)
        where TEntity : class, IEntity
    {
        var repo = context.GetService<ModelRepository<TEntity>>();
        if (repo is null)
        {
            throw new Exception();
        }

        var entity = repo.Find(predicate, context.inclusion);
        if (entity is null)
        {
            throw new QueryException(statusCode: StatusCodes.Status404NotFound);
        }

        context.TryLogElapsedTime(nameof(FindQuery));

        return context.PassArg(entity);
    }
}