using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TransactionalOutbox.Infrastructure;

namespace Shared.TransactionalOutbox;
public class OutboxRelayBackgroundService : BackgroundService
{
	private const int _delayInSeconds = 10;
	private readonly ILogger<OutboxRelayBackgroundService> _logger;
	private readonly TimeSpan _checkInterval = TimeSpan.FromSeconds(_delayInSeconds);


	public OutboxRelayBackgroundService(IServiceProvider services, ILogger<OutboxRelayBackgroundService> logger)
	{
		Services = services;
		_logger = logger;
	}
	public IServiceProvider Services { get; }

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		_logger.LogInformation("OutboxRelayBackgroundService started.");
		await CreateScopedService(stoppingToken);
	}

	private async Task CreateScopedService(CancellationToken stoppingToken)
	{
		using (var scope = Services.CreateScope())
		{
			var scopedProcessingService =
				scope.ServiceProvider
					.GetRequiredService<IScopedTransactionalOutboxBackgroundProcessingService>();

			await scopedProcessingService.NotifySubscribers(stoppingToken);
		}
	}

	public override async Task StopAsync(CancellationToken stoppingToken)
	{
		_logger.LogInformation(
			"OutboxRelayBackgroundService Hosted Service is stopping.");

		await base.StopAsync(stoppingToken);
	}
}

