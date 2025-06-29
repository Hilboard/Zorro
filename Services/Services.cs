using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Zorro.Data;
using Zorro.Data.Attributes;
using Zorro.Data.Interfaces;

using BackgroundTask = (Zorro.Data.Attributes.BackgroundTaskAttribute attribute, System.Reflection.MethodInfo method);

namespace Zorro.Services;

public static class Services
{
    public static IServiceCollection AddAuthAndIdentity<TUser, TDbContext, TRole, TKey>(
        this IServiceCollection services,
        Func<IServiceCollection, AuthenticationBuilder>? authenticationBuilder = null
    )
        where TUser : IdentityUser<TKey>, IEntity, new()
        where TDbContext : DbContext
        where TKey : IEquatable<TKey>
        where TRole : IdentityRole<TKey>
    {
        services.AddAuthorization(configure =>
        {
        });

        if (authenticationBuilder is not null)
        {
            authenticationBuilder.Invoke(services);
        }
        else
        {
            services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie();
        }

        services
            .AddIdentity<TUser, TRole>()
            .AddEntityFrameworkStores<TDbContext>()
            .AddSignInManager<SignInManager<TUser>>();

        return services;
    }

    public static IServiceCollection AddDatabase<TDbContext>(
        this IServiceCollection services, Action<DbContextOptionsBuilder>? builder, LogLevel minLogLevel = LogLevel.Critical)
        where TDbContext : DbContext
    {
        services.AddDbContext<TDbContext>(options =>
        {
            builder?.Invoke(options);
            options.LogTo(Console.WriteLine, minLogLevel);
        });

        MethodInfo addRepoM = typeof(Services).GetMethod(nameof(AddRepository), BindingFlags.NonPublic | BindingFlags.Static)!;

        foreach (var dbSetProp in typeof(TDbContext).GetProperties())
        {
            var setPropType = dbSetProp.PropertyType;

            if (!setPropType.IsGenericType || setPropType.GetGenericTypeDefinition() != typeof(DbSet<>))
                continue;

            var entityType = setPropType.GetGenericArguments()[0];

            if (!typeof(IEntity).IsAssignableFrom(entityType))
                continue;

            var addRepoMethod = addRepoM.MakeGenericMethod(typeof(TDbContext), entityType);
            addRepoMethod.Invoke(null, [services, dbSetProp.Name]);

            var backgroundTasks = entityType
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(m => m.GetCustomAttribute<BackgroundTaskAttribute>() is not null)
                .Select(m => (m.GetCustomAttribute<BackgroundTaskAttribute>()!, m));

            IHostedService CreateBackgroundService(IServiceProvider serviceProvider, BackgroundTask task)
            {
                var handlerType = typeof(BackgroundTaskHandler<>).MakeGenericType(entityType);
                var repoType = typeof(ModelRepository<>).MakeGenericType(entityType);

                return (IHostedService)Activator.CreateInstance(
                    handlerType,
                    serviceProvider.GetRequiredService<IServiceScopeFactory>(),
                    serviceProvider.GetRequiredService(repoType),
                    task.attribute.DELAY,
                    task.method
                )!;
            }

            foreach (BackgroundTask task in backgroundTasks)
            {
                services.AddSingleton(typeof(IHostedService), sp => CreateBackgroundService(sp, task));
            }
        }

        return services;
    }

    private static void AddRepository<TDbContext, TEntity>(IServiceCollection services, string dbSetPropName)
        where TDbContext : DbContext
        where TEntity : class, IEntity
    {
        services.AddScoped(factory =>
        {
            var dbContext = factory.GetRequiredService<TDbContext>();
            var dbSetProp = dbContext.GetType().GetProperty(dbSetPropName);

            if (dbSetProp?.GetValue(dbContext) is not DbSet<TEntity> dbSet)
                throw new RepositoryInitializeException<ModelRepository<TEntity>>();

            return new ModelRepository<TEntity>(dbContext, ref dbSet);
        });
    }

    public class RepositoryInitializeException<RepoType> : Exception { }
}