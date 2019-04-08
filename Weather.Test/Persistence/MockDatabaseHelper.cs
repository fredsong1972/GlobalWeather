using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Weather.Persistence.Models;

namespace Weather.Test.Persistence
{
    public static class MockDatabaseHelper
    {
        public static DbContextOptions<WeatherDbContext> CreateNewContextOptions(string databaseName)
        {
            //Create a fresh service provider, and therefore a fresh
            // InMemory database instance
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            // Create a new options instance telling the context to use an
            // InMemory database and the new service provider
            var builder = new DbContextOptionsBuilder<WeatherDbContext>();
            builder.UseInMemoryDatabase(databaseName)
                .UseInternalServiceProvider(serviceProvider);

            return builder.Options;
        }
    }
}
