using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Smart_Parking_Garage.Constants;

namespace Smart_Parking_Garage.Services;

<<<<<<< HEAD
public class CommandRetryService(IServiceScopeFactory scopeFactory, ILogger<CommandRetryService> logger) : BackgroundService
=======
public class CommandRetryService(IServiceScopeFactory scopeFactory, ILogger<CommandRetryService> logger  ) : BackgroundService
>>>>>>> aa9200ef2d84e766901b4ec568d8d6b93405ee74
{

    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
    private readonly ILogger<CommandRetryService> _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var deviceService = scope.ServiceProvider.GetRequiredService<IDeviceService>();

            await HandleCommandsAsync(context, deviceService, stoppingToken);

            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }

<<<<<<< HEAD
    private async Task HandleCommandsAsync(ApplicationDbContext context,IDeviceService deviceService,CancellationToken cancellationToken)
=======
    private  async Task HandleCommandsAsync(ApplicationDbContext context,IDeviceService deviceService,CancellationToken cancellationToken)
>>>>>>> aa9200ef2d84e766901b4ec568d8d6b93405ee74
    {
        var commands = await context.DeviceCommands.Where(x => x.Status != "done").ToListAsync(cancellationToken);

        foreach (var command in commands)
        {
            var timeoutSeconds =
                command.CommandType == DeviceCommands.CaptureImage ? 30 : 15;

            // failed ACK
            if (command.Status == "failed")
            {
                if (command.RetryCount == 0)
                {
                    try
                    {
<<<<<<< HEAD
                        await deviceService.RetryCommandAsync(command,cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(
                            ex,
                            "Failed to retry command {CommandId}",
                            command.CommandId);
=======
                        await deviceService.RetryCommandAsync(command, cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to retry command {CommandId}", command.Id);
>>>>>>> aa9200ef2d84e766901b4ec568d8d6b93405ee74
                    }
                }

                continue;
            }

            // no ACK yet
            if (command.Status == "pending")
            {
                var timeoutReached =DateTimeOffset.UtcNow > command.LastSentAt.AddSeconds(timeoutSeconds);

                if (!timeoutReached)
                    continue;

                if (command.RetryCount == 0)
                {
                    try
                    {
<<<<<<< HEAD
                        await deviceService.RetryCommandAsync( command, cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError( ex,"Failed to retry command {CommandId}",command.CommandId);
=======
                        await deviceService.RetryCommandAsync(command, cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to retry command {CommandId}", command.Id);
>>>>>>> aa9200ef2d84e766901b4ec568d8d6b93405ee74
                    }
                }
            }
        }
    }
}