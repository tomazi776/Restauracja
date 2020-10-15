namespace Restauracja.Migrations
{
    using Restauracja.Model;
    using Restauracja.Model.Entities;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<Restauracja.RestaurantDataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }
    }
}
