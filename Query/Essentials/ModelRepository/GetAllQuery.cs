using Zorro.Data;
using Zorro.Data.Interfaces;

namespace Zorro.Query.Essentials.ModelRepository;

public static class GetAllQuery
{
    public static (QueryContext, IEnumerable<TEntity>) GetAll<TEntity>(this ANY_TUPLE carriage)
        where TEntity : class, IEntity
    {
        var repo = carriage.context.http.RequestServices.GetService<ModelRepository<TEntity>>();
        if (repo is null)
        {
            throw new Exception();
        }

        return (carriage.context, repo.GetAll(carriage.context.inclusionContext));
    }
}