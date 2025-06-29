using Zorro.Data;
using Zorro.Middlewares;

namespace Zorro.Query.Essentials.BucketRepository;

public static class UploadQuery
{
    public static ANY_TUPLE Upload<TBucketRepository, TClient, TBucket, TItem>(
        this ANY_TUPLE carriage,
        IFormFile file,
        string filePath
    )
        where TBucketRepository : BucketRepository<TClient, TBucket, TItem>
    {
        var bucketRepo = carriage.context.http.RequestServices.GetService<TBucketRepository>();
        if (bucketRepo is null)
        {
            throw new Exception();
        }

        var uploadStatus = bucketRepo.UploadAsync(file, filePath).GetAwaiter().GetResult();
        if (uploadStatus is false)
        {
            throw new QueryException(statusCode: StatusCodes.Status500InternalServerError);
        }

        return carriage;
    }
}