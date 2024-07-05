using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shared.TransactionalOutbox.Models;

namespace Shared.TransactionalOutbox.MessageRelayService;
public class MessageRelayServiceNotifier<TContext> : IMessageRelayServiceNotifier where TContext : DbContext
{
	private readonly IServiceProvider _serviceProvider;
	private readonly ILogger<MessageRelayServiceNotifier<TContext>> _logger;

	public MessageRelayServiceNotifier(IServiceProvider serviceProvider, ILogger<MessageRelayServiceNotifier<TContext>> logger)
	{
		_serviceProvider = serviceProvider;
		_logger = logger;
	}

	public async Task Notify()
	{
		using var scope = _serviceProvider.CreateScope();
		var dbContext = scope.ServiceProvider.GetRequiredService<TContext>();
		var unprocessedMessages = await dbContext.Set<OutboxEntry>()
			.ToListAsync();
		//Maybe we should only process one message at a time, to avoid flooding the service bus
		//Or to make sure that messages that are not broken are sent
		foreach (var message in unprocessedMessages)
		{
			// Publish to the service bus
			await PublishToServiceBusAsync(message);

			// Delete the message from the outbox
			dbContext.Set<OutboxEntry>().Remove(message);
		}

		await dbContext.SaveChangesAsync();
	}

	private Task PublishToServiceBusAsync(OutboxEntry entry)
	{
		// Implement your logic to publish to the service bus
		_logger.LogInformation($"Publishing event to service bus: {entry.EventTypeName} \n With data: {entry.Data}");
		//Do the publishing here
		return Task.CompletedTask;
	}
}

