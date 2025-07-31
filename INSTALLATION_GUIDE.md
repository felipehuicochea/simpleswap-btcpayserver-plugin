# 🚀 SimpleSwap Plugin Installation Guide

## 📦 What's in the Zip File

The `SimpleSwap-Plugin.zip` contains:
- Complete BTCPayServer SimpleSwap plugin
- All source code and configuration files
- Test scripts for API validation
- Documentation and setup guides

## 📋 Installation Steps

### 1. Upload to Your Server

Upload the `SimpleSwap-Plugin.zip` file to your BTCPayServer server.

### 2. Extract the Plugin

```bash
# Navigate to your BTCPayServer plugins directory
cd /path/to/your/btcpayserver/plugins/

# Extract the plugin
unzip SimpleSwap-Plugin.zip

# Verify the extraction
ls -la BTCPayServer.Plugins.SimpleSwap/
```

### 3. Build the Plugin

```bash
# Navigate to the plugin directory
cd BTCPayServer.Plugins.SimpleSwap

# Build the plugin
dotnet build
```

### 4. Restart BTCPayServer

```bash
# If using Docker
docker-compose restart btcpayserver

# If using systemd
sudo systemctl restart btcpayserver

# If running directly, restart your BTCPayServer process
```

### 5. Configure the Plugin

1. **Login to BTCPayServer admin panel**
2. **Go to your store settings**
3. **Look for "SimpleSwap" in the left sidebar**
4. **Configure your settings:**
   - Enable the plugin
   - Enter your SimpleSwap API Key
   - Enter your SimpleSwap User ID
   - Select accepted cryptocurrencies
   - Set amount limits

## 🧪 Testing Your Installation

### Test 1: API Credentials
```bash
cd BTCPayServer.Plugins.SimpleSwap
python3 test-api.py
# OR
./test-api.sh
```

### Test 2: Plugin Functionality
1. Create a test invoice in BTCPayServer
2. During checkout, select "Pay with Altcoins"
3. Choose a cryptocurrency and test the swap creation

### Test 3: Merchant Swap
1. Go to SimpleSwap plugin settings
2. Use the "Merchant Swap" section
3. Try swapping a small amount (0.001 BTC to ETH)

## 🔍 Troubleshooting

### Plugin Not Showing?
- Check BTCPayServer logs: `docker logs btcpayserver | grep -i simpleswap`
- Ensure the plugin was built successfully
- Verify BTCPayServer was restarted

### Build Errors?
- Ensure .NET 8.0 is installed: `dotnet --version`
- Check file permissions: `chmod -R 755 BTCPayServer.Plugins.SimpleSwap`

### API Errors?
- Run the test scripts to verify credentials
- Check your SimpleSwap account balance
- Verify API key is active

## 📞 Support

If you encounter issues:
1. Check the full `README.md` in the plugin directory
2. Review BTCPayServer logs for error messages
3. Test your SimpleSwap credentials separately
4. Contact SimpleSwap support for API issues

---

**Ready to go!** Once configured, your customers will be able to pay with altcoins that automatically swap to Bitcoin. 🎉 
