namespace Emulator
{
    using QuickType;
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Emulator : DbContext
    {
        public Emulator()
            : base("name=Emulator")
        {
        }

        public virtual DbSet<ChartData> ChartDatas { get; set; }
        public virtual DbSet<TradeHistory> TradeHistories { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TradeHistory>()
                .Property(e => e.rate)
                .IsUnicode(false);

            modelBuilder.Entity<TradeHistory>()
                .Property(e => e.amount)
                .IsUnicode(false);

            modelBuilder.Entity<TradeHistory>()
                .Property(e => e.total)
                .IsUnicode(false);
        }
    }
}
