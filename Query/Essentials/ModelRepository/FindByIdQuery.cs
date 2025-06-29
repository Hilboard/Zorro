using Zorro.Data;
using Zorro.Data.Interfaces;
using Zorro.Middlewares;

namespace Zorro.Query.Essentials.ModelRepository;

public static class FindByIdQuery
{
    public static (QueryContext, TEntity) FindById<TEntity>(this ANY_TUPLE carriage, int id)
        where TEntity : class, IEntity
    {
        var repo = carriage.context.http.RequestServices.GetService<ModelRepository<TEntity>>();
        if (repo is null)
        {
            throw new Exception();
        }

        var entity = repo.FindById(id, carriage.context.inclusionContext);
        if (entity is null)
        {
            throw new QueryException(statusCode: StatusCodes.Status404NotFound);
        }

        return (carriage.context, entity);
    }
}