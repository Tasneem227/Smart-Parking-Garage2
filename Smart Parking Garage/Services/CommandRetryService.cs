using Microsoft.EntityFrameworkCore;
using Smart_Parking_Garage.Constants;

namespace Smart_Parking_Garage.Services;

public class CommandRetryService(IServiceScopeFactory scopeFactory): BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();

            var context =scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var deviceService =scope.ServiceProvider.GetRequiredService<IDeviceService>();

            await HandleCommandsAsync(context,deviceService,stoppingToken);

            await Task.Delay(TimeSpan.FromSeconds(5),stoppingToken);
        }
    }

    private static async Task HandleCommandsAsync(ApplicationDbContext context,IDeviceService deviceService,CancellationToken cancellationToken)
    {
        var commands = await context.DeviceCommands.Where(x => x.Status != "done").ToListAsync(cancellationToken);

        foreach (var command in commands)
        {
            var timeoutSeconds =command.CommandType == DeviceCommands.CaptureImage? 30: 15;

            if (command.Status == "failed")
            {

                if (command.RetryCount == 0)
                {
                    await deviceService.RetryCommandAsync(
                        command,
                        cancellationToken);
                }
                continue;
            }

            if (command.Status == "pending")
            {
                var timeoutReached = DateTimeOffset.UtcNow > command.LastSentAt.AddSeconds(timeoutSeconds);

                if (!timeoutReached)
                    continue;

                if (command.RetryCount == 0)
                {
                    await deviceService.RetryCommandAsync(command,cancellationToken);
                }
            }
        }
    }
}