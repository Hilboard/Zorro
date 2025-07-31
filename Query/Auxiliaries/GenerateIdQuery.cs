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
        if (repo is null)
        {
            throw new Exception();
        }

        int idAttempt;
        do
        {
            idAttempt = _rng.Next(10000000, 99999999);
        }
        while (repo.FindById(idAttempt) is not null);

        id = idAttempt;

        return context.PassArg(idAttempt);
    }
}