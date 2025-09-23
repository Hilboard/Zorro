using Zorro.Data;
using Zorro.Data.Interfaces;
using Zorro.Middlewares;

namespace Zorro.Query.Essentials.ModelRepository;

public static class UpdateQuery
{
    public static ArgHttpQueryContext<TEntity> Update<TEntity, TForm>(this HttpQueryContext context, int id, TForm form)
        where TEntity : class, IEntity, IUpdateable<TForm>
    {
        var repo = context.GetService<ModelRepository<TEntity>>();
        repo.context.ChangeTracker.Clear();

        TEntity? entity = repo.FindById(id);
        if (entity is null)
        {
            throw new QueryException(statusCode: StatusCodes.Status404NotFound);
        }

        if (entity.UpdateFill(form) is false)
        {
            throw new QueryException(statusCode: StatusCodes.Status400BadRequest);
        }

        var updateStatus = repo.Update(ref entity, context.inclusion);
        if (updateStatus is false)
        {
            throw new QueryException(statusCode: StatusCodes.Status400BadRequest);
        }

        context.TryLogElapsedTime(nameof(UpdateQuery));
        return context.PassArg(entity);
    }

    public static ArgHttpQueryContext<TEntity> Update<TEntity, TForm>(this ArgHttpQueryContext<TEntity> context, TForm form)
        where TEntity : class, IEntity, IUpdateable<TForm>
    {
        var repo = context.GetService<ModelRepository<TEntity>>();
        repo.context.ChangeTracker.Clear();

        TEntity entity = context.arg;
        if (entity.UpdateFill(form) is false)
        {
            throw new QueryException(statusCode: StatusCodes.Status400BadRequest);
        }

        var updateStatus = repo.Update(ref entity, context.inclusion);
        if (updateStatus is false)
        {
            throw new QueryException(statusCode: StatusCodes.Status400BadRequest);
        }

        context.TryLogElapsedTime(nameof(UpdateQuery));
        return context.PassArg(entity);
    }
}