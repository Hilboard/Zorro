namespace Zorro.Secrets;

public abstract class SecretsManager
{
    public abstract string GetSecret(string key);
    public abstract string GetSecret(string path, string key);
}