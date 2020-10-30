using Restauracja.Extensions;
using Restauracja.Model;
using Restauracja.Model.Entities;
using Restauracja.Services;
using Restauracja.Utilities;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Restauracja.ViewModel
{
    public class OrderSummaryViewModel : BaseViewModel, IWithCacheableData
    {
        public event EventHandler<EventArgs> OrderSaved;

        public ICommand FinalizeOrderCommand { get; }
        public ICommand BackCommand { get;}

        private ObservableCollection<ProductPOCO> orderProducts = new ObservableCollection<ProductPOCO>();
        public ObservableCollection<ProductPOCO> OrderProducts
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
                // protect from assigning null instead overloading with parameterless ctor
                if (order != value && value != null)
                {
                    SetProperty(ref order, value);
                    SingleOrder.Instance.Order = value;
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
        
        public OrderSummaryViewModel(OrderPOCO order = null )
        {
            Order = order;
            Sender = SingleCustomer.Instance.Email;
            var summaryProds = OrderProducts;

            this.GetCatchedData();
            FinalizeOrderCommand = new CommandHandler(TrySaveOrderToDb, () => true);
        }

        public void TrySendEmail(SecureString pass)
        {
            EmailService emailService = new EmailService(Sender, Order);
            string emailBodyInHtml = TryGenerateEmail(emailService);
            try
            {
                emailService.SendEmail(this, emailBodyInHtml, pass);
                UpdateOrder(orderSent: true);
                MessageBox.Show($"Twoje zamówienie zostało zapisane i wysłane na email '{Recipent}'", "Sukces!");
            }
            catch (Exception ex)
            {
                Logger.Log(LogTarget.File, ex);
                Logger.Log(LogTarget.EventLog, ex);
                UpdateOrder(orderSent: false);
                MessageBox.Show(@"Could not send an email - ensure you have internet connection. If so - check Logs.txt file in '\Data' installation foler or in Windows Event Viewer for details.", "Error!");
            }
        }

        private void TrySaveOrderToDb()
        {
            if (IsSameAsLastOrderId())
            {
                UpdateOrder(orderSent:true);
                OnOrderSaved();
                return;
            }

            using (var dbContext = new RestaurantDataContext())
            {
                try
                {
                    CreateNewOrder(dbContext);
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

        private bool IsSameAsLastOrderId()
        {
            using (var dbContext = new RestaurantDataContext())
            {
                Order lastOrder = null;
                var orders = dbContext.Orders;
                if (orders.Any())
                {
                     lastOrder = dbContext.Orders.OrderByDescending(o => o.Id).Select(order => order).FirstOrDefault();
                }
                else
                {
                    return false;
                }
                return lastOrder.Id == Order.Id;
            }
        }

        protected virtual void OnOrderSaved()
        {
            if (OrderSaved != null)
            {
                OrderSaved.Invoke(this, EventArgs.Empty);
            }
        }

        private void CreateNewOrder(RestaurantDataContext dataContext)
        {
            Order order = new Order(new Customer(Sender), Order.FinalCost, Order.Description, DateTime.Now);
            AddProductsTo(order);
            dataContext.Orders.Add(order);
        }

        private string TryGenerateEmail(EmailService emailService)
        {
            try
            {
                var emailBodyInHtml = emailService.GenerateEmail();
                return emailBodyInHtml;
            }
            catch (Exception ex)
            {
                Logger.Log(LogTarget.File, ex);
                Logger.Log(LogTarget.EventLog, ex);
                MessageBox.Show(@"Error while generating email - check Logs.txt file in '\Data' installation foler or in Windows Event Viewer for details.", "Error!");
                throw;
            }
        }

        private void UpdateOrder(bool orderSent)
        {
            using (var dbContext = new RestaurantDataContext())
            {
                var currentOrder = dbContext.Orders.OrderByDescending(o => o.Id).Select(order => order).FirstOrDefault();
                currentOrder.Sent = orderSent? 1 : 0;
                currentOrder.Description = SingleOrder.Instance.Order.Description;
                dbContext.SaveChanges();
            }
        }

        private void AddProductsTo(Order order)
        {
            foreach (var prod in OrderProducts)
            {
                var orderItem = MapToOrderItem(prod);
                order.OrderItem.Add(orderItem);
            }
        }

        private OrderItem MapToOrderItem(ProductPOCO pocoProduct)
        {
            return new OrderItem(pocoProduct.Name, pocoProduct.Price, pocoProduct.Quantity, pocoProduct.Description, pocoProduct.Remarks);
        }
    }
}
