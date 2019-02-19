namespace Emulator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrateDB : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.TradeHistories", newName: "BTCTradeHistories");
            CreateTable(
                "dbo.ETHTradeHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GlobalTradeId = c.Long(nullable: false),
                        TradeId = c.Long(nullable: false),
                        Date = c.DateTimeOffset(nullable: false, precision: 7),
                        Type = c.Int(nullable: false),
                        Rate = c.String(),
                        Amount = c.String(),
                        Total = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.XRPTradeHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GlobalTradeId = c.Long(nullable: false),
                        TradeId = c.Long(nullable: false),
                        Date = c.DateTimeOffset(nullable: false, precision: 7),
                        Type = c.Int(nullable: false),
                        Rate = c.String(),
                        Amount = c.String(),
                        Total = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.XRPTradeHistories");
            DropTable("dbo.ETHTradeHistories");
            RenameTable(name: "dbo.BTCTradeHistories", newName: "TradeHistories");
        }
    }
}
