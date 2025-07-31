# SimpleSwap Plugin Configuration Guide

## Getting Your SimpleSwap API Credentials

1. **Sign up/Login to SimpleSwap**: Go to [simpleswap.io](https://simpleswap.io) and create an account or login
2. **Access API Settings**: Navigate to your account settings or API section
3. **Generate API Key**: Create a new API key for BTCPayServer integration
4. **Get User ID**: Note your User ID (usually found in account settings)

## Configuration Steps

### 1. Plugin Installation
First, ensure the plugin is properly installed in your BTCPayServer:

```bash
# Build the plugin
cd BTCPayServer.Plugins.SimpleSwap
dotnet build

# Copy to BTCPayServer plugins directory (adjust path as needed)
cp -r BTCPayServer.Plugins.SimpleSwap /path/to/btcpayserver/plugins/
```

### 2. Configure in BTCPayServer

1. **Access Store Settings**: Go to your BTCPayServer admin panel
2. **Navigate to Store**: Select the store where you want to enable SimpleSwap
3. **Find SimpleSwap Tab**: Look for "SimpleSwap" in the left sidebar navigation
4. **Enter Credentials**:
   - **API Key**: Your SimpleSwap API key
   - **User ID**: Your SimpleSwap User ID
   - **Enable Plugin**: Check the "Enable SimpleSwap for this store" checkbox

### 3. Configure Accepted Cryptocurrencies

Select which cryptocurrencies you want to accept from the dropdown list. Popular options include:
- ETH (Ethereum)
- USDT (Tether)
- USDC (USD Coin)
- LTC (Litecoin)
- BCH (Bitcoin Cash)
- XRP (Ripple)

### 4. Set Amount Limits

Configure minimum and maximum swap amounts:
- **Minimum**: 0.001 (recommended)
- **Maximum**: 10.0 (adjust based on your needs)

### 5. Optional: Set Refund Address

Enter a Bitcoin address where refunds should be sent if transactions fail.

## Testing Your Configuration

### Test 1: API Connection Test
Create a simple test to verify your API credentials work:

```bash
# Test API connection (replace with your actual credentials)
curl -X POST https://api.simpleswap.io/api/v2/get_exchange_rate \
  -H "Content-Type: application/json" \
  -d '{
    "currency_from": "ETH",
    "currency_to": "BTC",
    "amount": 0.1
  }'
```

### Test 2: Plugin Functionality
1. **Create a Test Invoice**: Create a small test invoice in BTCPayServer
2. **Select SimpleSwap**: During checkout, choose "Pay with Altcoins"
3. **Test Swap Creation**: Try creating a small swap (e.g., 0.001 ETH to BTC)
4. **Verify Transaction**: Check that the transaction appears in your SimpleSwap dashboard

### Test 3: Merchant Swap
1. **Go to Plugin Settings**: Navigate to the SimpleSwap plugin settings
2. **Use Merchant Swap**: Try swapping a small amount of BTC to another cryptocurrency
3. **Verify Results**: Check that the swap is created successfully

## Troubleshooting

### Common Issues:

1. **"API Key Invalid"**
   - Verify your API key is correct
   - Ensure the API key is active in your SimpleSwap account
   - Check for any extra spaces or characters

2. **"User ID Not Found"**
   - Verify your User ID is correct
   - User ID is usually a numeric value

3. **"Plugin Not Showing"**
   - Ensure the plugin is enabled
   - Check that at least one cryptocurrency is selected
   - Verify the plugin is properly installed

4. **"Transaction Failures"**
   - Check that the amount sent matches exactly
   - Verify the destination address is correct
   - Ensure sufficient balance in your SimpleSwap account

### Debug Information:

Check BTCPayServer logs for detailed error messages:
```bash
# View BTCPayServer logs
docker logs btcpayserver 2>&1 | grep -i simpleswap
```

## Security Notes

- Keep your API keys secure and never share them
- Use different API keys for different environments (test/production)
- Regularly rotate your API keys
- Monitor your SimpleSwap account for unusual activity

## Support

If you encounter issues:
1. Check the troubleshooting section above
2. Review BTCPayServer logs for error messages
3. Verify your SimpleSwap account settings
4. Contact SimpleSwap support if API issues persist 