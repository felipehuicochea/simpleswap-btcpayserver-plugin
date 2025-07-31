#!/bin/bash

# SimpleSwap API Test Script (Bash version)
# This script helps you test your SimpleSwap API credentials

echo "🚀 SimpleSwap API Test Script"
echo "=============================="

# Test basic API connectivity
echo "🔍 Testing SimpleSwap API connectivity..."
if curl -s -o /dev/null -w "%{http_code}" "https://api.simpleswap.io/api/v2/get_currencies" | grep -q "200"; then
    echo "✅ API is accessible"
else
    echo "❌ Cannot connect to SimpleSwap API"
    exit 1
fi

# Get credentials from user
echo ""
echo "📝 Enter your SimpleSwap credentials:"
read -p "API Key: " API_KEY
read -p "User ID: " USER_ID

if [ -z "$API_KEY" ] || [ -z "$USER_ID" ]; then
    echo "❌ API Key and User ID are required"
    exit 1
fi

echo ""
echo "🔑 Testing with API Key: ${API_KEY:0:8}..."
echo "👤 User ID: $USER_ID"

# Test exchange rate API
echo ""
echo "💰 Testing exchange rate API..."
RATE_RESPONSE=$(curl -s -X POST "https://api.simpleswap.io/api/v2/get_exchange_rate" \
  -H "Content-Type: application/json" \
  -d '{
    "currency_from": "ETH",
    "currency_to": "BTC",
    "amount": 0.1
  }')

if echo "$RATE_RESPONSE" | grep -q "rate"; then
    echo "✅ Exchange rate API working"
    RATE=$(echo "$RATE_RESPONSE" | grep -o '"rate":[^,]*' | cut -d':' -f2)
    echo "   Rate: $RATE"
else
    echo "❌ Exchange rate API failed"
    echo "   Response: $RATE_RESPONSE"
fi

# Test exchange creation API
echo ""
echo "🔄 Testing exchange creation API..."
EXCHANGE_RESPONSE=$(curl -s -X POST "https://api.simpleswap.io/api/v2/create_exchange" \
  -H "Content-Type: application/json" \
  -d "{
    \"currency_from\": \"ETH\",
    \"currency_to\": \"BTC\",
    \"amount\": 0.001,
    \"address_to\": \"bc1qxy2kgdygjrsqtzq2n0yrf2493p83kkfjhx0wlh\",
    \"user_id\": \"$USER_ID\",
    \"api_key\": \"$API_KEY\"
  }")

if echo "$EXCHANGE_RESPONSE" | grep -q "id"; then
    echo "✅ Exchange creation API working"
    EXCHANGE_ID=$(echo "$EXCHANGE_RESPONSE" | grep -o '"id":"[^"]*"' | cut -d'"' -f4)
    echo "   Exchange ID: $EXCHANGE_ID"
else
    echo "❌ Exchange creation failed"
    echo "   Response: $EXCHANGE_RESPONSE"
fi

echo ""
echo "=============================="
echo "📊 Test completed!"
echo ""
echo "💡 Next steps:"
echo "1. If tests passed, configure the BTCPayServer SimpleSwap plugin"
echo "2. Enter these credentials in your BTCPayServer store settings"
echo "3. Select accepted cryptocurrencies"
echo "4. Test with a small transaction" 