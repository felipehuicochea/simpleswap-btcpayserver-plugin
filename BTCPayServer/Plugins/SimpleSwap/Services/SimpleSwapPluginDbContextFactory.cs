using BTCPayServer.Plugins.SimpleSwap.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

using System.IO;

namespace BTCPayServer.Plugins.SimpleSwap.Data
{
    public class SimpleSwapPluginDbContextFactory : IDesignTimeDbContextFactory<SimpleSwapPluginDbContext>
    {
        public SimpleSwapPluginDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<SimpleSwapPluginDbContext>();
            var connectionString = "Data Source=" + Path.Combine(Directory.GetCurrentDirectory(), "simpleswap.db");
            builder.UseSqlite(connectionString);
            return new SimpleSwapPluginDbContext(builder.Options);
        }
    }
}
