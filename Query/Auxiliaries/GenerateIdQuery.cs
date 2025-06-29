using Zorro.Data;
using Zorro.Data.Interfaces;

namespace Zorro.Query.Auxiliaries;

public static class GenerateIdQuery
{
    private static readonly Random _rng = new Random();

    public static (QueryContext, int) GenerateId<TEntity>(
        this ANY_TUPLE carriage, 
        out int id
    )
        where TEntity : class, IEntity
    {
        var repo = carriage.context.http.RequestServices.GetService<ModelRepository<TEntity>>();
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

        return (carriage.context, idAttempt);
    }
}