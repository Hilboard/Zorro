using System.Reflection;
using Zorro.Data;
using Zorro.Data.Interfaces;

namespace Zorro;

public class BackgroundTaskHandler<TEntity> : BackgroundService
    where TEntity : class, IEntity
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ModelRepository<TEntity> _modelRepository;
    private readonly TimeSpan _executionDelay;
    private readonly MethodInfo _backgroundTask;

    public BackgroundTaskHandler(
        IServiceScopeFactory serviceScopeFactory,
        ModelRepository<TEntity> modelRepository,
        TimeSpan executionDelay,
        MethodInfo backgroundTask
    )
    {
        _serviceScopeFactory = serviceScopeFactory;
        _modelRepository = modelRepository;
        _executionDelay = executionDelay;
        _backgroundTask = backgroundTask;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var args = new object[] { scope.ServiceProvider };
                foreach (var model in _modelRepository.GetAll())
                    _backgroundTask.Invoke(model, args);
            }

            await Task.Delay(_executionDelay, stoppingToken);
        }
    }
}