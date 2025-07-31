using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BTCPayServer.Plugins.SimpleSwap.Model;
using BTCPayServer.Plugins.SimpleSwap.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BTCPayServer.Plugins.SimpleSwap.Services
{
    public class SimpleSwapPluginService
    {
        private readonly SimpleSwapPluginDbContextFactory _contextFactory;
        private readonly ILogger<SimpleSwapPluginService> _logger;

        public SimpleSwapPluginService(SimpleSwapPluginDbContextFactory contextFactory, ILogger<SimpleSwapPluginService> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<SimpleSwapSettings> GetStoreSettings(string storeId)
        {
            using var context = _contextFactory.CreateContext();
            var settings = await context.SimpleSwapSettings.FirstOrDefaultAsync(s => s.StoreId == storeId);
            return settings ?? new SimpleSwapSettings { StoreId = storeId };
        }

        public async Task UpdateSettings(SimpleSwapSettings settings)
        {
            using var context = _contextFactory.CreateContext();
            var existing = await context.SimpleSwapSettings.FirstOrDefaultAsync(s => s.StoreId == settings.StoreId);
            
            if (existing != null)
            {
                existing.ApiKey = settings.ApiKey;
                existing.UserId = settings.UserId;
                existing.AcceptedCryptos = settings.AcceptedCryptos;
                existing.Enabled = settings.Enabled;
                existing.DefaultRefundAddress = settings.DefaultRefundAddress;
                existing.MinimumSwapAmount = settings.MinimumSwapAmount;
                existing.MaximumSwapAmount = settings.MaximumSwapAmount;
                context.SimpleSwapSettings.Update(existing);
            }
            else
            {
                context.SimpleSwapSettings.Add(settings);
            }
            
            await context.SaveChangesAsync();
        }

        public async Task<List<SimpleSwapTransaction>> GetStoreTransactions(string storeId)
        {
            using var context = _contextFactory.CreateContext();
            return await context.SimpleSwapTransactions
                .Where(t => t.StoreId == storeId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task CreateTransaction(string storeId, string swapId, string fromAddress, decimal amount, string fromCrypto, string toCrypto, string toAddress)
        {
            using var context = _contextFactory.CreateContext();
            var transaction = new SimpleSwapTransaction
            {
                Id = Guid.NewGuid().ToString(),
                SwapId = swapId,
                FromCrypto = fromCrypto,
                ToCrypto = toCrypto,
                FromAmount = (float)amount,
                FromAddress = fromAddress,
                ToAddress = toAddress,
                Status = "pending",
                CreatedAt = DateTime.UtcNow,
                StoreId = storeId
            };
            
            context.SimpleSwapTransactions.Add(transaction);
            await context.SaveChangesAsync();
        }

        public async Task UpdateTransactionStatus(string swapId, string status, float? toAmount = null)
        {
            using var context = _contextFactory.CreateContext();
            var transaction = await context.SimpleSwapTransactions.FirstOrDefaultAsync(t => t.SwapId == swapId);
            
            if (transaction != null)
            {
                transaction.Status = status;
                if (toAmount.HasValue)
                    transaction.ToAmount = toAmount.Value;
                if (status == "completed")
                    transaction.CompletedAt = DateTime.UtcNow;
                
                context.SimpleSwapTransactions.Update(transaction);
                await context.SaveChangesAsync();
            }
        }

        public async Task<SimpleSwapTransaction> GetTransaction(string swapId)
        {
            using var context = _contextFactory.CreateContext();
            return await context.SimpleSwapTransactions.FirstOrDefaultAsync(t => t.SwapId == swapId);
        }
    }
} 