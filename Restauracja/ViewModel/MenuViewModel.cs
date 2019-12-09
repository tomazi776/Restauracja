using Restauracja.Model;
using Restauracja.Model.Entities;
using Restauracja.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Restauracja.ViewModel
{
    public class MenuViewModel : BaseViewModel
    {
        public const string WELCOME_MESSAGE_HEADER = "Zamówienie już prawie złożone!";
        public const string WELCOME_MESSAGE_CONTENT = "Teraz tylko podaj maila, w celu wysłania zamówienia, mniam!";
        public const string CONFIRMATION_PROMPT_CONTENT = "Czy na pewno chcesz złożyć to zamówienie?";
        public const string CONFIRMATION_PROMPT_HEADER = "Uwaga";

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

        public List<IProduct> POCOPizzas { get; set; } = new List<IProduct>();
        public List<IProduct> POCOPizzaToppings { get; set; } = new List<IProduct>();
        public List<IProduct> POCOMainCourses { get; set; } = new List<IProduct>();
        public List<IProduct> POCOMainCourseSideDishes { get; set; } = new List<IProduct>();
        public List<IProduct> POCOSoups { get; set; } = new List<IProduct>();
        public List<IProduct> POCOBeverages { get; set; } = new List<IProduct>();

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
            GetProducts(ProductType.Pizza, POCOPizzas);
            GetProducts(ProductType.PizzaTopping, POCOPizzaToppings);
            GetProducts(ProductType.MainCourse, POCOMainCourses);
            GetProducts(ProductType.MainCourseSideDish, POCOMainCourseSideDishes);
            GetProducts(ProductType.Soup, POCOSoups);
            GetProducts(ProductType.Beverage, POCOBeverages);
        }

        public void GetOrderCost()
        {
            int orderCost = 0;
            foreach (var prod in OrderProducts)
            {
                for (int i = 0; i < prod.Quantity; i++)
                {
                    orderCost += prod.Price;
                }
            }
            OrderCost = orderCost;
        }

        private void CreatePOCOProducts(List<Product> products, List<IProduct> pocoPrtoducts)
        {
            ProductFactory productFactory = new ProductFactory();
            foreach (var product in products)
            {
                pocoPrtoducts.Add(productFactory.CreatePOCOProduct(product));
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

        private void GetProducts(ProductType productType, List<IProduct> products)
        {
            using (var myRestaurantContext = new RestaurantDataContext())
            {
                var query = from prod in myRestaurantContext.Products
                            where prod.ProductType == productType
                            select prod;
                CreatePOCOProducts(query.ToList(), products);
            }
        }
    }
}
