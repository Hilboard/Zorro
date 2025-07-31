using Zorro.Data;
using Zorro.Middlewares;
using Zorro.Query.Essentials.Auth;

namespace Zorro.Query.Essentials.BucketRepository;

public static class DeleteQuery
{
    public static HttpQueryContext Delete<TBucketRepository, TClient, TBucket, TItem>(
        this HttpQueryContext context,
        string filePath
    )
        where TBucketRepository : BucketRepository<TClient, TBucket, TItem>
    {
        var bucketRepo = context.GetService<TBucketRepository>();
        if (bucketRepo is null)
        {
            throw new Exception();
        }

        var hasFile = bucketRepo.HasAsync(filePath).GetAwaiter().GetResult();
        if (hasFile is null)
        {
            throw new QueryException(statusCode: StatusCodes.Status500InternalServerError);
        }
        else if (hasFile is false)
        {
            throw new QueryException(statusCode: StatusCodes.Status404NotFound);
        }

        var deleteStatus = bucketRepo.DeleteAsync(filePath).GetAwaiter().GetResult();
        if (deleteStatus is false)
        {
            throw new QueryException(statusCode: StatusCodes.Status500InternalServerError);
        }

        context.TryLogElapsedTime(nameof(DeleteQuery));

        return context;
    }
}