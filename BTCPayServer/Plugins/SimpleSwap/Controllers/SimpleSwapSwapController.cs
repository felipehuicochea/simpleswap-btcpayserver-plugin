using BTCPayServer.Plugins.SimpleSwap.Model;
using BTCPayServer.Plugins.SimpleSwap.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BTCPayServer.Plugins.SimpleSwap.Controllers
{
    [Route("~/plugins/SimpleSwap")]
    public class SimpleSwapSwapController : Controller
    {
        private readonly SimpleSwapPluginService _pluginService;
        private readonly SimpleSwapService _simpleSwapService;

        public SimpleSwapSwapController(SimpleSwapPluginService pluginService, SimpleSwapService simpleSwapService)
        {
            _pluginService = pluginService;
            _simpleSwapService = simpleSwapService;
        }

        [HttpPost]
        [Route("CreateSwap")]
        public async Task<IActionResult> CreateSwap([FromBody] SwapRequest request)
        {
            try
            {
                var settings = await _pluginService.GetStoreSettings(request.StoreId);
                if (!settings.Enabled)
                {
                    return BadRequest(new { error = "SimpleSwap is not enabled for this store" });
                }

                if (request.Amount < settings.MinimumSwapAmount || request.Amount > settings.MaximumSwapAmount)
                {
                    return BadRequest(new { error = $"Amount must be between {settings.MinimumSwapAmount} and {settings.MaximumSwapAmount}" });
                }

                var swapRequest = new SwapCreationRequest
                {
                    FromCrypto = request.FromCrypto,
                    FromNetwork = request.FromNetwork,
                    FromAmount = request.Amount,
                    ToCrypto = "BTC",
                    ToNetwork = "BTC",
                    ToAddress = request.ToAddress,
                    UserId = settings.UserId,
                    ApiKey = settings.ApiKey,
                    FromRefundAddress = settings.DefaultRefundAddress
                };

                var response = await _simpleSwapService.CreateSwapAsync(swapRequest);

                await _pluginService.CreateTransaction(
                    request.StoreId, 
                    response.SwapId, 
                    response.FromAddress, 
                    (decimal)request.Amount, 
                    request.FromCrypto, 
                    "BTC", 
                    request.ToAddress);

                return Ok(new
                {
                    swapId = response.SwapId,
                    fromAddress = response.FromAddress,
                    amount = response.FromAmount,
                    status = response.StatusMessage
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet]
        [Route("GetSwapStatus/{swapId}")]
        public async Task<IActionResult> GetSwapStatus(string swapId)
        {
            try
            {
                var transaction = await _pluginService.GetTransaction(swapId);
                if (transaction == null)
                {
                    return NotFound(new { error = "Swap not found" });
                }

                return Ok(new
                {
                    swapId = transaction.SwapId,
                    status = transaction.Status,
                    fromCrypto = transaction.FromCrypto,
                    toCrypto = transaction.ToCrypto,
                    fromAmount = transaction.FromAmount,
                    toAmount = transaction.ToAmount,
                    createdAt = transaction.CreatedAt,
                    completedAt = transaction.CompletedAt
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
} 