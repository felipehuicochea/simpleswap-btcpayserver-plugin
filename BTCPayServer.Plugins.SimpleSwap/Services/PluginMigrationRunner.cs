using BTCPayServer.Plugins.SimpleSwap.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System;
using System.Threading;
using System.Threading.Tasks;


namespace BTCPayServer.Plugins.SimpleSwap.Services
{
    public class PluginMigrationRunner : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<PluginMigrationRunner> _logger;

        public PluginMigrationRunner(IServiceProvider serviceProvider, ILogger<PluginMigrationRunner> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<SimpleSwapPluginDbContext>();
                await context.Database.MigrateAsync(stoppingToken);
                _logger.LogInformation("SimpleSwap plugin database migration completed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during SimpleSwap plugin database migration");
            }
        }
    }
} 
