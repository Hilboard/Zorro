using Zorro.Data;
using Zorro.Data.Interfaces;
using Zorro.Middlewares;

namespace Zorro.Query.Essentials.ModelRepository;

public static class AddQuery
{
    public static (QueryContext, TEntity) Add<TEntity, TForm>(this ANY_TUPLE carriage, TForm form, int? id = null)
        where TEntity : class, IEntity, IAddable<TForm>, new()
        where TForm : struct
    {
        TEntity entity = new();
        if (entity.AddFill(form) is false)
        {
            throw new QueryException(statusCode: StatusCodes.Status400BadRequest);
        }

        if (id.HasValue)
        {
            entity.Id = id.Value;
        }

        var repo = carriage.context.http.RequestServices.GetService<ModelRepository<TEntity>>();
        if (repo is null)
        {
            throw new Exception();
        }
        repo.context.ChangeTracker.Clear();

        var entityInstance = repo.Add(entity, carriage.context.inclusionContext);
        if (entityInstance is null)
        {
            throw new QueryException(statusCode: StatusCodes.Status500InternalServerError);
        }

        return (carriage.context, entityInstance);
    }
}