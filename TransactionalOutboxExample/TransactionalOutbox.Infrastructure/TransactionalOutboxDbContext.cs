using Microsoft.EntityFrameworkCore;
using Shared.TransactionalOutbox.Models;

namespace TransactionalOutbox.Infrastructure;
internal class TransactionalOutboxDbContext : DbContext
{
	public TransactionalOutboxDbContext(DbContextOptions<TransactionalOutboxDbContext> options) : base(options)
	{
	}

	internal DbSet<OutboxEntry> TransactionalOutbox { get; set; } = null!;
}
