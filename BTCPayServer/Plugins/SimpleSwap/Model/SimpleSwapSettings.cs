using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BTCPayServer.Plugins.SimpleSwap.Model
{
    public class SimpleSwapSettings
    {
        public string StoreId { get; set; }
        
        [Required]
        public string ApiKey { get; set; }
        
        [Required]
        public string UserId { get; set; }
        
        public List<string> AcceptedCryptos { get; set; } = new List<string>();
        
        public bool Enabled { get; set; } = true;
        
        public string DefaultRefundAddress { get; set; }
        
        public float MinimumSwapAmount { get; set; } = 0.001f;
        
        public float MaximumSwapAmount { get; set; } = 10.0f;
    }
} 