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

        //public List<ProductPOCO> MyProperty { get; set; }

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
            GetPizzas();        // Add Commands for tabs and move methods to their corresponding classes
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
                switch (item.ProductType)
                {
                    case ProductType.Pizza:
                        var newPizzaProduct = new Pizza(item.Name, item.Price, item.Remarks);
                        POCOPizzas.Add(newPizzaProduct);
                        break;

                    case ProductType.PizzaTopping:
                        var pocoPizzaTopping = new PizzaTopping(item.Name);
                        POCOPizzaToppings.Add(pocoPizzaTopping);
                        break;

                    case ProductType.MainCourse:
                        var pocoMainCourse = new MainCourse(item.Name, item.Price);
                        POCOMainCourses.Add(pocoMainCourse);
                        break;

                    case ProductType.MainCourseSideDish:
                        var pocoMainCourseSideDish = new MainCourseSideDish(item.Name, item.Price);
                        POCOMainCourseSideDishes.Add(pocoMainCourseSideDish);
                        break;

                    case ProductType.Beverage:
                        var pocoBeverage = new Beverage(item.Name);
                        POCOBeverages.Add(pocoBeverage);
                        break;

                    case ProductType.Soup:
                        var pocoSoup = new Soup(item.Name, item.Price);
                        POCOSoups.Add(pocoSoup);
                        break;

                    default:
                        break;
                }
            }
        }

        private void GetPizzaToppings()
        {
            using (var myRestaurantContext = new RestaurantDataContext())
            {
                var query = from prod in myRestaurantContext.Products
                            where prod.ProductType == ProductType.PizzaTopping
                            select prod;
                CreatePOCOProducts(query.ToList());
            }
        }

        private void GetMainCourses()
        {
            using (var myRestaurantContext = new RestaurantDataContext())
            {
                var query = from prod in myRestaurantContext.Products
                            where prod.ProductType == ProductType.MainCourse
                            select prod;
                CreatePOCOProducts(query.ToList());
            }
        }

        public void AddSelectedProductToOrder()
        {
            if (ToBeAdded != null)     
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
                            where prod.ProductType == ProductType.Pizza
                            select prod;
                CreatePOCOProducts(query.ToList());
            }
        }

        private void GetMainCourseSideDishes()
        {
            using (var myRestaurantContext = new RestaurantDataContext())
            {
                var query = from prod in myRestaurantContext.Products
                            where prod.ProductType == ProductType.MainCourseSideDish
                            select prod;
                CreatePOCOProducts(query.ToList());
            }
        }

        private void GetSoups()
        {
            using (var myRestaurantContext = new RestaurantDataContext())
            {
                var query = from prod in myRestaurantContext.Products
                            where prod.ProductType == ProductType.Soup
                            select prod;
                CreatePOCOProducts(query.ToList());
            }
        }

        private void GetBeverages()
        {
            using (var myRestaurantContext = new RestaurantDataContext())
            {
                var query = from prod in myRestaurantContext.Products
                            where prod.ProductType == ProductType.Beverage
                            select prod;
                CreatePOCOProducts(query.ToList());
            }
        }

        //private void GetProducts(List<Product> products)
        //{

        //    using (var myRestaurantContext = new RestaurantDataContext())
            

        //        foreach (var item in products)
        //        {
        //            if (item.ProductType == ProductType.Pizza)    // Change to enum
        //            {
        //                var newPizzaProduct = new Pizza(item.Name, item.Price, item.Remarks);
        //                POCOPizzas.Add(newPizzaProduct);
        //            }
        //            var query = from prod in myRestaurantContext.Products
        //                        where prod.ProductType == ProductType.Pizza
        //                        select prod;

        //            //CreatePOCOProducts(query.ToList());
        //            var pizzas = query.ToList();
        //            foreach (var item in pizzas)
        //            {
        //                POCOPizzas.Add(item);

        //            }
        //        }            
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
