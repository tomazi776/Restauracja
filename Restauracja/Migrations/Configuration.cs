namespace Restauracja.Migrations
{
    using Restauracja.Model;
    using Restauracja.Model.Entities;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Restauracja.RestaurantDataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Restauracja.RestaurantDataContext context)
        {
            //  This method will be called after migrating to the latest version.


            using (var dbContext = new RestaurantDataContext())
            {

                // CREATE PRODUCTS

                ProductPOCO margheritta = new Pizza(1,"Margheritta", 20, 1, "Z serem", "Moze zawierać soje");
                ProductPOCO vegetariana = new Pizza(2, "Vegetariana", 22, 1, "Z serem, z fetą i pomidorami", "Bezmięsna");
                ProductPOCO tosca = new Pizza(3, "Tosca", 25, 1, "Pizza Toskańska", "Z oliwą z oliwek");
                ProductPOCO venecia = new Pizza(4, "Venecia", 22, 1, "Z pieczarkami i szynką parmeńską", "Mięsna");

                ProductPOCO podwojnySer = new PizzaTopping(5,"Podwójny ser");
                ProductPOCO salami = new PizzaTopping(6, "Salami");
                ProductPOCO szynka = new PizzaTopping(7, "Szynka");
                ProductPOCO pieczarki = new PizzaTopping(8, "Pieczarki");

                ProductPOCO schabowyZDodatkami = new MainCourse(9, "Schabowy z frytkami/ryżem/ziemniakami", 30, "Schabowy, w zestawie: frytki, bądź ryż lub ziemniaki");
                ProductPOCO rybaZFrytkami = new MainCourse(10, "Ryba z frytkami", 28);
                ProductPOCO placekWegierski = new MainCourse(11, "Placek po węgiersku", 27);

                ProductPOCO barSalatkowy = new MainCourseSideDish(12, "Bar sałatkowy", 5);
                ProductPOCO zestawSosow = new MainCourseSideDish(13, "Zestaw sosów", 6);

                ProductPOCO pomidorowa = new Soup(14, "Zupa pomidorowa", 12);
                ProductPOCO rosol = new Soup(15, "Rosół", 10);

                ProductPOCO kawa = new Beverage(16, "Kawa");
                ProductPOCO herbata = new Beverage(17, "Herbata");
                ProductPOCO cola = new Beverage(18, "Cola");


                // MAP PRODUCTS (Mapping)

                var newMargherittaProduct = new Product
                {
                    //Id = capriciosa.Id,
                    Name = margheritta.Name,
                    Description = margheritta.Description,
                    Price = margheritta.Price,
                    Quantity = margheritta.Quantity,
                    Remarks = margheritta.Remarks,
                    //OrderId = clientOrder.Id
                };

                var newVegetarianaProduct = new Product
                {
                    //Id = vegetariana.Id,
                    Name = vegetariana.Name,
                    Description = vegetariana.Description,
                    Price = vegetariana.Price,
                    Quantity = vegetariana.Quantity,
                    Remarks = vegetariana.Remarks,
                    //OrderId = clientOrder.Id
                };

                var newToscaProduct = new Product
                {
                    //Id = coffe.Id,
                    Name = tosca.Name,
                    Description = tosca.Description,
                    Price = tosca.Price,
                    Quantity = tosca.Quantity,
                    Remarks = tosca.Remarks,
                    //OrderId = clientOrder.Id
                };

                var newVeneciaProduct = new Product
                {
                    //Id = coffe.Id,
                    Name = venecia.Name,
                    Description = venecia.Description,
                    Price = venecia.Price,
                    Quantity = venecia.Quantity,
                    Remarks = venecia.Remarks,
                    //OrderId = clientOrder.Id
                };

                // DODATKI DO PIZZY

                var newCheeseTopping = new Product
                {
                    //Id = coffe.Id,
                    Name = podwojnySer.Name,
                    Description = podwojnySer.Description,
                    Price = podwojnySer.Price,
                    Quantity = podwojnySer.Quantity,
                    Remarks = podwojnySer.Remarks,
                    //OrderId = clientOrder.Id
                };

                var newSalamiTopping = new Product
                {
                    //Id = coffe.Id,
                    Name = salami.Name,
                    Description = salami.Description,
                    Price = salami.Price,
                    Quantity = salami.Quantity,
                    Remarks = salami.Remarks,
                    //OrderId = clientOrder.Id
                };

                var newHamTopping = new Product
                {
                    //Id = coffe.Id,
                    Name = szynka.Name,
                    Description = szynka.Description,
                    Price = szynka.Price,
                    Quantity = szynka.Quantity,
                    Remarks = szynka.Remarks,
                    //OrderId = clientOrder.Id
                };

                var newChampignonsTopping = new Product
                {
                    //Id = coffe.Id,
                    Name = pieczarki.Name,
                    Description = pieczarki.Description,
                    Price = pieczarki.Price,
                    Quantity = pieczarki.Quantity,
                    Remarks = pieczarki.Remarks,
                    //OrderId = clientOrder.Id
                };


                // DANIA GŁÓWNE

                var newSchabowy = new Product
                {
                    //Id = coffe.Id,
                    Name = schabowyZDodatkami.Name,
                    Description = schabowyZDodatkami.Description,
                    Price = schabowyZDodatkami.Price,
                    Quantity = schabowyZDodatkami.Quantity,
                    Remarks = schabowyZDodatkami.Remarks,
                    //OrderId = clientOrder.Id
                };

                var newFishNChips = new Product
                {
                    //Id = coffe.Id,
                    Name = rybaZFrytkami.Name,
                    Description = rybaZFrytkami.Description,
                    Price = rybaZFrytkami.Price,
                    Quantity = rybaZFrytkami.Quantity,
                    Remarks = rybaZFrytkami.Remarks,
                    //OrderId = clientOrder.Id
                };

                var newPlacekWegierski = new Product
                {
                    //Id = coffe.Id,
                    Name = placekWegierski.Name,
                    Description = placekWegierski.Description,
                    Price = placekWegierski.Price,
                    Quantity = placekWegierski.Quantity,
                    Remarks = placekWegierski.Remarks,
                    //OrderId = clientOrder.Id
                };

                // PRZYSTAWKI DO DAN GLOWNYCH

                var newSaladBarSideDish = new Product
                {
                    //Id = coffe.Id,
                    Name = barSalatkowy.Name,
                    Description = barSalatkowy.Description,
                    Price = barSalatkowy.Price,
                    Quantity = barSalatkowy.Quantity,
                    Remarks = barSalatkowy.Remarks,
                    //OrderId = clientOrder.Id
                };

                var newSaucePlateSideDish = new Product
                {
                    //Id = coffe.Id,
                    Name = zestawSosow.Name,
                    Description = zestawSosow.Description,
                    Price = zestawSosow.Price,
                    Quantity = zestawSosow.Quantity,
                    Remarks = zestawSosow.Remarks,
                    //OrderId = clientOrder.Id
                };

                // ZUPY

                var newPomidorowaSoup = new Product
                {
                    //Id = coffe.Id,
                    Name = pomidorowa.Name,
                    Description = pomidorowa.Description,
                    Price = pomidorowa.Price,
                    Quantity = pomidorowa.Quantity,
                    Remarks = pomidorowa.Remarks,
                    //OrderId = clientOrder.Id
                };

                var newRosolSoup = new Product
                {
                    //Id = coffe.Id,
                    Name = rosol.Name,
                    Description = rosol.Description,
                    Price = rosol.Price,
                    Quantity = rosol.Quantity,
                    Remarks = rosol.Remarks,
                    //OrderId = clientOrder.Id
                };

                // NAPOJE

                var newKawa = new Product
                {
                    //Id = coffe.Id,
                    Name = kawa.Name,
                    Description = kawa.Description,
                    Price = kawa.Price,
                    Quantity = kawa.Quantity,
                    Remarks = kawa.Remarks,
                    //OrderId = clientOrder.Id
                };

                var newHerbata = new Product
                {
                    //Id = coffe.Id,
                    Name = herbata.Name,
                    Description = herbata.Description,
                    Price = herbata.Price,
                    Quantity = herbata.Quantity,
                    Remarks = herbata.Remarks,
                    //OrderId = clientOrder.Id
                };

                var newCola = new Product
                {
                    //Id = coffe.Id,
                    Name = cola.Name,
                    Description = cola.Description,
                    Price = cola.Price,
                    Quantity = cola.Quantity,
                    Remarks = cola.Remarks,
                    //OrderId = clientOrder.Id
                };

                dbContext.Products.Add(newMargherittaProduct);
                dbContext.Products.Add(newVegetarianaProduct);
                dbContext.Products.Add(newToscaProduct);
                dbContext.Products.Add(newVeneciaProduct);
                dbContext.Products.Add(newCheeseTopping);
                dbContext.Products.Add(newSalamiTopping);
                dbContext.Products.Add(newHamTopping);
                dbContext.Products.Add(newChampignonsTopping);
                dbContext.Products.Add(newSchabowy);
                dbContext.Products.Add(newFishNChips);
                dbContext.Products.Add(newPlacekWegierski);
                dbContext.Products.Add(newSaladBarSideDish);
                dbContext.Products.Add(newSaucePlateSideDish);
                dbContext.Products.Add(newPomidorowaSoup);
                dbContext.Products.Add(newRosolSoup);
                dbContext.Products.Add(newKawa);
                dbContext.Products.Add(newHerbata);
                dbContext.Products.Add(newCola);

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

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}
