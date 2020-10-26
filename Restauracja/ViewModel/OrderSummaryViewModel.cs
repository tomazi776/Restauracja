using Restauracja.Model;
using Restauracja.Model.Entities;
using Restauracja.Services;
using Restauracja.Utilities;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Restauracja.ViewModel
{
    public class OrderSummaryViewModel : BaseViewModel
    {
        public event EventHandler<EventArgs> OrderSaved;

        public ICommand FinalizeOrderCommand { get; }
        public ICommand BackCommand { get;}

        private ObservableCollection<ProductPOCO> orderProducts = new ObservableCollection<ProductPOCO>();
        public ObservableCollection<ProductPOCO> OrderSummaryProducts
        {
            get => orderProducts;
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

                    // protect from assigning null instead overloading with parameterless ctor
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
            get => sendEnabled;
            set 
            {
                SetProperty(ref sendEnabled, value);
            }
        }

        private string recipent;
        public string Recipent
        {
            get => recipent;
            set
            {
                SetProperty(ref recipent, value);
            }
        }

        private string sender;
        public string Sender
        {
            get => sender;
            set
            {
                if (sender != value)
                {
                    SetProperty(ref sender, value);
                    SingleCustomer.Instance.Email = sender;
                }
            }
        }

        private string emailBody;
        public string EmailBody
        {
            get => emailBody;
            set
            {
                SetProperty(ref emailBody, value);
            }
        }

        private int orderCost;
        public int OrderCost
        {
            get => orderCost;
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
            FinalizeOrderCommand = new CommandHandler(SaveOrderToDb, () => true);
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
                    OnOrderSaved();
                }
                catch (Exception ex)
                {
                    Logger.Log(LogTarget.File, ex);
                    Logger.Log(LogTarget.EventLog, ex);
                    MessageBox.Show(@"Could not save the order in the database - check 'Logs.txt' file in '\Data' installation foler or in Windows Event Viewer for details.", "Error!");
                }
            }
        }

        private OrderItem MapToOrderItem(ProductPOCO pocoProduct)
        {
            return new OrderItem(pocoProduct.Name, pocoProduct.Price, pocoProduct.Quantity, pocoProduct.Description, pocoProduct.Remarks);
        }
    }
}
