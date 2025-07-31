# 🚀 SimpleSwap Plugin Deployment Guide

## 📋 Prerequisites

### Server Requirements
- **Operating System**: Ubuntu 20.04+ / Debian 11+ / CentOS 8+
- **.NET 8.0 SDK**: Required for building the plugin
- **Docker** (optional): For containerized deployment
- **Git**: For cloning the repository
- **Minimum RAM**: 4GB (8GB recommended)
- **Storage**: 20GB+ available space

### Software Installation

#### 1. Install .NET 8.0 SDK
```bash
# Ubuntu/Debian
wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
sudo apt-get update
sudo apt-get install -y apt-transport-https
sudo apt-get install -y dotnet-sdk-8.0

# Verify installation
dotnet --version
```

#### 2. Install Git (if not already installed)
```bash
sudo apt-get update
sudo apt-get install -y git
```

## 🔧 Deployment Steps

### Option 1: Direct Deployment (Recommended for Testing)

#### 1. Clone the Repository
```bash
git clone https://github.com/felipehuicochea/simpleswap-btcpayserver-plugin.git
cd simpleswap-btcpayserver-plugin
```

#### 2. Build the Project
```bash
# Restore dependencies
dotnet restore

# Build the solution
dotnet build --configuration Release

# Verify the build
ls -la BTCPayServer/bin/Release/net8.0/
```

#### 3. Run BTCPayServer with the Plugin
```bash
# Navigate to BTCPayServer directory
cd BTCPayServer

# Run the application
dotnet run --configuration Release
```

### Option 2: Docker Deployment

#### 1. Create Dockerfile
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["BTCPayServer/BTCPayServer.csproj", "BTCPayServer/"]
COPY ["BTCPayServer.Abstractions/BTCPayServer.Abstractions.csproj", "BTCPayServer.Abstractions/"]
COPY ["BTCPayServer.Common/BTCPayServer.Common.csproj", "BTCPayServer.Common/"]
COPY ["BTCPayServer.Data/BTCPayServer.Data.csproj", "BTCPayServer.Data/"]
COPY ["BTCPayServer.Client/BTCPayServer.Client.csproj", "BTCPayServer.Client/"]
COPY ["BTCPayServer.Rating/BTCPayServer.Rating.csproj", "BTCPayServer.Rating/"]
COPY ["BTCPayServer.PluginPacker/BTCPayServer.PluginPacker.csproj", "BTCPayServer.PluginPacker/"]
COPY ["BTCPayServer/Plugins/SimpleSwap/BTCPayServer.Plugins.SimpleSwap.csproj", "BTCPayServer/Plugins/SimpleSwap/"]
RUN dotnet restore "BTCPayServer/BTCPayServer.csproj"
COPY . .
WORKDIR "/src/BTCPayServer"
RUN dotnet build "BTCPayServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BTCPayServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BTCPayServer.dll"]
```

#### 2. Build and Run Docker Container
```bash
# Build the Docker image
docker build -t btcpayserver-simpleswap .

# Run the container
docker run -d \
  --name btcpayserver \
  -p 80:80 \
  -p 443:443 \
  -v btcpayserver_data:/app/data \
  btcpayserver-simpleswap
```

## 🧪 Testing the Plugin

### 1. Initial Setup
1. **Access BTCPayServer**: Navigate to `http://your-server-ip` or `https://your-domain`
2. **Create Admin Account**: Follow the initial setup wizard
3. **Create a Store**: Set up your first store for testing

### 2. Configure SimpleSwap Plugin
1. **Navigate to Store Settings**: Go to your store → Settings
2. **Find SimpleSwap**: Look for "SimpleSwap" in the left sidebar
3. **Verify Default Settings**:
   - ✅ API Key should be pre-filled: `9bad34cc-9f86-4441-87b6-dd1cf60384ae`
   - ✅ User ID should be: `affiliate`
   - ✅ Plugin should be enabled by default
4. **Select Cryptocurrencies**: Choose which altcoins to accept (ETH, USDT, USDC, etc.)
5. **Save Settings**: Click "Save Settings"

### 3. Test Merchant Swap
1. **Go to SimpleSwap Plugin**: Navigate to the SimpleSwap tab
2. **Use Merchant Swap Section**: 
   - Enter BTC amount: `0.001`
   - Select destination crypto: `ETH`
   - Enter destination address (your ETH wallet)
3. **Create Swap**: Click "Create Swap"
4. **Verify**: Check that swap ID is generated and transaction appears in history

### 4. Test Customer Checkout
1. **Create Test Invoice**: Create a new invoice in BTCPayServer
2. **During Checkout**: Look for "Pay with Altcoins" option
3. **Select Cryptocurrency**: Choose ETH or another supported crypto
4. **Complete Payment**: Follow the swap process
5. **Verify**: Check transaction status in SimpleSwap plugin

## 🔍 Troubleshooting

### Common Issues

#### 1. Build Errors
```bash
# Check .NET version
dotnet --version

# Clean and rebuild
dotnet clean
dotnet restore
dotnet build --configuration Release
```

#### 2. Plugin Not Showing
- ✅ Verify plugin is in `BTCPayServer/Plugins/SimpleSwap/`
- ✅ Check solution file includes the plugin
- ✅ Restart BTCPayServer after plugin installation
- ✅ Check logs: `docker logs btcpayserver | grep -i simpleswap`

#### 3. API Connection Issues
```bash
# Test SimpleSwap API connectivity
cd BTCPayServer/Plugins/SimpleSwap
python3 test-api.py
```

#### 4. Database Migration Issues
- The plugin includes automatic database migrations
- Check logs for migration errors
- Ensure database has write permissions

### Logs and Debugging
```bash
# View BTCPayServer logs
docker logs btcpayserver

# View specific plugin logs
docker logs btcpayserver | grep -i simpleswap

# Check application logs
tail -f /app/data/logs/btcpayserver.log
```

## 📊 Monitoring

### Key Metrics to Monitor
1. **Swap Success Rate**: Percentage of successful swaps
2. **API Response Times**: SimpleSwap API performance
3. **Error Rates**: Failed transactions and API errors
4. **User Adoption**: Number of customers using altcoin payments

### Health Checks
```bash
# Check plugin status
curl -s http://localhost/plugins/simpleswap/health

# Check API connectivity
curl -s http://localhost/plugins/simpleswap/api/test
```

## 🔒 Security Considerations

### Production Deployment
1. **Use HTTPS**: Always use SSL/TLS in production
2. **Firewall Configuration**: Restrict access to necessary ports only
3. **Database Security**: Use strong passwords and encrypted connections
4. **API Key Management**: Consider using your own SimpleSwap API key for production
5. **Regular Updates**: Keep BTCPayServer and the plugin updated

### Environment Variables
```bash
# Set production environment
export ASPNETCORE_ENVIRONMENT=Production
export BTCPAY_HOST=your-domain.com
export BTCPAY_SSHKEYFILE=/path/to/ssh/key
```

## 📞 Support

### Getting Help
1. **Check Logs**: Review application and plugin logs
2. **Test API**: Use the included test scripts
3. **Documentation**: Review README.md and QUICK_SETUP.md
4. **GitHub Issues**: Report bugs at the repository

### Useful Commands
```bash
# Quick health check
curl -s http://localhost/health

# Plugin status
curl -s http://localhost/plugins/simpleswap/status

# API test
python3 BTCPayServer/Plugins/SimpleSwap/test-api.py
```

---

**Ready for Testing!** 🚀

The SimpleSwap plugin is now deployed and ready for testing. Start with small amounts and gradually increase as you verify everything works correctly. 