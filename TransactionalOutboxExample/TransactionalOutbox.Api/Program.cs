
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Shared.TransactionalOutbox;
using System.Text.Json;
using TransactionalOutbox.Api.WebApi;
using TransactionalOutbox.Core.Infrastructure;
using TransactionalOutbox.Infrastructure;
using TransactionalOutbox.Infrastructure.Repositories;

namespace TransactionalOutbox.Api;

public class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		builder.ConfigureServices();
		var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
					   ?? throw new Exception("Default SQL connection string is null");

		var app = builder.Build();
		app.ConfigureMiddlewares(connectionString);
		//var running = app.RunAsync();
		//if (running.IsFaulted)
		//{
		//	throw running.Exception;
		//}
		app.Run();

	}
}

public static class ProgramExtensions
{
	public static void ConfigureServices(this WebApplicationBuilder builder)
	{
		// Add services to the container.
		builder.Services.AddAuthorization();
		
		builder.Services.AddDbContext<PersonDbContext>(options =>
		{
			options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
		});

		builder.Services.AddScoped<IPersonRepository, PersonRepository>();
		builder.Services.AddScoped<IPersonUnitOfWork, PersonUnitOfWork>();

		builder.Services.AddSingleton(new JsonSerializerOptions
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			WriteIndented = true
		});
		
		builder.Services.RegisterTransactionalOutbox(() => new TransactionalOutboxEfCoreConfiguration("dbo", "TransactionalOutbox"));


		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen();



		// Add DbMigrator to the dependency injection container
		builder.Services.AddDbMigrator();
	}

	private static Action<TransactionalOutboxEfCoreConfiguration> ConfigurePersonTransactionalOutbox()
	{
		return options =>
		{
			new TransactionalOutboxEfCoreConfiguration("dbo", "Person");
		};
	}


public static void ConfigureMiddlewares(this WebApplication app, string connectionString)
{
	// Use DbMigrator to migrate the database
	app.UseDbMigrator(connectionString);

	app.UseHttpsRedirection();

	app.UseAuthorization();

	app.MapPersonApi();

	if (app.Environment.IsDevelopment())
	{
		app.UseExceptionHandler(errorApp =>
		{
			errorApp.Run(async context =>
			{
				var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

				await context.Response.WriteAsJsonAsync(new { contextFeature?.Error });
				await context.Response.CompleteAsync();
			});
		});

		// Configure the HTTP request pipeline.
		app.UseSwagger();
		app.UseSwaggerUI();
	}
}
	}
