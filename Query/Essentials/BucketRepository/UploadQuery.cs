using Zorro.Data;
using Zorro.Middlewares;

namespace Zorro.Query.Essentials.BucketRepository;

public static class UploadQuery
{
    public static HttpQueryContext Upload<TBucketRepository, TClient, TBucket, TItem>(
        this HttpQueryContext context,
        IFormFile file,
        string filePath
    )
        where TBucketRepository : BucketRepository<TClient, TBucket, TItem>
    {
        var bucketRepo = context.GetService<TBucketRepository>();

        var uploadStatus = bucketRepo.UploadAsync(file, filePath).GetAwaiter().GetResult();
        if (uploadStatus is false)
        {
            throw new QueryException(statusCode: StatusCodes.Status500InternalServerError);
        }

        context.TryLogElapsedTime(nameof(UploadQuery));
        return context;
    }
}