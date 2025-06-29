using Zorro.Data;
using Zorro.Data.Interfaces;
using Zorro.Middlewares;

namespace Zorro.Query.Essentials.ModelRepository;

public static class UpdateQuery
{
    public static (QueryContext, TEntity) Update<TEntity, TForm>(this ANY_TUPLE carriage, int id, TForm form)
        where TEntity : class, IEntity, IUpdateable<TForm>
        where TForm : struct
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

        return (carriage.context, entityInstance).Update(form);
    }

    public static (QueryContext, TEntity) Update<TEntity, TForm>(this (QueryContext context, TEntity entity) carriage, TForm form)
        where TEntity : class, IEntity, IUpdateable<TForm>
        where TForm : struct
    {
        var repo = carriage.context.http.RequestServices.GetService<ModelRepository<TEntity>>();
        if (repo is null)
        {
            throw new Exception();
        }
        repo.context.ChangeTracker.Clear();

        if (carriage.entity.UpdateFill(form) is false)
        {
            throw new QueryException(statusCode: StatusCodes.Status400BadRequest);
        }

        var updateStatus = repo.Update(ref carriage.entity, carriage.context.inclusionContext);
        if (updateStatus is false)
        {
            throw new QueryException(statusCode: StatusCodes.Status400BadRequest);
        }

        return (carriage.context, carriage.entity);
    }
}