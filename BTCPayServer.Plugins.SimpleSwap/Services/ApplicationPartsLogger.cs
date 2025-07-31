using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System;
using System.Threading;
using System.Threading.Tasks;


namespace BTCPayServer.Plugins.SimpleSwap.Services
{
    public class ApplicationPartsLogger : BackgroundService
    {
        private readonly ApplicationPartManager _applicationPartManager;
        private readonly ILogger<ApplicationPartsLogger> _logger;

        public ApplicationPartsLogger(ApplicationPartManager applicationPartManager, ILogger<ApplicationPartsLogger> logger)
        {
            _applicationPartManager = applicationPartManager;
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var applicationParts = _applicationPartManager.ApplicationParts.Select(x => x.Name);
            _logger.LogInformation("SimpleSwap plugin loaded with application parts: {ApplicationParts}", string.Join(", ", applicationParts));
            return Task.CompletedTask;
        }
    }
} 
