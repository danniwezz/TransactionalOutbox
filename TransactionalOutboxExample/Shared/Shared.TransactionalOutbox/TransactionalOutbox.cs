using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shared.TransactionalOutbox.MessageRelayService;
using Shared.TransactionalOutbox.Models;
using System.Text.Json;

namespace Shared.TransactionalOutbox;

public abstract class TransactionalOutbox<TContext>
{
	private readonly TransactionalOutboxEfCoreConfiguration _transactionalOutboxEfCoreConfiguration;
	private readonly IMessageRelayServiceNotifier _messageRelayServiceNotifier;
	private readonly ILogger<TransactionalOutbox<TContext>> _logger;
	private readonly JsonSerializerOptions _jsonSerializerOptions;
	private DbContext? _context;
	private bool _hasEventsToNotifyAbout;
	private static bool _modelBuilderConfigured; //  OnModelCreating will only be called once per context type, thats why this is static: https://learn.microsoft.com/en-us/ef/core/modeling/dynamic-model#imodelcachekeyfactory

	public TransactionalOutbox(
		TransactionalOutboxEfCoreConfiguration transactionalOutboxEfCoreConfiguration,
		IMessageRelayServiceNotifier messageRelayServiceNotifier,
		ILogger<TransactionalOutbox<TContext>> logger,
		JsonSerializerOptions jsonSerializerOptions)
	{
		_transactionalOutboxEfCoreConfiguration = transactionalOutboxEfCoreConfiguration;
		_messageRelayServiceNotifier = messageRelayServiceNotifier;
		_logger = logger;
		_jsonSerializerOptions = jsonSerializerOptions;
	}

	/// <summary>
	/// Notifies the message relay service about new messages in the outbox
	/// </summary>
	private void Notify()
	{
		try
		{
			if (_hasEventsToNotifyAbout)
			{
				_hasEventsToNotifyAbout = false;
				_messageRelayServiceNotifier.Notify();
			}
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Failed to notify about outstanding messages");
		}
	}

	/// <summary>
	/// Attaches the outbox to this context. Returns an action to run in OnModelCreating of the context.
	/// </summary>
	public Action<ModelBuilder> Attach(DbContext context)
	{
		if (_context != null)
		{
			throw new Exception("TransactionalOutbox already attached to context");
		}
		_context = context;
		context.SavingChanges += Context_SavingChanges;
		context.SavedChanges += Context_SavedChanges;
		return ConfigureEfCoreModel;
	}

	private void Context_SavingChanges(object? sender, SavingChangesEventArgs e)
	{
		SaveEvents();
	}

	/// <summary>
	/// Saves any entity events from tracked entities implementing IEventSource
	/// to the transactional outbox and clears entities list of events.
	/// Rolls back if SaveChanges fail! (the whole idea of transactional outbox)
	/// </summary>
	private void SaveEvents()
	{
		if (_context == null)
		{
			throw new Exception("TransactionalOutbox not attached to context");
		}
		if (!_modelBuilderConfigured)
		{
			throw new Exception("TransactionalOutbox model not configured");
		}
		var trackedEventSources = _context.ChangeTracker.Entries<IEventSource>();
		var events = trackedEventSources
			.SelectMany(eventsource => eventsource.Entity.Events)
			.Select(@event =>
			new OutboxEntry
			{
				EventTypeName = @event.GetEventName(),
				CreatedAt = DateTime.UtcNow,
				Data = JsonSerializer.Serialize(@event, @event.GetType()!, _jsonSerializerOptions)
			})
			.ToList();

		_context.Set<OutboxEntry>().AddRange(events);
		foreach (var trackedPart in trackedEventSources)
		{
			trackedPart.Entity.ClearEvents();
		}
		_hasEventsToNotifyAbout = events.Count > 0;
	}

	private void Context_SavedChanges(object? sender, SavedChangesEventArgs e)
	{
		Notify();
	}

	/// <summary>
	/// Configures ef core to ignore IEventSource.Events properties on all entities implementing IEventSource
	/// Configures ef core to use the configured table and schema for the outbox entries
	/// </summary>
	/// <param name="modelBuilder"></param>
	private void ConfigureEfCoreModel(ModelBuilder modelBuilder)
	{
		var entityTypes = modelBuilder.Model.GetEntityTypes()
			.Where(t => typeof(IEventSource).IsAssignableFrom(t.ClrType));
		foreach (var entityType in entityTypes)
		{
			var entityTypeBuilder = modelBuilder.Entity(entityType.ClrType);
			entityTypeBuilder.Ignore(nameof(IEventSource.Events));
		}

		modelBuilder.Entity<OutboxEntry>().ToTable(_transactionalOutboxEfCoreConfiguration.Table, _transactionalOutboxEfCoreConfiguration.Schema);
		_modelBuilderConfigured = true;
	}
}
