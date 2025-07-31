#!/bin/bash

# SimpleSwap Plugin Deployment Script
# This script automates the deployment of BTCPayServer with SimpleSwap plugin

set -e

echo "🚀 Starting SimpleSwap Plugin Deployment..."

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Function to print colored output
print_status() {
    echo -e "${GREEN}[INFO]${NC} $1"
}

print_warning() {
    echo -e "${YELLOW}[WARNING]${NC} $1"
}

print_error() {
    echo -e "${RED}[ERROR]${NC} $1"
}

# Check if running as root
if [[ $EUID -eq 0 ]]; then
   print_warning "Running as root detected"
   print_status "For security reasons, it's recommended to run as a non-root user"
   print_status "However, we'll continue with root privileges..."
   print_status "Press Ctrl+C to cancel or any key to continue..."
   read -n 1 -s
fi

# Check system requirements
print_status "Checking system requirements..."

# Check OS
if [[ "$OSTYPE" == "linux-gnu"* ]]; then
    print_status "Linux detected"
elif [[ "$OSTYPE" == "darwin"* ]]; then
    print_error "macOS detected - This script is designed for Linux servers"
    exit 1
else
    print_error "Unsupported operating system"
    exit 1
fi

# Check .NET
if ! command -v dotnet &> /dev/null; then
    print_warning ".NET not found. Installing .NET 8.0..."
    
    # Install .NET 8.0
    wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
    sudo dpkg -i packages-microsoft-prod.deb
    sudo apt-get update
    sudo apt-get install -y apt-transport-https
    sudo apt-get install -y dotnet-sdk-8.0
    
    print_status ".NET 8.0 installed successfully"
else
    DOTNET_VERSION=$(dotnet --version)
    print_status ".NET found: $DOTNET_VERSION"
fi

# Check Git
if ! command -v git &> /dev/null; then
    print_warning "Git not found. Installing Git..."
    sudo apt-get update
    sudo apt-get install -y git
    print_status "Git installed successfully"
else
    print_status "Git found: $(git --version)"
fi

# Clone repository
print_status "Cloning SimpleSwap plugin repository..."
if [ -d "simpleswap-btcpayserver-plugin" ]; then
    print_warning "Repository already exists. Updating..."
    cd simpleswap-btcpayserver-plugin
    git pull origin master
else
    git clone https://github.com/felipehuicochea/simpleswap-btcpayserver-plugin.git
    cd simpleswap-btcpayserver-plugin
fi

# Build the project
print_status "Building BTCPayServer with SimpleSwap plugin..."
dotnet restore
dotnet build --configuration Release

# Check if build was successful
if [ $? -eq 0 ]; then
    print_status "Build completed successfully!"
else
    print_error "Build failed!"
    exit 1
fi

# Create run script
print_status "Creating run script..."
cat > run-btcpayserver.sh << 'EOF'
#!/bin/bash
cd "$(dirname "$0")"
cd BTCPayServer
dotnet run --configuration Release
EOF

chmod +x run-btcpayserver.sh

# Create systemd service (optional)
print_status "Creating systemd service..."
if [[ $EUID -eq 0 ]]; then
    # Running as root, use root user for service
    SERVICE_USER="root"
    SUDO_CMD=""
else
    # Running as regular user, use current user for service
    SERVICE_USER="$USER"
    SUDO_CMD="sudo"
fi

$SUDO_CMD tee /etc/systemd/system/btcpayserver.service > /dev/null << EOF
[Unit]
Description=BTCPayServer with SimpleSwap Plugin
After=network.target

[Service]
Type=simple
User=$SERVICE_USER
WorkingDirectory=$(pwd)/BTCPayServer
ExecStart=/usr/bin/dotnet run --configuration Release
Restart=always
RestartSec=10
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=BTCPAY_HOST=localhost

[Install]
WantedBy=multi-user.target
EOF

print_status "Deployment completed successfully!"
echo ""
echo "🎉 Next steps:"
echo "1. Start BTCPayServer: ./run-btcpayserver.sh"
if [[ $EUID -eq 0 ]]; then
    echo "2. Or enable systemd service: systemctl enable btcpayserver && systemctl start btcpayserver"
else
    echo "2. Or enable systemd service: sudo systemctl enable btcpayserver && sudo systemctl start btcpayserver"
fi
echo "3. Access BTCPayServer at: http://localhost"
echo "4. Configure SimpleSwap plugin in store settings"
echo ""
echo "📖 For detailed instructions, see: DEPLOYMENT_GUIDE.md" 