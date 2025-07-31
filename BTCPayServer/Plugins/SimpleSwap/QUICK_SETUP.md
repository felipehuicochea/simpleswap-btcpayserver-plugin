# 🚀 Quick Setup Guide - SimpleSwap Plugin

## Step 1: Choose Your Setup Option

### Option A: Quick Start (Recommended)
The plugin comes pre-configured with a default affiliate API key. You can start using it immediately without any setup!

**Benefits:**
- ✅ No account creation required
- ✅ Works out of the box
- ✅ No additional fees
- ✅ Quick setup

### Option B: Use Your Own SimpleSwap Account
If you prefer to use your own SimpleSwap account:

1. Go to [SimpleSwap](https://simpleswap.io/?ref=cf9858404d01) and sign up/login
2. Navigate to your account settings → API section
3. Generate a new API key
4. Note your User ID (usually a number) 


## Step 2: Test Your API Credentials

### Option A: Using the Python script (recommended)
```bash
cd BTCPayServer.Plugins.SimpleSwap
python3 test-api.py
```

### Option B: Using the Bash script
```bash
cd BTCPayServer.Plugins.SimpleSwap
./test-api.sh
```

### Option C: Manual curl test
```bash
# Test exchange rate
curl -X POST https://api.simpleswap.io/api/v2/get_exchange_rate \
  -H "Content-Type: application/json" \
  -d '{
    "currency_from": "ETH",
    "currency_to": "BTC",
    "amount": 0.1
  }'
```

## Step 3: Build and Install the Plugin

```bash
# Build the plugin
cd BTCPayServer.Plugins.SimpleSwap
dotnet build

# Copy to BTCPayServer (adjust path as needed)
cp -r BTCPayServer.Plugins.SimpleSwap /path/to/your/btcpayserver/plugins/
```

## Step 4: Configure in BTCPayServer

1. **Restart BTCPayServer** to load the plugin
2. **Go to your store settings**
3. **Look for "SimpleSwap" in the left sidebar**
4. **Configure the plugin:**
   - ✅ Enable SimpleSwap for this store
   - 🔑 Enter your API Key
   - 👤 Enter your User ID
   - 💰 Select accepted cryptocurrencies (ETH, USDT, USDC, etc.)
   - 📊 Set amount limits (Min: 0.001, Max: 10.0)
   - 🔄 Optional: Set refund address

## Step 5: Test the Integration

### Test 1: Merchant Swap
1. Go to SimpleSwap plugin settings
2. Use "Merchant Swap" section
3. Try swapping 0.001 BTC to ETH
4. Verify the swap is created

### Test 2: Customer Checkout
1. Create a test invoice in BTCPayServer
2. During checkout, select "Pay with Altcoins"
3. Choose ETH from the dropdown
4. Verify the swap creation process

## 🔧 Troubleshooting

### Plugin not showing?
- Ensure BTCPayServer was restarted after plugin installation
- Check that the plugin files are in the correct directory
- Verify .NET 8.0 is installed

### API errors?
- Run the test scripts above to verify credentials
- Check that your SimpleSwap account has sufficient balance
- Ensure API key is active and not expired

### Transaction failures?
- Verify exact amounts are sent
- Check destination addresses are correct
- Monitor SimpleSwap dashboard for transaction status

## 📞 Support

If you encounter issues:
1. Check BTCPayServer logs: `docker logs btcpayserver | grep -i simpleswap`
2. Run the test scripts to verify API connectivity
3. Contact SimpleSwap support for API issues
4. Check the full README.md for detailed documentation

## 🎯 Next Steps

Once configured:
- Test with small amounts first
- Monitor transaction history
- Configure additional cryptocurrencies as needed
- Set up proper refund addresses
- Consider production deployment

---

**Need help?** Check the full `README.md` for comprehensive documentation and troubleshooting guides. 
