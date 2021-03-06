﻿using Restauracja.Extensions;
using Restauracja.Model;
using Restauracja.Model.Entities;
using Restauracja.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Restauracja.ViewModel
{
    public partial class MenuViewModel : BaseViewModel, IWithCacheableData
    {

        public ICommand AddSelectedProductToOrderCommand { get; set; }
        public ICommand RemoveSelectedProductFromOrderCommand { get; set; }
        public ICommand PlaceOrderCommand { get; set; }

        public const string WELCOME_MESSAGE_HEADER = "Zamówienie już prawie złożone!";
        public const string WELCOME_MESSAGE_CONTENT = "Teraz tylko podaj maila, w celu wysłania zamówienia, mniam!";
        public const string CONFIRMATION_PROMPT_CONTENT = "Czy na pewno chcesz złożyć to zamówienie?";
        public const string CONFIRMATION_PROMPT_HEADER = "Uwaga";

        private ObservableCollection<ProductPOCO> orderProducts = new ObservableCollection<ProductPOCO>();
        public ObservableCollection<ProductPOCO> OrderProducts
        {
            get => orderProducts;
            set
            {
                if (orderProducts != value)
                {
                    SetProperty(ref orderProducts, value);
                }
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
            get => toBeAdded;
            set
            {
                if (toBeAdded != value)
                {
                    SetProperty(ref toBeAdded, value);
                }
            }
        }

        private bool placeOrderEnabled;
        public bool PlaceOrderEnabled
        {
            get => placeOrderEnabled;
            set
            {
                if (placeOrderEnabled != value)
                {
                    SetProperty(ref placeOrderEnabled, value);
                }
            }
        }

        public bool IsSameOrder { get; set; }

        private ProductPOCO toBeRemoved;
        public ProductPOCO ToBeRemoved
        {
            get => toBeRemoved;
            set
            {
                if (toBeRemoved != value)
                {
                    SetProperty(ref toBeRemoved, value);
                }
            }
        }

        private OrderPOCO order = new OrderPOCO();
        public OrderPOCO Order
        {
            get => order;
            set 
            {
                if (order != value)
                {
                    SetProperty(ref order, value);
                }
            }
        }

        public OrderSummaryViewModel OrderSummaryViewModel { get; set; }

        public MenuViewModel()
        {
            AddSelectedProductToOrderCommand = new CommandHandler(AddSelectedProductToOrder, () => true);
            RemoveSelectedProductFromOrderCommand = new CommandHandler(RemoveSelectedProductFromOrder, () => true);

            PlaceOrderCommand = new CommandHandler(PlaceOrder, () => true);
            this.GetCatchedData();
            EnableDisablePlacingOrder(OrderProducts);

            GetProducts(ProductType.Pizza, POCOPizzas);
            GetProducts(ProductType.PizzaTopping, POCOPizzaToppings);
            GetProducts(ProductType.MainCourse, POCOMainCourses);
            GetProducts(ProductType.MainCourseSideDish, POCOMainCourseSideDishes);
            GetProducts(ProductType.Soup, POCOSoups);
            GetProducts(ProductType.Beverage, POCOBeverages);
        }

        public event EventHandler<EventArgs> OrderPlaced;

        public virtual void OnOrderPlaced()
        {
            if (OrderPlaced != null)
            {
                OrderPlaced.Invoke(this, EventArgs.Empty);
            }
        }

        private void PlaceOrder()
        {
            MessageBoxResult result = MessageBox.Show(CONFIRMATION_PROMPT_CONTENT, CONFIRMATION_PROMPT_HEADER, MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    //Pass Order via DI
                    AssignOrderId(Order);
                    OrderSummaryViewModel = new OrderSummaryViewModel(Order);

                    OnOrderPlaced();
                    MessageBox.Show(WELCOME_MESSAGE_CONTENT, WELCOME_MESSAGE_HEADER);
                    break;
            }
        }

        private void AssignOrderId(OrderPOCO ord)
        {
            int lastOrderId = 0;
            using (var dbContext = new RestaurantDataContext())
            {
                var orders = dbContext.Orders;
                if (orders.Any())
                {
                    lastOrderId = orders.OrderByDescending(o => o.Id).Select(order => order).FirstOrDefault().Id;
                }
            }
            ord.Id = lastOrderId;
            if (IsSameOrder)
                return;
            else
                ord.Id++;
        }

        private void UpdateSummaryCost()
        {
            Order.FinalCost = Order.GetOrderCost<ProductPOCO>(Order.Products);
        }

        private void CreatePOCOProducts(List<Product> products, List<IProduct> pocoPrtoducts)
        {
            ProductFactory productFactory = new ProductFactory();
            foreach (var product in products)
            {
                pocoPrtoducts.Add(productFactory.CreatePOCOProduct(product));
            }
        }

        private void AddSelectedProductToOrder()
        {
            if (ToBeAdded != null)
            {
                EnableDisablePlacingOrder(OrderProducts);
                AddIncrementProduct();                
            }
            EnableDisablePlacingOrder(OrderProducts);
            UpdateSummaryCost();
        }

        private void AddIncrementProduct()
        {
            if (ProductExistsInOrder(ToBeAdded))
            {
                var product = OrderProducts.FirstOrDefault(prod => prod.Name == ToBeAdded.Name).Quantity++;
            }
            else
            {
                Order.Products.Add(ToBeAdded); // for data to be passed further
                OrderProducts.Add(ToBeAdded); // for display in this View
                IsSameOrder = false;
            }
        }

        private bool ProductExistsInOrder(ProductPOCO productPoco)
        {
            var existingInOrder = Order.Products.Where(product => product.Name == productPoco.Name).ToList();
            return existingInOrder.Any();
        }

        private void EnableDisablePlacingOrder(ObservableCollection<ProductPOCO> orderProducts)
        {
            PlaceOrderEnabled = orderProducts.Count() == 0 ? false : true;
        }

        private void RemoveSelectedProductFromOrder()
        {
            if (ToBeRemoved != null)
            {
                ToBeRemoved.Quantity = 1;
                Order.Products.Remove(ToBeRemoved); // for data to be passed further passed
                OrderProducts.Remove(ToBeRemoved); // for display in this View
                IsSameOrder = false;
                EnableDisablePlacingOrder(OrderProducts);
            }
            UpdateSummaryCost();
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
