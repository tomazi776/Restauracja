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

        public event EventHandler<OrderProductsEventArgs> OrderPlaced;

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

        private OrderPOCO order = new OrderPOCO();

        public OrderPOCO Order
        {
            get { return order; }
            set 
            { 
                SetProperty(ref order, value); 
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

        protected virtual void OnOrderPlaced(ObservableCollection<ProductPOCO> orderProducts)
        {
            if (OrderPlaced != null)
            {
                OrderPlaced(this, new OrderProductsEventArgs() { OrderProducts = orderProducts });
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
                    OrderSummaryViewModel = new OrderSummaryViewModel(eventAggregator);


                    eventAggregator.GetEvent<ProductsPOCOMessageSentEvent>().Publish(OrderProducts);
                    eventAggregator.GetEvent<ProductRemarksMessageSentEvent>().Publish(OrderRemarks);


                    Window orderWindow = new OrderSummaryWindow(); //menuVm.OrderProducts, menuVm.OrderRemarks
                    orderWindow.DataContext = OrderSummaryViewModel;
                    orderWindow.Show();
                    //orderWindow.Show();
                    //this.Close();
                    //MessageBox.Show(MenuViewModel.WELCOME_MESSAGE_CONTENT, MenuViewModel.WELCOME_MESSAGE_HEADER);
                    //break;



                    //IWindowService windowService = locator.GetService<IWindowService>();
                    //windowService.ShowWindow(this);

                    MessageBox.Show(WELCOME_MESSAGE_CONTENT, WELCOME_MESSAGE_HEADER);
                    break;
            }
        }

        public void GetSummaryCost()
        {
            OrderCost = Order.GetOrderCost<ProductPOCO>(OrderProducts);
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
