using System.Linq.Expressions;
using Zorro.Data;
using Zorro.Data.Interfaces;

namespace Zorro.Query.Essentials.ModelRepository;

public static class GetAllQuery
{
    public static ArgQueryContext<IEnumerable<TEntity>> GetAll<TEntity>(this QueryContext context)
        where TEntity : class, IEntity
    {
        var repo = context.GetService<ModelRepository<TEntity>>();
        if (repo is null)
        {
            throw new Exception();
        }

        var items = repo.GetAll(context.inclusion);

        context.TryLogElapsedTime(nameof(GetAllQuery));

        return context.PassArg(items);
    }

    public static ArgQueryContext<IEnumerable<TEntity>> GetAll<TEntity>(this QueryContext context, Expression<Func<TEntity, bool>> predicate)
        where TEntity : class, IEntity
    {
        var repo = context.GetService<ModelRepository<TEntity>>();
        if (repo is null)
        {
            throw new Exception();
        }

        var items = repo.GetAll(predicate, context.inclusion);

        context.TryLogElapsedTime(nameof(GetAllQuery));

        return context.PassArg(items);
    }
}