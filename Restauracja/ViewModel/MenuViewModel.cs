using Restauracja.Model;
using Restauracja.Model.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restauracja.ViewModel
{
    public class MenuViewModel : BaseViewModel
    {
        private ObservableCollection<ProductPOCO> orderProducts = new ObservableCollection<ProductPOCO>();
        public ObservableCollection<ProductPOCO> OrderProducts
        {
            get { return orderProducts; }
            set
            {
                SetProperty(ref orderProducts, value);
            }
        }

        private string orderRemarks;
        public string OrderRemarks
        {
            get
            {
                return orderRemarks;
            }
            set
            {
                SetProperty(ref orderRemarks, value);
            }
        }

        

        private int orderCost;
        public int OrderCost
        {
            get { return orderCost; }
            set
            {
                SetProperty(ref orderCost, value);
            }
        }

        public List<Pizza> POCOPizzas { get; set; } = new List<Pizza>();
        public List<PizzaTopping> POCOPizzaToppings { get; set; } = new List<PizzaTopping>();
        public List<MainCourse> POCOMainCourses { get; set; } = new List<MainCourse>();
        public List<MainCourseSideDish> POCOMainCourseSideDishes { get; set; } = new List<MainCourseSideDish>();
        public List<Soup> POCOSoups { get; set; } = new List<Soup>();
        public List<Beverage> POCOBeverages { get; set; } = new List<Beverage>();

        private bool tabSelected;
        public bool TabSelected     // Add loading data from db on tab selection
        {
            get { return tabSelected; }
            set
            {
                SetProperty(ref tabSelected, value);
            }
        }

        private ProductPOCO toBeAdded;
        public ProductPOCO ToBeAdded
        {
            get
            {
                return toBeAdded;
            }
            set
            {
                SetProperty(ref toBeAdded, value);
            }
        }

        private ProductPOCO toBeRemoved;
        public ProductPOCO ToBeRemoved
        {
            get
            {
                return toBeRemoved;
            }
            set
            {
                SetProperty(ref toBeRemoved, value);
            }
        }

        public MenuViewModel()
        {
            GetPizzas();
            GetPizzaToppings();
            GetMainCourses();
            GetMainCourseSideDishes();
            GetSoups();
            GetBeverages();
        }

        public void GetOrderCost()      // Abstract that method to other class (repetition in OrderSummaryViewModel)
        {
            int orderCost = 0;
            foreach (var prod in OrderProducts)         //Change to show summary cost from DB
            {
                for (int i = 0; i < prod.Quantity; i++)
                {
                    orderCost += prod.Price;
                }
            }
            OrderCost = orderCost;
        }

        private void CreatePOCOProducts(List<Product> products)
        {
            foreach (var item in products)
            {
                if (item.Description == "Pizza")    // Change to enum
                {
                    var pocoPizza = new Pizza(item.Id, item.Name, item.Price, item.Quantity, item.Description, item.Remarks);
                    POCOPizzas.Add(pocoPizza);
                }

                if (item.Description == "Pizza topping")
                {
                    var pocoPizzaTopping = new PizzaTopping(item.Id, item.Name, item.Price, item.Quantity, item.Description, item.Remarks);
                    POCOPizzaToppings.Add(pocoPizzaTopping);
                }

                if (item.Description == "Main course")
                {
                    var pocoMainCourse = new MainCourse(item.Id, item.Name, item.Price, item.Description, item.Quantity, item.Remarks);
                    POCOMainCourses.Add(pocoMainCourse);
                }

                if (item.Description == "Main course side dish")
                {
                    var pocoMainCourseSideDish = new MainCourseSideDish(item.Id, item.Name, item.Price, item.Description, item.Quantity, item.Remarks);
                    POCOMainCourseSideDishes.Add(pocoMainCourseSideDish);
                }

                if (item.Description == "Soup")
                {
                    var pocoSoup = new Soup(item.Id, item.Name, item.Price, item.Description, item.Quantity, item.Remarks);
                    POCOSoups.Add(pocoSoup);
                }

                if (item.Description == "Beverage")
                {
                    var pocoBeverage = new Beverage(item.Id, item.Name, item.Price, item.Description, item.Quantity, item.Remarks);
                    POCOBeverages.Add(pocoBeverage);
                }
            }
        }

        private void GetPizzaToppings()
        {
            using (var myRestaurantContext = new RestaurantDataContext())
            {
                var query = from prod in myRestaurantContext.Products
                            where prod.Description == "Pizza topping"
                            select prod;
                CreatePOCOProducts(query.ToList());
            }
        }

        private void GetMainCourses()
        {
            using (var myRestaurantContext = new RestaurantDataContext())
            {
                var query = from prod in myRestaurantContext.Products
                            where prod.Description == "Main course"
                            select prod;
                CreatePOCOProducts(query.ToList());
            }
        }

        public void AddSelectedProductToOrder()
        {

            if (ToBeAdded != null)      // Add method to increase quantity for doubled product
            {
                if (!OrderProducts.Contains(ToBeAdded))
                {
                    OrderProducts.Add(ToBeAdded);
                }
                else
                {
                    ToBeAdded.Quantity++;
                }
            }
        }

        public void RemoveSelectedProductFromOrder()
        {
            if (ToBeRemoved != null)
            {
                ToBeRemoved.Quantity = 1;
                OrderProducts.Remove(ToBeRemoved);
            }
        }

        private void GetPizzas()
        {
            using (var myRestaurantContext = new RestaurantDataContext())
            {
                var query = from prod in myRestaurantContext.Products
                            where prod.Description == "Pizza"
                            select prod;

                CreatePOCOProducts(query.ToList());

                var ddd = "";
            }
        }

        private void GetMainCourseSideDishes()
        {
            using (var myRestaurantContext = new RestaurantDataContext())
            {
                var query = from prod in myRestaurantContext.Products
                            where prod.Description == "Main course side dish"
                            select prod;

                CreatePOCOProducts(query.ToList());
            }
        }

        private void GetSoups()
        {
            using (var myRestaurantContext = new RestaurantDataContext())
            {
                var query = from prod in myRestaurantContext.Products
                            where prod.Description == "Soup"
                            select prod;

                CreatePOCOProducts(query.ToList());
            }
        }

        private void GetBeverages()
        {
            using (var myRestaurantContext = new RestaurantDataContext())
            {
                var query = from prod in myRestaurantContext.Products
                            where prod.Description == "Beverage"
                            select prod;

                CreatePOCOProducts(query.ToList());
            }
        }

        //private ProductPOCO GetSingleProduct()
        //{
        //    // POBIERZ Z BAZY DANYCH

        //    using (var myRestaurantContext = new RestaurantDataContext())
        //    {
        //        var query = from prod in myRestaurantContext.Products
        //                    where prod.Id == ToBeAdded.Id
        //                    select prod;

        //        var product = query.First();
        //        var newPOCOProd = new Pizza(product.Id, product.Name, product.Price, product.Quantity,
        //            product.Description, product.Remarks);
        //        return newPOCOProd;
        //    }
        //}

        //private void UpdateProductPizzaInDb(ProductPOCO itemToBeUpdated)        // Change to update all products in an order
        //{
        //    var dbItem = new Product(itemToBeUpdated.Id, itemToBeUpdated.Name, itemToBeUpdated.Price, 
        //        itemToBeUpdated.Quantity, itemToBeUpdated.Description, itemToBeUpdated.Remarks);

        //    using (var myRestaurantContext = new RestaurantDataContext())
        //    {
        //        myRestaurantContext.Entry(dbItem).State = EntityState.Modified;
        //        myRestaurantContext.SaveChanges();
        //    }
        //}
    }
}
