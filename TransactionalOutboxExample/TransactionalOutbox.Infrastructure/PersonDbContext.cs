using Microsoft.EntityFrameworkCore;
using Shared.TransactionalOutbox.Models;
using StrictId.EFCore;
using TransactionalOutbox.Core.Models;

namespace TransactionalOutbox.Infrastructure;
public class PersonDbContext : DbContext
{

	private readonly Action<ModelBuilder> _transactionalOutboxModelBuilder;
	public DbSet<OutboxEntry> OutboxEntries { get; set; }

	public PersonDbContext(PersonTransactionalOutbox transactionalOutbox, DbContextOptions<PersonDbContext> options) : base(options)
	{
		_transactionalOutboxModelBuilder = transactionalOutbox.Attach(this);
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
		_transactionalOutboxModelBuilder(modelBuilder);

		// your configuration ...
		modelBuilder.Entity<Person>(b =>
		{
			b.HasKey(x => x.Id);
			b.Property(p => p.Id)
			.HasStrictIdValueGenerator();
			b.OwnsMany(x => x.Addresses, a =>
			{
				a.WithOwner().HasForeignKey("PersonId");
				a.HasKey(x => x.Id);
				a.Property(p => p.Id)
				.HasStrictIdValueGenerator();
			});
		});
	}
	protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
	{
		configurationBuilder.Properties<DateTime>().HaveConversion<DateTimeOffset>();
		configurationBuilder.ConfigureStrictId();
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
#if DEBUG
		optionsBuilder.EnableDetailedErrors();
		optionsBuilder.EnableSensitiveDataLogging();
#endif
	}
}
