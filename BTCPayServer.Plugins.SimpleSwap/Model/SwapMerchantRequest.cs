using System.ComponentModel.DataAnnotations;

namespace BTCPayServer.Plugins.SimpleSwap.Model
{
    public class SwapMerchantRequest
    {
        [Required]
        public float BtcAmount { get; set; }

        [Required]
        public string ToCrypto { get; set; }

        [Required]
        public string ToAddress { get; set; }
    }
} 