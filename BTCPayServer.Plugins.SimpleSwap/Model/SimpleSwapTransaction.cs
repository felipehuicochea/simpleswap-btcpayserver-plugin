using System;

namespace BTCPayServer.Plugins.SimpleSwap.Model
{
    public class SimpleSwapTransaction
    {
        public string Id { get; set; }
        public string SwapId { get; set; }
        public string FromCrypto { get; set; }
        public string ToCrypto { get; set; }
        public float FromAmount { get; set; }
        public float ToAmount { get; set; }
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string StoreId { get; set; }
    }
} 