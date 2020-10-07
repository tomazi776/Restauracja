using Prism.Events;
using Restauracja.Model;
using Restauracja.Model.Entities;
using Restauracja.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Windows;
using System.Windows.Input;
using static Restauracja.ViewModel.MenuViewModel;

namespace Restauracja.ViewModel
{
    public class OrderSummaryViewModel : BaseViewModel
    {
        public const string EMAIL_SUBJECT = "Nowe zamówienie klienta";
        public event EventHandler<EventArgs> OrderSaved;

        public ICommand SendMailCommand { get; }
        public ICommand BackCommand { get;}
        public ICommand OrderHistoryCommand { get; set; }

        private ObservableCollection<ProductPOCO> orderProducts = new ObservableCollection<ProductPOCO>();
        public ObservableCollection<ProductPOCO> OrderSummaryProducts
        {
            get { return orderProducts; }
            set
            {
                if (orderProducts != value)
                {
                    SetProperty(ref orderProducts, value);
                    SingleOrder.Instance.OrderProducts = value;
                    SingleOrder.Instance.Order = this.Order;
                }
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
                    SingleOrder.Instance.Order = value;
                }
            }
        }

        //public MenuViewModel MenuViewModel { get; set; }
        public OrderHistoryViewModel HistoryVm { get; set; }

        private bool sendEnabled;
        public bool SendEnabled
        {
            get { return sendEnabled; }
            set 
            {
                SetProperty(ref sendEnabled, value);
            }
        }

        private string recipent;
        public string Recipent
        {
            get { return recipent; }
            set
            {
                SetProperty(ref recipent, value);
            }
        }

        private string sender;
        public string Sender
        {
            get { return sender; }
            set
            {
                if (sender != value)
                {
                    SetProperty(ref sender, value);
                    //SingleCustomer.GetInstance().Email = sender; //uncoment after testing
                }
            }
        }

        private string emailBody;
        public string EmailBody
        {
            get { return emailBody; }
            set
            {
                SetProperty(ref emailBody, value);
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

        public OrderSummaryViewModel(IEventAggregator eventAggregator)
        {
            HistoryVm = new OrderHistoryViewModel();
            Sender = SingleCustomer.GetInstance().Email;

            GetCachedData();
            OrderHistoryCommand = new CommandHandler(OpenOrderHistory, () => true);
            //SendMailCommand = new CommandHandler(SendMail, () => true);
            eventAggregator.GetEvent<OrderMessageSentEvent>().Subscribe(OrderMessageReceived);
        }

        private void OpenOrderHistory()
        {
            
        }

        public void GetCachedData()
        {
            if (SingleOrder.Instance.Order != null)
                Order = SingleOrder.Instance.Order;

            if (SingleOrder.Instance.Order?.Products?.Count > 0)
                OrderSummaryProducts = new ObservableCollection<ProductPOCO>(SingleOrder.Instance.Order.Products);

            //EnableDisablePlacingOrder(OrderProducts);

            Console.WriteLine("Got data from cache (ORDERSUMMARY_VM)!!!!!!!!!!!!!!");
        }

        private void OrderMessageReceived(OrderPOCO obj)
        {
            order = obj;
            OrderSummaryProducts = new ObservableCollection<ProductPOCO>(order.Products);
        }


        protected virtual void OnOrderSaved()
        {
            if (OrderSaved != null)
            {
                OrderSaved.Invoke(this, EventArgs.Empty);
            }
        }

        //TODO: Why create new order if its already made in MenuViewModel?
        public void MakeOrder()
        {
            SaveOrderToDb();
            OnOrderSaved();
        }

        private void SaveOrderToDb()
        {
            using (var dbContext = new RestaurantDataContext())
            {
                Customer newCustomer = new Customer(Sender);
                Order newlyPlacedOrder = new Order(Order.FinalCost, Order.Description, DateTime.Now);
                newlyPlacedOrder.Customer = newCustomer;

                foreach (var prod in OrderSummaryProducts)
                {
                    var orderItem = MapToOrderItem(prod);
                    newlyPlacedOrder.OrderItem.Add(orderItem);
                }
                dbContext.Orders.Add(newlyPlacedOrder);

                try
                {
                    dbContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    //TODO: Logging mechanism
                    Console.WriteLine("BŁĄD ZAPISU DO BAZY DANYCH - " + ex.Message);
                    MessageBox.Show("Nie można zapisać zamówienia w bazie danych," +
                        "/r/n sprawdź logi w pliku logs.txt lub skontaktuj się z administratorem.", "Błąd");
                    throw;
                }
            }
        }

        private OrderItem MapToOrderItem(ProductPOCO pocoProduct)
        {
            return new OrderItem(pocoProduct.Name, pocoProduct.Price, pocoProduct.Quantity, pocoProduct.Description, pocoProduct.Remarks);
        }
    }
}
