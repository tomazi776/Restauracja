namespace Restauracja.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSent : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cust_Order", "Sent", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Cust_Order", "Sent");
        }
    }
}
