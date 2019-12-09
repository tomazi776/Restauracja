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
            using (var dbContext = new RestaurantDataContext())
            {
                List<ProductPOCO> newPOCOProducts = new List<ProductPOCO>()
                {
                    new Pizza("Margheritta", 20,"Sos, ser mozarella, oregano"),
                    new Pizza("Vegetariana", 22,"Bezmięsna"),
                    new Pizza("Tosca", 25,"Z oliwą z oliwek"),
                    new Pizza("Venecia", 22, "Mięsna"),

                    new PizzaTopping("Podwójny ser"),
                    new PizzaTopping("Salami"),
                    new PizzaTopping("Szynka"),
                    new PizzaTopping("Pieczarki"),

                    new MainCourse("Schabowy z frytkami/ryżem/ziemniakami", 30),
                    new MainCourse("Ryba z frytkami", 28),
                    new MainCourse("Placek po węgiersku", 27),

                    new MainCourseSideDish("Bar sałatkowy", 5),
                    new MainCourseSideDish("Zestaw sosów", 6),

                    new Soup("Zupa pomidorowa", 12),
                    new Soup("Rosół", 10),

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
