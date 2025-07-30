#!/usr/bin/env python3
"""
SimpleSwap API Test Script
This script helps you test your SimpleSwap API credentials and basic functionality.
"""

import requests
import json
import sys

# SimpleSwap API Configuration
BASE_URL = "https://api.simpleswap.io/api/v2/"

def test_api_connection():
    """Test basic API connectivity"""
    print("🔍 Testing SimpleSwap API connectivity...")
    try:
        response = requests.get(f"{BASE_URL}get_currencies", timeout=10)
        if response.status_code == 200:
            print("✅ API is accessible")
            return True
        else:
            print(f"❌ API returned status code: {response.status_code}")
            return False
    except Exception as e:
        print(f"❌ Failed to connect to API: {e}")
        return False

def test_exchange_rate(api_key, user_id):
    """Test getting exchange rate"""
    print("\n💰 Testing exchange rate API...")
    
    payload = {
        "currency_from": "ETH",
        "currency_to": "BTC",
        "amount": 0.1
    }
    
    try:
        response = requests.post(
            f"{BASE_URL}get_exchange_rate",
            json=payload,
            headers={"Content-Type": "application/json"},
            timeout=10
        )
        
        if response.status_code == 200:
            data = response.json()
            print("✅ Exchange rate API working")
            print(f"   Rate: {data.get('rate', 'N/A')}")
            print(f"   Estimated BTC: {data.get('estimated_btc', 'N/A')}")
            return True
        else:
            print(f"❌ Exchange rate API failed: {response.status_code}")
            print(f"   Response: {response.text}")
            return False
    except Exception as e:
        print(f"❌ Exchange rate test failed: {e}")
        return False

def test_create_exchange(api_key, user_id):
    """Test creating an exchange (dry run)"""
    print("\n🔄 Testing exchange creation API...")
    
    payload = {
        "currency_from": "ETH",
        "currency_to": "BTC",
        "amount": 0.001,  # Small test amount
        "address_to": "bc1qxy2kgdygjrsqtzq2n0yrf2493p83kkfjhx0wlh",  # Test address
        "user_id": user_id,
        "api_key": api_key
    }
    
    try:
        response = requests.post(
            f"{BASE_URL}create_exchange",
            json=payload,
            headers={"Content-Type": "application/json"},
            timeout=10
        )
        
        if response.status_code == 200:
            data = response.json()
            print("✅ Exchange creation API working")
            print(f"   Exchange ID: {data.get('id', 'N/A')}")
            print(f"   Status: {data.get('status', 'N/A')}")
            return True
        else:
            print(f"❌ Exchange creation failed: {response.status_code}")
            print(f"   Response: {response.text}")
            return False
    except Exception as e:
        print(f"❌ Exchange creation test failed: {e}")
        return False

def main():
    print("🚀 SimpleSwap API Test Script")
    print("=" * 40)
    
    # Test basic connectivity first
    if not test_api_connection():
        print("\n❌ Cannot connect to SimpleSwap API. Please check your internet connection.")
        sys.exit(1)
    
    # Get credentials from user
    print("\n📝 Enter your SimpleSwap credentials:")
    api_key = input("API Key: ").strip()
    user_id = input("User ID: ").strip()
    
    if not api_key or not user_id:
        print("❌ API Key and User ID are required")
        sys.exit(1)
    
    print(f"\n🔑 Testing with API Key: {api_key[:8]}...")
    print(f"👤 User ID: {user_id}")
    
    # Run tests
    tests_passed = 0
    total_tests = 2
    
    if test_exchange_rate(api_key, user_id):
        tests_passed += 1
    
    if test_create_exchange(api_key, user_id):
        tests_passed += 1
    
    # Summary
    print("\n" + "=" * 40)
    print(f"📊 Test Results: {tests_passed}/{total_tests} tests passed")
    
    if tests_passed == total_tests:
        print("🎉 All tests passed! Your SimpleSwap credentials are working correctly.")
        print("\n✅ You can now configure the BTCPayServer SimpleSwap plugin with these credentials.")
    else:
        print("⚠️  Some tests failed. Please check your credentials and try again.")
        print("\n💡 Common issues:")
        print("   - Verify your API key is correct and active")
        print("   - Ensure your User ID is correct")
        print("   - Check if your SimpleSwap account has sufficient balance")
        print("   - Contact SimpleSwap support if issues persist")

if __name__ == "__main__":
    main() 