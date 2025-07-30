using System.ComponentModel.DataAnnotations;

namespace BTCPayServer.Plugins.SimpleSwap.Model
{
    public class SwapRequest
    {
        [Required]
        public string StoreId { get; set; }

        [Required]
        public string FromCrypto { get; set; }

        [Required]
        public string FromNetwork { get; set; }

        [Required]
        public float Amount { get; set; }

        [Required]
        public string ToAddress { get; set; }
    }
} 