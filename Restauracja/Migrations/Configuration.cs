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

        protected override void Seed(Restauracja.RestaurantDataContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.

            using (var dbContext = new RestaurantDataContext())
            {
                List<ProductPOCO> newPOCOProducts = new List<ProductPOCO>()
                {
                    new Pizza("Margheritta", 20m,"Sos, ser mozarella, oregano"),
                    new Pizza("Vegetariana", 22m,"Bezmięsna"),
                    new Pizza("Tosca", 25m,"Z oliwą z oliwek"),
                    new Pizza("Venecia", 22m, "Mięsna"),

                    new PizzaTopping("Podwójny ser"),
                    new PizzaTopping("Salami"),
                    new PizzaTopping("Szynka"),
                    new PizzaTopping("Pieczarki"),

                    new MainCourse("Schabowy z frytkami/ryżem/ziemniakami", 30m),
                    new MainCourse("Ryba z frytkami", 28m),
                    new MainCourse("Placek po węgiersku", 27m),

                    new MainCourseSideDish("Bar sałatkowy", 5m),
                    new MainCourseSideDish("Zestaw sosów", 6m),

                    new Soup("Zupa pomidorowa", 12m),
                    new Soup("Rosół", 10m),

                    new Beverage("Kawa"),
                    new Beverage("Herbata"),
                    new Beverage("Cola")

                };

                foreach (var prod in newPOCOProducts)
                {
                    var productEntity = new Product(prod.Name, prod.Price, prod.ProductType,
                    prod.Description, prod.Remarks);

                    dbContext.Products.Add(productEntity);
                }

                try
                {
                    dbContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    Console.ReadLine();
                    throw;
                }


            }
        }
    }
}
