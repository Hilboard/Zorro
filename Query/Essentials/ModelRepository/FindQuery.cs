using System.Linq.Expressions;
using Zorro.Data;
using Zorro.Data.Interfaces;
using Zorro.Middlewares;

namespace Zorro.Query.Essentials.ModelRepository;

public static class FindQuery
{
    public static (QueryContext, TEntity) Find<TEntity>(this ANY_TUPLE carriage, Expression<Func<TEntity, bool>> predicate)
        where TEntity : class, IEntity
    {
        var repo = carriage.context.http.RequestServices.GetService<ModelRepository<TEntity>>();
        if (repo is null)
        {
            throw new Exception();
        }

        var entity = repo.Find(predicate, carriage.context.inclusionContext);
        if (entity is null)
        {
            throw new QueryException(statusCode: StatusCodes.Status404NotFound);
        }

        return (carriage.context, entity);
    }
}