using Microsoft.Extensions.Logging;
using Shared.TransactionalOutbox.MessageRelayService;

namespace TransactionalOutbox.Infrastructure;

public class ScopedTransactionalOutboxBackgroundProcessingService : IScopedTransactionalOutboxBackgroundProcessingService
{
	private readonly ILogger _logger;
	private readonly IMessageRelayServiceNotifier _messageRelayServiceNotifier;

	public ScopedTransactionalOutboxBackgroundProcessingService(ILogger<ScopedTransactionalOutboxBackgroundProcessingService> logger, IMessageRelayServiceNotifier messageRelayServiceNotifier)
	{
		_logger = logger;
		_messageRelayServiceNotifier = messageRelayServiceNotifier;
	}

	public int Delay => 1000;
	
	public async Task NotifySubscribers(CancellationToken stoppingToken)
	{
		while (!stoppingToken.IsCancellationRequested)
		{
			_logger.LogInformation("Scoped Processing Service is working");
			await _messageRelayServiceNotifier.Notify();

			await Task.Delay(10000, stoppingToken);
		}
	}
}
