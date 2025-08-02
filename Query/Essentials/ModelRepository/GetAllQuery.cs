using System.Linq.Expressions;
using Zorro.Data;
using Zorro.Data.Interfaces;

namespace Zorro.Query.Essentials.ModelRepository;

public static class GetAllQuery
{
    public static ArgHttpQueryContext<IEnumerable<TEntity>> GetAll<TEntity>(
        this HttpQueryContext context
    )
        where TEntity : class, IEntity
    {
        var repo = context.GetService<ModelRepository<TEntity>>();

        var items = repo.GetAll(context.inclusion);

        context.TryLogElapsedTime(nameof(GetAllQuery));
        return context.PassArg(items);
    }

    public static ArgHttpQueryContext<IEnumerable<TPartialEntity>> GetAll<TEntity, TPartialEntity>(
        this HttpQueryContext context,
        Expression<Func<TEntity, TPartialEntity>> selector
    )
        where TEntity : class, IEntity
        where TPartialEntity : class, IEntity
    {
        var repo = context.GetService<ModelRepository<TEntity>>();
               
        var items = repo.GetAll(selector, context.inclusion);

        context.TryLogElapsedTime(nameof(GetAllQuery));
        return context.PassArg(items);
    }

    public static ArgHttpQueryContext<IEnumerable<TEntity>> GetAll<TEntity>(
        this HttpQueryContext context, 
        Expression<Func<TEntity, bool>> predicate
    )
        where TEntity : class, IEntity
    {
        var repo = context.GetService<ModelRepository<TEntity>>();

        var items = repo.GetAll(predicate, context.inclusion);

        context.TryLogElapsedTime(nameof(GetAllQuery));
        return context.PassArg(items);
    }

    public static ArgHttpQueryContext<IEnumerable<TPartialEntity>> GetAll<TEntity, TPartialEntity>(
        this HttpQueryContext context,
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TPartialEntity>> selector
    )
        where TEntity : class, IEntity
        where TPartialEntity : class, IEntity
    {
        var repo = context.GetService<ModelRepository<TEntity>>();

        var items = repo.GetAll(predicate, selector, context.inclusion);

        context.TryLogElapsedTime(nameof(GetAllQuery));
        return context.PassArg(items);
    }
}