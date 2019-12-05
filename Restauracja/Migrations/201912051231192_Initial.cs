﻿namespace Restauracja.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderItem",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Price = c.Int(nullable: false),
                        Description = c.String(),
                        Quantity = c.Int(nullable: false),
                        Remarks = c.String(),
                        OrderId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cust_Order", t => t.OrderId, cascadeDelete: true)
                .Index(t => t.OrderId);
            
            CreateTable(
                "dbo.Cust_Order",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FinalCost = c.Int(nullable: false),
                        Description = c.String(),
                        Customer_Customer_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customer", t => t.Customer_Customer_Id)
                .Index(t => t.Customer_Customer_Id);
            
            CreateTable(
                "dbo.Customer",
                c => new
                    {
                        Customer_Id = c.Int(nullable: false, identity: true),
                        CustomerEmail = c.String(),
                    })
                .PrimaryKey(t => t.Customer_Id);
            
            CreateTable(
                "dbo.Product",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Price = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Remarks = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderItem", "OrderId", "dbo.Cust_Order");
            DropForeignKey("dbo.Cust_Order", "Customer_Customer_Id", "dbo.Customer");
            DropIndex("dbo.Cust_Order", new[] { "Customer_Customer_Id" });
            DropIndex("dbo.OrderItem", new[] { "OrderId" });
            DropTable("dbo.Product");
            DropTable("dbo.Customer");
            DropTable("dbo.Cust_Order");
            DropTable("dbo.OrderItem");
        }
    }
}
