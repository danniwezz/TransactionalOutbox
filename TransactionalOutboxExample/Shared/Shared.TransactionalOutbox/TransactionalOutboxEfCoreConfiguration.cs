namespace Shared.TransactionalOutbox;

// To help configuration of ef core.
public record TransactionalOutboxEfCoreConfiguration(string Schema, string Table);