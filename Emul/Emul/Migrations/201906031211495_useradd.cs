namespace Emul.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class useradd : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Examinations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmulationNumber = c.Int(nullable: false),
                        StartDate = c.DateTimeOffset(nullable: false, precision: 7),
                        EndDate = c.DateTimeOffset(nullable: false, precision: 7),
                        Diff = c.Double(nullable: false),
                        CheckDiff = c.Double(nullable: false),
                        CheckTime = c.Double(nullable: false),
                        BuyTime = c.Double(nullable: false),
                        HoldTime = c.Double(nullable: false),
                        Balance = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.THs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmulationNumber = c.Int(nullable: false),
                        Date = c.DateTimeOffset(nullable: false, precision: 7),
                        Type = c.String(),
                        Rate = c.Double(nullable: false),
                        Amount = c.Double(nullable: false),
                        Total = c.Double(nullable: false),
                        Fee = c.Double(nullable: false),
                        Balance = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Coin_TH",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CurrencyName = c.String(),
                        GlobalTradeId = c.Long(nullable: false),
                        TradeId = c.Long(nullable: false),
                        Date = c.DateTimeOffset(nullable: false, precision: 7),
                        Type = c.String(),
                        Rate = c.Double(nullable: false),
                        Amount = c.Double(nullable: false),
                        Total = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Users");
            DropTable("dbo.Coin_TH");
            DropTable("dbo.THs");
            DropTable("dbo.Examinations");
        }
    }
}
