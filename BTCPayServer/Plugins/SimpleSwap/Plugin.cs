using Microsoft.Extensions.DependencyInjection;
using BTCPayServer.Abstractions.Contracts;
using BTCPayServer.Abstractions.Models;
using BTCPayServer.Abstractions.Services;
using BTCPayServer.Plugins.SimpleSwap.Data;

namespace BTCPayServer.Plugins.SimpleSwap;

public class Plugin : BaseBTCPayServerPlugin
{
    public override IBTCPayServerPlugin.PluginDependency[] Dependencies { get; } =
    {
        new IBTCPayServerPlugin.PluginDependency { Identifier = nameof(BTCPayServer), Condition = ">=2.0.1" }
    };

    public override void Execute(IServiceCollection services)
    {
        services.AddUIExtension("header-nav", "SimpleSwapPluginHeaderNav");
        // -- Checkout v2 --
        // Tab (Payment Method)
        services.AddUIExtension("checkout-payment-method", "CheckoutV2/CheckoutPaymentMethodExtension");
        // Widget
        services.AddUIExtension("checkout-payment", "CheckoutV2/CheckoutPaymentExtension");

        services.AddHostedService<ApplicationPartsLogger>();
        services.AddHostedService<PluginMigrationRunner>();
        services.AddSingleton<SimpleSwapPluginService>();
        services.AddSingleton<SimpleSwapService>();
        services.AddSingleton<SimpleSwapPluginDbContextFactory>();
        services.AddDbContext<SimpleSwapPluginDbContext>((provider, o) =>
        {
            var factory = provider.GetRequiredService<SimpleSwapPluginDbContextFactory>();
            factory.ConfigureBuilder(o);
        });
    }
} 
