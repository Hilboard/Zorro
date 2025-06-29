namespace Zorro.Data;

public abstract class BucketRepository<TClient, TBucket, TItem>
{
    protected readonly TClient client;
    public string bucket { get; set; }

    public BucketRepository(TClient client, string bucket)
    {
        this.client = client;
        this.bucket = bucket;
    }

    public abstract string GetFullPath(string path);

    public abstract Task<bool> UploadAsync(IFormFile file, string path);

    public abstract Task<bool> UploadAsync(Stream fileStream, string path, string contentType);

    public abstract Task<bool> DeleteAsync(string path);

    public abstract Task<bool?> HasAsync(string path);

    public abstract Task<List<TItem>?> ListAsync(string path);

    public abstract Task<List<TBucket>?> ListBucketsAsync();

    public abstract Task<bool?> BucketExistsAsync(string bucket);
}