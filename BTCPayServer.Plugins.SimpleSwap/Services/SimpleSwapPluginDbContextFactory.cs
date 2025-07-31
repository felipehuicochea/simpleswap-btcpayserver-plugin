using BTCPayServer.Plugins.SimpleSwap.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace BTCPayServer.Plugins.SimpleSwap.Services
{
    public class SimpleSwapPluginDbContextFactory : IDesignTimeDbContextFactory<SimpleSwapPluginDbContext>
    {
        public SimpleSwapPluginDbContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<SimpleSwapPluginDbContext>();
            var connectionString = config.GetConnectionString("DefaultConnection");
            optionsBuilder.UseNpgsql(connectionString);

            return new SimpleSwapPluginDbContext(optionsBuilder.Options, true);
        }

        public void ConfigureBuilder(DbContextOptionsBuilder builder)
        {
            // This method will be called by the plugin system to configure the DbContext
            // The actual connection string will be provided by BTCPayServer
        }
    }
} 