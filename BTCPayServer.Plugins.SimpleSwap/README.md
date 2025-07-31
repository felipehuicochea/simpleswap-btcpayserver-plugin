# BTCPayServer SimpleSwap Plugin

A BTCPayServer plugin that allows customers to pay with altcoins using the SimpleSwap service. This plugin integrates seamlessly with BTCPayServer's checkout process, enabling merchants to accept payments in various cryptocurrencies that are automatically swapped to Bitcoin.

## Features

- **Multi-Cryptocurrency Support**: Accept payments in popular altcoins like Ethereum, USDT, USDC, Litecoin, and many more
- **Seamless Integration**: Works with BTCPayServer's existing checkout flow
- **Merchant Swaps**: Merchants can also swap their Bitcoin balance to other cryptocurrencies
- **Transaction Tracking**: Complete transaction history and status monitoring
- **Configurable Limits**: Set minimum and maximum swap amounts
- **Refund Support**: Automatic refund address configuration

## Installation

1. Clone this repository into your BTCPayServer plugins directory
2. Build the plugin using .NET 8.0
3. Restart your BTCPayServer instance
4. Configure the plugin in your store settings

## Configuration
### Option 1: Use Default Affiliate API Key (Recommended for Quick Start)

The plugin comes pre-configured with a default affiliate API key. This allows you to start using SimpleSwap immediately without creating your own account. A small commission will be credited to the plugin developers at no additional cost to you.

**Benefits:**
- No account creation required
- Works out of the box
- No additional fees for you
- Quick setup

### Option 2: Use Your Own SimpleSwap Account

If you prefer to use your own SimpleSwap account:

1. Go to [SimpleSwap](https://simpleswap.io/?ref=cf9858404d01) and sign up/login
2. Navigate to your account settings → API section
3. Generate a new API key
4. Note your User ID (usually a number)
5. Replace the default API key and User ID in the plugin settings 

### Setup Steps

1. Go to your BTCPayServer store settings
2. Navigate to the "SimpleSwap" tab in the left sidebar
3. Configure the following settings:
   - **Enable SimpleSwap**: Toggle to enable the plugin
   - **API Key**: Your SimpleSwap API key
   - **User ID**: Your SimpleSwap User ID
   - **Default Refund Address**: Optional BTC address for refunds
   - **Accepted Cryptocurrencies**: Select which cryptocurrencies to accept
   - **Amount Limits**: Set minimum and maximum swap amounts

## Usage

### For Customers

1. During checkout, customers will see a "Pay with Altcoins" option
2. They can select their preferred cryptocurrency from the dropdown
3. The plugin creates a SimpleSwap transaction
4. Customers receive a deposit address and amount to send
5. Once the payment is confirmed, the transaction is marked as complete

### For Merchants

1. **Merchant Swaps**: Use the "Merchant Swap" section to convert your Bitcoin balance to other cryptocurrencies
2. **Transaction History**: View all SimpleSwap transactions in the plugin dashboard
3. **Settings Management**: Configure accepted cryptocurrencies and limits

## Supported Cryptocurrencies

The plugin supports a wide range of cryptocurrencies including:

- Ethereum (ETH)
- Tether (USDT)
- USD Coin (USDC)
- Litecoin (LTC)
- Bitcoin Cash (BCH)
- Ripple (XRP)
- Cardano (ADA)
- Polkadot (DOT)
- Chainlink (LINK)
- Uniswap (UNI)
- Polygon (MATIC)
- Avalanche (AVAX)
- Solana (SOL)
- Cosmos (ATOM)
- Fantom (FTM)
- NEAR Protocol (NEAR)
- Algorand (ALGO)
- VeChain (VET)
- TRON (TRX)
- EOS (EOS)
- Stellar (XLM)
- Monero (XMR)
- Zcash (ZEC)
- Dash (DASH)

## API Integration

The plugin integrates with SimpleSwap's API v2 for:
- Creating exchange transactions
- Getting exchange rates
- Checking transaction status

## Security Considerations

- API keys are stored securely in the database
- All API communications use HTTPS
- Transaction validation ensures proper amounts and addresses
- Refund addresses can be configured for failed transactions

## Troubleshooting

### Common Issues

1. **API Key Invalid**: Ensure your SimpleSwap API key is correct and active
2. **User ID Not Found**: Verify your SimpleSwap User ID is correct
3. **Transaction Failures**: Check that the amount sent matches exactly what was requested
4. **Plugin Not Showing**: Ensure the plugin is enabled and cryptocurrencies are selected

### Logs

Check BTCPayServer logs for detailed error messages related to SimpleSwap operations.

## Development

### Building the Plugin

```bash
cd BTCPayServer.Plugins.SimpleSwap
dotnet build
```

### Testing

The plugin includes comprehensive error handling and logging. Test thoroughly with small amounts before using in production.

## License

This plugin is licensed under the MIT License. See the LICENSE file for details.

## Support

For issues and questions:
1. Check the troubleshooting section above
2. Review BTCPayServer logs
3. Open an issue in this repository

## Contributing

Contributions are welcome! Please feel free to submit pull requests or open issues for bugs and feature requests.

## Disclaimer

This plugin integrates with third-party services. Users should review SimpleSwap's terms of service and understand the risks associated with cryptocurrency exchanges. 
