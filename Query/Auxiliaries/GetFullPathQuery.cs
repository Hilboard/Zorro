using Zorro.Data;

namespace Zorro.Query.Auxiliaries;

public static class GetFullPathQuery
{
    public static HttpQueryContext GetFullPath<TBucketRepository, TClient, TBucket, TItem>(
        this HttpQueryContext context,
        string relativePath,
        out string fullPath
    )
        where TBucketRepository : BucketRepository<TClient, TBucket, TItem>
    {
        var bucketRepo = context.GetService<TBucketRepository>();

        fullPath = bucketRepo.GetFullPath(relativePath);

        context.TryLogElapsedTime(nameof(GetFullPathQuery));
        return context;
    }
}