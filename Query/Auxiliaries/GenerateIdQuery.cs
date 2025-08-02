using Zorro.Data;
using Zorro.Data.Interfaces;

namespace Zorro.Query.Auxiliaries;

public static class GenerateIdQuery
{
    private static readonly Random _rng = new Random();

    public static ArgHttpQueryContext<int> GenerateId<TEntity>(
        this HttpQueryContext context,
        out int id
    )
        where TEntity : class, IEntity
    {
        var repo = context.GetService<ModelRepository<TEntity>>();

        int idAttempt;
        do
        {
            idAttempt = _rng.Next(10000000, 99999999);
        }
        while (repo.HasId(idAttempt));

        id = idAttempt;

        context.TryLogElapsedTime(nameof(GenerateIdQuery));
        return context.PassArg(idAttempt);
    }
}