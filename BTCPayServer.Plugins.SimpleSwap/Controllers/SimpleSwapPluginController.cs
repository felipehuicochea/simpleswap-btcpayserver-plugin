using BTCPayServer.Abstractions.Constants;
using BTCPayServer.Client;
using BTCPayServer.Plugins.SimpleSwap.Model;
using BTCPayServer.Plugins.SimpleSwap.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;


using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BTCPayServer.Abstractions.Extensions;
using BTCPayServer.Abstractions.Models;
using BTCPayServer.Client;
using System.Threading.Tasks;



namespace BTCPayServer.Plugins.SimpleSwap.Controllers
{
    [Route("~/plugins/{storeId}/SimpleSwap")]
    [Authorize(AuthenticationSchemes = AuthenticationSchemes.Cookie, Policy = Policies.CanModifyStoreSettings)]
    [Authorize(AuthenticationSchemes = AuthenticationSchemes.Cookie, Policy = Policies.CanViewInvoices)]
    [Authorize(Policy = Policies.CanCreateNonApprovedPullPayments, AuthenticationSchemes = AuthenticationSchemes.Cookie)]
    [Authorize(Policy = Policies.CanManagePayouts, AuthenticationSchemes = AuthenticationSchemes.Cookie)]
    public class SimpleSwapPluginController : Controller
    {
        private readonly SimpleSwapPluginService _pluginService;
        private readonly SimpleSwapService _simpleSwapService;

        public SimpleSwapPluginController(SimpleSwapPluginService pluginService, SimpleSwapService simpleSwapService)
        {
            _pluginService = pluginService;
            _simpleSwapService = simpleSwapService;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromRoute] string storeId)
        {
            var model = new SimpleSwapModel
            {
                Settings = await _pluginService.GetStoreSettings(storeId),
                Transactions = await _pluginService.GetStoreTransactions(storeId)
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(SimpleSwapSettings settings, string command)
        {
            if (ModelState.IsValid && command == "save")
            {
                try
                {
                    settings.AcceptedCryptos ??= new List<string>();
                    await _pluginService.UpdateSettings(settings);
                    TempData[WellKnownTempData.SuccessMessage] = "Settings successfully saved";
                }
                catch (Exception ex)
                {
                    TempData[WellKnownTempData.ErrorMessage] = $"Error: {ex.Message}";
                    throw;
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("SwapMerchant")]
        public async Task<IActionResult> SwapMerchant([FromRoute] string storeId, [FromForm] SwapMerchantRequest req)
        {
            var response = new SwapCreationResponse();
            try
            {
                var settings = await _pluginService.GetStoreSettings(storeId);
                if (string.IsNullOrEmpty(settings.ApiKey) || string.IsNullOrEmpty(settings.UserId))
                {
                    TempData[WellKnownTempData.ErrorMessage] = "SimpleSwap API credentials not configured";
                    return RedirectToAction("Index", new { storeId });
                }

                string toCrypto, toNetwork;
                if (req.ToCrypto.Contains("-"))
                {
                    var split = req.ToCrypto.Split('-');
                    toCrypto = split[0];
                    toNetwork = split[1];
                }
                else
                {
                    toCrypto = req.ToCrypto;
                    toNetwork = req.ToCrypto;
                }

                var simpleSwapRequest = new SwapCreationRequest
                {
                    FromCrypto = "BTC",
                    FromNetwork = "BTC",
                    FromAmount = req.BtcAmount,
                    ToCrypto = toCrypto,
                    ToNetwork = toNetwork,
                    ToAmount = 0,
                    ToAddress = req.ToAddress,
                    UserId = settings.UserId,
                    ApiKey = settings.ApiKey,
                    FromRefundAddress = settings.DefaultRefundAddress
                };

                response = await _simpleSwapService.CreateSwapAsync(simpleSwapRequest);

                await _pluginService.CreateTransaction(storeId, response.SwapId, response.FromAddress, (decimal)req.BtcAmount, "BTC", toCrypto, req.ToAddress);

                TempData[WellKnownTempData.SuccessMessage] = "SimpleSwap successfully created: " + response.SwapId;
                TempData["SwapId"] = response.SwapId;
            }
            catch (Exception ex)
            {
                TempData[WellKnownTempData.ErrorMessage] = "Error during swap creation: " + ex.Message;
            }
            return RedirectToAction("Index", new { storeId });
        }
    }
} 
