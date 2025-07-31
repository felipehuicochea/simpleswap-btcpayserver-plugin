#!/bin/bash

# Quick Deploy Script for SimpleSwap Plugin
# This script can be run directly from the repository

echo "🚀 Quick Deploy - SimpleSwap Plugin"

# Check if we're in the right directory
if [ ! -f "deploy.sh" ]; then
    echo "❌ Error: deploy.sh not found in current directory"
    echo "📁 Current directory: $(pwd)"
    echo "📋 Files in current directory:"
    ls -la
    echo ""
    echo "💡 Make sure you're in the simpleswap-btcpayserver-plugin directory"
    echo "   Run: cd simpleswap-btcpayserver-plugin"
    exit 1
fi

# Make deploy.sh executable and run it
chmod +x deploy.sh
./deploy.sh
