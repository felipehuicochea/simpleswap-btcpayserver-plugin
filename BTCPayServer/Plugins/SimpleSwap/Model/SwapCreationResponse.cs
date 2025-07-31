namespace BTCPayServer.Plugins.SimpleSwap.Model
{
    public class SwapCreationResponse
    {
        public string StatusMessage { get; set; }
        public string SwapId { get; set; }
        public string FromAddress { get; set; }
        public float FromAmount { get; set; }
        public string ErrorMessage { get; set; }
    }
} 