using Zorro.Data;
using Zorro.Data.Interfaces;
using Zorro.Middlewares;

namespace Zorro.Query.Essentials.ModelRepository;

public static class RemoveQuery
{
    public static ANY_TUPLE Remove<TEntity>(this ANY_TUPLE carriage, int id)
        where TEntity : class, IEntity
    {
        var repo = carriage.context.http.RequestServices.GetService<ModelRepository<TEntity>>();
        if (repo is null)
        {
            throw new Exception();
        }
        repo.context.ChangeTracker.Clear();

        var entityInstance = repo.FindById(id);
        if (entityInstance is null)
        {
            throw new QueryException(statusCode: StatusCodes.Status404NotFound);
        }

        var removeStatus = repo.Remove(entityInstance);
        if (removeStatus is false)
        {
            throw new QueryException(statusCode: StatusCodes.Status500InternalServerError);
        }

        return (carriage.context, null);
    }

    public static ANY_TUPLE Remove<TEntity>(this (QueryContext context, TEntity entity) carriage)
        where TEntity : class, IEntity
    {
        return carriage.Remove<TEntity>(carriage.entity.Id);
    }
}