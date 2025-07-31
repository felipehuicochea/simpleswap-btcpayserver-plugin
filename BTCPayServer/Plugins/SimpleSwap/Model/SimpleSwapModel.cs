using System.Collections.Generic;

namespace BTCPayServer.Plugins.SimpleSwap.Model
{
    public class SimpleSwapModel
    {
        public SimpleSwapSettings Settings { get; set; }
        public List<SimpleSwapTransaction> Transactions { get; set; } = new List<SimpleSwapTransaction>();
    }
} 