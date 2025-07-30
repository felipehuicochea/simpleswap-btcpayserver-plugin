using BTCPayServer.Plugins.SimpleSwap.Model;
using Microsoft.EntityFrameworkCore;

namespace BTCPayServer.Plugins.SimpleSwap.Data
{
    public class SimpleSwapPluginDbContext : DbContext
    {
        private readonly bool _designTime;

        public SimpleSwapPluginDbContext(DbContextOptions<SimpleSwapPluginDbContext> options, bool designTime = false)
            : base(options)
        {
            _designTime = designTime;
        }

        public DbSet<SimpleSwapSettings> SimpleSwapSettings { get; set; }
        public DbSet<SimpleSwapTransaction> SimpleSwapTransactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("BTCPayServer.Plugins.SimpleSwap");
            
            // Configure SimpleSwapSettings
            modelBuilder.Entity<SimpleSwapSettings>(entity =>
            {
                entity.HasKey(e => e.StoreId);
                entity.Property(e => e.AcceptedCryptos).HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());
            });
            
            // Configure SimpleSwapTransaction
            modelBuilder.Entity<SimpleSwapTransaction>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
        }
    }
} 