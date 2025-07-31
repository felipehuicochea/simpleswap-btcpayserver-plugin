using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using BTCPayServer.Plugins.SimpleSwap.Model;

using Newtonsoft.Json.Linq;
using System.Linq;

namespace BTCPayServer.Plugins.SimpleSwap.Services
{
    public class SimpleSwapService
    {
        private readonly string BaseUrl = "https://api.simpleswap.io/api/v2/";
        private readonly HttpClient _httpClient;
        private readonly ILogger<SimpleSwapService> _logger;

        public SimpleSwapService(ILogger<SimpleSwapService> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(BaseUrl);
        }

        public async Task<SwapCreationResponse> CreateSwapAsync(SwapCreationRequest req)
        {
            string responseContent = "";
            try
            {
                var createSwapRequest = new Dictionary<string, object>
                {
                    ["currency_from"] = req.FromCrypto,
                    ["currency_to"] = req.ToCrypto,
                    ["amount"] = req.FromAmount,
                    ["address_to"] = req.ToAddress,
                    ["user_id"] = req.UserId,
                    ["api_key"] = req.ApiKey
                };

                if (!string.IsNullOrEmpty(req.FromRefundAddress))
                    createSwapRequest["address_from"] = req.FromRefundAddress;

                var createJson = JsonConvert.SerializeObject(createSwapRequest);
                _logger.LogInformation($"SimpleSwap API Request: {createJson}");

                var webRequest = new HttpRequestMessage(HttpMethod.Post, "create_exchange")
                {
                    Content = new StringContent(createJson, Encoding.UTF8, "application/json"),
                };

                using (var response = await _httpClient.SendAsync(webRequest))
                {
                    responseContent = await response.Content.ReadAsStringAsync();
                    response.EnsureSuccessStatusCode();
                }

                dynamic jsonResponse = JsonConvert.DeserializeObject<dynamic>(responseContent);
                
                var swap = new SwapCreationResponse()
                {
                    StatusMessage = jsonResponse.status?.ToString() ?? "created",
                    SwapId = jsonResponse.id?.ToString(),
                    FromAddress = jsonResponse.address_from?.ToString(),
                    FromAmount = Convert.ToSingle(jsonResponse.amount ?? req.FromAmount),
                };

                _logger.LogInformation($"SimpleSwap Swap Created: {swap.SwapId} {req.FromCrypto} -> {req.ToCrypto}");
                return swap;
            }
            catch (Exception ex)
            {
                _logger.LogError($"SimpleSwapPlugin.CreateSwap(): {ex.Message} - {responseContent} - {req.FromCrypto} {req.ToCrypto}");
                if (string.IsNullOrEmpty(responseContent))
                {
                    throw;
                }
                else
                {
                    try
                    {
                        dynamic jsonResponse = JsonConvert.DeserializeObject<dynamic>(responseContent);
                        string errorMsg = jsonResponse.message?.ToString() ?? jsonResponse.error?.ToString() ?? "Unknown error";
                        throw new Exception(errorMsg);
                    }
                    catch
                    {
                        throw new Exception($"API Error: {responseContent}");
                    }
                }
            }
        }

        public async Task<Dictionary<string, object>> GetExchangeRateAsync(string fromCrypto, string toCrypto, float amount)
        {
            try
            {
                var request = new Dictionary<string, object>
                {
                    ["currency_from"] = fromCrypto,
                    ["currency_to"] = toCrypto,
                    ["amount"] = amount
                };

                var json = JsonConvert.SerializeObject(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("get_exchange_rate", content);
                var responseContent = await response.Content.ReadAsStringAsync();
                response.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<Dictionary<string, object>>(responseContent);
            }
            catch (Exception ex)
            {
                _logger.LogError($"SimpleSwapPlugin.GetExchangeRate(): {ex.Message}");
                throw;
            }
        }

        public async Task<Dictionary<string, object>> GetExchangeStatusAsync(string exchangeId, string apiKey)
        {
            try
            {
                var request = new Dictionary<string, object>
                {
                    ["id"] = exchangeId,
                    ["api_key"] = apiKey
                };

                var json = JsonConvert.SerializeObject(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("get_exchange", content);
                var responseContent = await response.Content.ReadAsStringAsync();
                response.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<Dictionary<string, object>>(responseContent);
            }
            catch (Exception ex)
            {
                _logger.LogError($"SimpleSwapPlugin.GetExchangeStatus(): {ex.Message}");
                throw;
            }
        }
    }
} 
