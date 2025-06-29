using System.Linq.Expressions;
using Zorro.Data;
using Zorro.Data.Interfaces;

namespace Zorro.Query.Essentials.ModelRepository;

public static class GetAllWhereQuery
{
    public static (QueryContext, IEnumerable<TEntity>) GetAllWhere<TEntity>(this ANY_TUPLE carriage, Expression<Func<TEntity, bool>> predicate)
        where TEntity : class, IEntity
    {
        var repo = carriage.context.http.RequestServices.GetService<ModelRepository<TEntity>>();
        if (repo is null)
        {
            throw new Exception();
        }

        return (carriage.context, repo.GetAll(predicate, carriage.context.inclusionContext));
    }
}