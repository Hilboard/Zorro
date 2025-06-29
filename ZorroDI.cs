using Zorro.Secrets;

namespace Zorro;

public static class ZorroDI
{
    public static WebApplicationBuilder? builder { get; private set; }
    public static WebApplication? application { get; private set; }

    public static bool isRunning { get; private set; }

    public static Enums.Environment environment { get; private set; }

    public static SecretsManager? secretsManager { get; set; }

    public static void Main(string[] args) { }

    /// <summary>
    /// Initializes web application without any services attached
    /// </summary>
    /// <param name="builderArgs">Program entry arguments</param>
    /// <returns>Services collection</returns>
    public static IServiceCollection InitRaw(string[] builderArgs, bool logging = true)
    {
        builder = WebApplication.CreateBuilder(builderArgs);

        if (logging)
            builder.Services.AddLogging();

        if (builder.Environment.IsDevelopment())
            environment = Enums.Environment.Development;
        else if (builder.Environment.IsStaging())
            environment = Enums.Environment.Staging;
        else if (builder.Environment.IsProduction())
            environment = Enums.Environment.Production;

        return builder.Services;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <exception cref="BuilderUninitializedException"></exception>
    public static WebApplication Build()
    {
        if (builder is null)
            throw new BuilderUninitializedException();

        application = builder.Build();
        return application;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="url"></param>
    /// <exception cref="ApplicationUninitializedException"></exception>
    public static bool Run(string url = "http://0.0.0.0", int? port = null)
    {
        if (application is null)
            throw new ApplicationUninitializedException();

        if (isRunning)
            return false;

        if (port is null)
            port = DEFAULT_PORT;

        application.Run(string.Join(':', [url, port.ToString()]));
        isRunning = true;

        return true;
    }

    #region Constants

    public const int DEFAULT_PORT = 5000;
    public static readonly string[] ENVIRONMENT_VALUES = ["dev", "staging", "prod"];

    #endregion

    public class BuilderUninitializedException : Exception;
    public class ApplicationUninitializedException : Exception;
    public class SecretsManagerNullReferenceException : Exception;
}