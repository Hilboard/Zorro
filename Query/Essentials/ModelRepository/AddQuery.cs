using Zorro.Data;
using Zorro.Data.Interfaces;
using Zorro.Middlewares;

namespace Zorro.Query.Essentials.ModelRepository;

public static class AddQuery
{
    public static ArgHttpQueryContext<TEntity> Add<TEntity, TForm>(this HttpQueryContext context, TForm form, int? id = null)
        where TEntity : class, IEntity, IAddable<TForm>, new()
        where TForm : struct
    {
        var repo = context.GetService<ModelRepository<TEntity>>();
        if (repo is null)
        {
            throw new Exception();
        }
        repo.context.ChangeTracker.Clear();

        TEntity? entity = new();
        if (entity.AddFill(form) is false)
        {
            throw new QueryException(statusCode: StatusCodes.Status400BadRequest);
        }

        if (id.HasValue)
        {
            entity.Id = id.Value;
        }

        entity = repo.Add(entity, context.inclusion);
        if (entity is null)
        {
            throw new QueryException(statusCode: StatusCodes.Status500InternalServerError);
        }

        context.TryLogElapsedTime(nameof(AddQuery));

        return context.PassArg(entity);
    }

    public static HttpQueryContext Add<TEntity, TForm>(this HttpQueryContext context, TForm[] forms)
        where TEntity : class, IEntity, IAddable<TForm>, new()
        where TForm : struct
    {
        var repo = context.GetService<ModelRepository<TEntity>>();
        if (repo is null)
        {
            throw new Exception();
        }
        repo.context.ChangeTracker.Clear();

        var entities = forms.Select(f =>
        {
            TEntity entity = new();
            if (entity.AddFill(f) is false)
            {
                throw new QueryException(statusCode: StatusCodes.Status400BadRequest);
            }
            return entity;
        }).ToArray();

        if (repo.AddRange(entities, context.inclusion) is false)
        {
            throw new QueryException(statusCode: StatusCodes.Status500InternalServerError);
        }

        context.TryLogElapsedTime(nameof(AddQuery));

        return context;
    }
}