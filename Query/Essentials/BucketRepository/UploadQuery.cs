using Zorro.Data;
using Zorro.Middlewares;

namespace Zorro.Query.Essentials.BucketRepository;

public static class UploadQuery
{
    public static QueryContext Upload<TBucketRepository, TClient, TBucket, TItem>(
        this QueryContext context,
        IFormFile file,
        string filePath
    )
        where TBucketRepository : BucketRepository<TClient, TBucket, TItem>
    {
        var bucketRepo = context.GetService<TBucketRepository>();
        if (bucketRepo is null)
        {
            throw new Exception();
        }

        var uploadStatus = bucketRepo.UploadAsync(file, filePath).GetAwaiter().GetResult();
        if (uploadStatus is false)
        {
            throw new QueryException(statusCode: StatusCodes.Status500InternalServerError);
        }

        return context;
    }
}