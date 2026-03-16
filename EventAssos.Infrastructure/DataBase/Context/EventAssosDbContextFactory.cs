using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;

namespace EventAssos.Infrastructure.DataBase.Context;

public class EventAssosDbContextFactory : IDesignTimeDbContextFactory<EventAssosDbContext>
{
  public EventAssosDbContext CreateDbContext(string[] args)
  {
    string path = Path.Combine(Directory.GetCurrentDirectory(), "..", "EventAssos.Api");

    IConfigurationRoot configuration = new ConfigurationBuilder()
      .SetBasePath(path)
      .AddJsonFile("appsettings.Development.json")
      .Build();

    var connectionString = configuration.GetConnectionString("Default");

    var optionBuilder = new DbContextOptionsBuilder<EventAssosDbContext>();
    optionBuilder.UseSqlServer(connectionString);
    return new EventAssosDbContext(optionBuilder.Options);
  }
}