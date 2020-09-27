using Prism.Events;
using Restauracja.Model;
using Restauracja.Model.Entities;
using Restauracja.Services;
using Restauracja.Utilities;
using Restauracja.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Restauracja.ViewModel
{
    public partial class MenuViewModel : BaseViewModel
    {

        public ICommand AddSelectedProductToOrderCommand { get; set; }
        public ICommand RemoveSelectedProductFromOrderCommand { get; set; }
        public ICommand PlaceOrderCommand { get; set; }
        IServiceLocator locator;
        private readonly IEventAggregator eventAggregator;


        public const string WELCOME_MESSAGE_HEADER = "Zamówienie już prawie złożone!";
        public const string WELCOME_MESSAGE_CONTENT = "Teraz tylko podaj maila, w celu wysłania zamówienia, mniam!";
        public const string CONFIRMATION_PROMPT_CONTENT = "Czy na pewno chcesz złożyć to zamówienie?";
        public const string CONFIRMATION_PROMPT_HEADER = "Uwaga";

        private ObservableCollection<ProductPOCO> orderProducts = new ObservableCollection<ProductPOCO>();
        public ObservableCollection<ProductPOCO> OrderProducts
        {
            get
            {
                //if (SingleOrder.Instance.OrderProducts?.Count > 0)
                //{
                //    orderProducts = new ObservableCollection<ProductPOCO>(SingleOrder.Instance.OrderProducts);
                //}
                //return orderProducts;

                if (SingleOrder.Instance.Order?.Products?.Count > 0)
                {
                    orderProducts = new ObservableCollection<ProductPOCO>(SingleOrder.Instance.Order.Products);
                }
                return orderProducts;
            }
            set
            {
                if (orderProducts != value)
                {
                    SetProperty(ref orderProducts, value);
                }
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

        //TODO: Dlaczego jesteś zjebem i wrzuciłeś koszt zamówienia tu, zamiast użyć final cost z Order?
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

        private OrderPOCO order = new OrderPOCO();
        public OrderPOCO Order
        {
            get { return order; }
            set 
            {
                if (order != value)
                {
                    SetProperty(ref order, value);
                }
            }
        }

        public OrderSummaryViewModel OrderSummaryViewModel { get; set; }

        //public MenuViewModel()
        //{

        //}

        public MenuViewModel(IEventAggregator ea)
        {
            AddSelectedProductToOrderCommand = new CommandHandler(AddSelectedProductToOrder, () => true);
            RemoveSelectedProductFromOrderCommand = new CommandHandler(RemoveSelectedProductFromOrder, () => true);

            PlaceOrderCommand = new CommandHandler(PlaceOrder, () => true);

            this.eventAggregator = ea;
            //locator = new ServiceLocator();

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

        public event EventHandler<OrderEventArgs> OrderPlacedWithData;

        public virtual void OnOrderPlacedWithData()
        {
            if (OrderPlacedWithData != null)
            {
                OrderPlacedWithData.Invoke(this, new OrderEventArgs() { Order = Order });
            }
        }

        private void PlaceOrder()
        {
            MessageBoxResult result = MessageBox.Show(CONFIRMATION_PROMPT_CONTENT, CONFIRMATION_PROMPT_HEADER, MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    //OrderSummaryViewModel = new OrderSummaryViewModel(OrderProducts);
                    //OrderPlaced += OrderSummaryViewModel.OnOrderPlaced;
                    CreateOrder();
                    OrderSummaryViewModel = new OrderSummaryViewModel(eventAggregator);

                    // TODO: Replace with one event sending aggregated data in one Order Object
                    eventAggregator.GetEvent<ProductsPOCOMessageSentEvent>().Publish(OrderProducts);
                    eventAggregator.GetEvent<ProductRemarksMessageSentEvent>().Publish(OrderRemarks);

                    OnOrderPlaced();

                    //IWindowService windowService = locator.GetService<IWindowService>();
                    //windowService.ShowWindow(this);

                    MessageBox.Show(WELCOME_MESSAGE_CONTENT, WELCOME_MESSAGE_HEADER);
                    break;
            }
        }

        private void CreateOrder()
        {
            //Order.FinalCost = 
        }

        public void GetSummaryCost()
        {
            Order.FinalCost = Order.GetOrderCost<ProductPOCO>(OrderProducts);
            //Order.FinalCost = 12.5m;
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
            GetSummaryCost();
        }

        public void RemoveSelectedProductFromOrder()
        {
            if (ToBeRemoved != null)
            {
                ToBeRemoved.Quantity = 1;
                OrderProducts.Remove(ToBeRemoved);
            }
            GetSummaryCost();
        }

        //TODO: Get rid of second parameter, decide upon type
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
