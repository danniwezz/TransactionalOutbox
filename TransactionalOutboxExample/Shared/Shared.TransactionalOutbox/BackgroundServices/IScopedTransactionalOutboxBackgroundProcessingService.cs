namespace TransactionalOutbox.Infrastructure;

public interface IScopedTransactionalOutboxBackgroundProcessingService
{
	/// <summary>
	/// The delay between each time the service is run
	/// </summary>
	public int Delay { get; }
	/// <summary>
	/// Notifies the subscribers about new messages in the outbox
	/// </summary>
	/// <param name="stoppingToken"></param>
	/// <returns></returns>
	Task NotifySubscribers(CancellationToken stoppingToken);
}
