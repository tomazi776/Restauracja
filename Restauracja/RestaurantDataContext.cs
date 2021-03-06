namespace Restauracja
{
    using Restauracja.Migrations;
    using Restauracja.Model.Entities;
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class RestaurantDataContext : DbContext
    {
        // Your context has been configured to use a 'RestaurantDataContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'Restauracja.RestaurantDataContext' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'RestaurantDataContext' 
        // connection string in the application configuration file.
        public RestaurantDataContext()
            : base("name=RestaurantDataContext")
        {
            //Database.SetInitializer<RestaurantDataContext>(new DropCreateDatabaseAlways<RestaurantDataContext>());
            Database.SetInitializer(new RestaurantInitializer());
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<RestaurantDataContext, Configuration>());
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Customer> Customers { get; set; }
    }
}