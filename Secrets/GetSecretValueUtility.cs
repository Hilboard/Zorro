using static Zorro.ZorroSetup;

namespace Zorro.Secrets;

public static class GetSecretValueUtility
{
    public static string GetSecretValue(
        string key)
    {
        if (secretsManager is null)
            throw new SecretsManagerNullReferenceException();
        return secretsManager.GetSecret(key);
    }

    public static string GetSecretValue(
        string path, string key)
    {
        if (secretsManager is null)
            throw new SecretsManagerNullReferenceException();
        return secretsManager.GetSecret(path, key);
    }

    public static TSecret GetSecretValue<TSecret>(
        string key, Func<string, TSecret> parser)
    {
        if (secretsManager is null)
            throw new SecretsManagerNullReferenceException();
        return parser(secretsManager.GetSecret(key));
    }

    public static TSecret GetSecretValue<TSecret>(
        string path, string key, Func<string, TSecret> parser)
    {
        if (secretsManager is null)
            throw new SecretsManagerNullReferenceException();
        return parser(secretsManager.GetSecret(path, key));
    }
}