using Restauracja.Model;
using Restauracja.Model.Entities;
using Restauracja.Utilities;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Restauracja.ViewModel
{
    public class OrderSummaryViewModel : BaseViewModel
    {
        public const string EMAIL_SUBJECT = "Nowe zamówienie klienta";
        public event EventHandler<EventArgs> OrderSaved;

        public ICommand FinalizeOrderCommand { get; }
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
                    if (value != null)
                    {
                        SingleOrder.Instance.Order = value;
                    }
                }
            }
        }

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
        
        public OrderSummaryViewModel(OrderPOCO order = null )
        {
            Order = order;
            Sender = SingleCustomer.Instance.Email;

            GetCachedData();
            OrderHistoryCommand = new CommandHandler(OpenOrderHistory, () => true);
            FinalizeOrderCommand = new CommandHandler(SaveOrderToDb, () => true);
        }

        private void OpenOrderHistory()
        {
            // open History by command
        }

        public void GetCachedData()
        {
            if (SingleOrder.Instance.Order != null)
                Order = SingleOrder.Instance.Order;

            if (SingleOrder.Instance.Order?.Products?.Count > 0)
                OrderSummaryProducts = new ObservableCollection<ProductPOCO>(SingleOrder.Instance.Order.Products);

            Console.WriteLine("Got data from cache (ORDERSUMMARY_VM)!!!!!!!!!!!!!!");
        }

        protected virtual void OnOrderSaved()
        {
            if (OrderSaved != null)
            {
                OrderSaved.Invoke(this, EventArgs.Empty);
            }
        }

        //TODO: Why make new order if its already made in MenuViewModel and sent here?
        // Cause its needed now to map an OrderPOCO to Order object
        private void SaveOrderToDb()
        {
            using (var dbContext = new RestaurantDataContext())
            {
                Sender = "urban776@gmail.com"; // for testing
                Customer newCustomer = new Customer(Sender);

                // Rethink having separate Entity and Domain objects (Order / OrderPOCO)
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
                    OnOrderSaved();
                }
                catch (Exception ex)
                {
                    //TODO: Logging mechanism
                    MessageBox.Show("Nie można zapisać zamówienia w bazie danych," + "Szczegóły:" + ex.Message + 
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
