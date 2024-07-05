using Shared.TransactionalOutbox;
using Shared.TransactionalOutbox.MessageRelayService;
using TransactionalOutbox.Infrastructure;

namespace TransactionalOutbox.Api;
public static class ServiceCollectionExtensions
{
    public static void RegisterTransactionalOutbox(this IServiceCollection serviceCollection, Func<TransactionalOutboxEfCoreConfiguration> configure)
    {
        serviceCollection.AddSingleton<TransactionalOutboxEfCoreConfiguration>(configure());

        // register the transactionalOutbox
        serviceCollection.AddScoped<PersonTransactionalOutbox>();

        // register your message relay service notifier. We don't have any yet, so maybe just create some no-op implementation
		serviceCollection.AddScoped<IMessageRelayServiceNotifier, MessageRelayServiceNotifier<PersonDbContext>>();

		// register the background service that will relay the messages if they where for some reason not sent
		// by the event from the transactional outbox
		serviceCollection.AddScoped<OutboxRelayBackgroundService>();
		serviceCollection.AddHostedService<OutboxRelayBackgroundService>();
		serviceCollection.AddScoped<IScopedTransactionalOutboxBackgroundProcessingService, ScopedTransactionalOutboxBackgroundProcessingService>();

		// If you noticed, transactional outbox needs to know how to serialize messages. For now, I just inject JsonSerializerOptions, so be sure to register that as well. Maybe we should add some other interface here, so that you can serialize however you like.
	}
}
