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
using System.Windows.Input;
using static Restauracja.ViewModel.MenuViewModel;

namespace Restauracja.ViewModel
{
    public class OrderSummaryViewModel : BaseViewModel
    {
        public const string EMAIL_SUBJECT = "Nowe zamówienie klienta";
        public const string GMAIL_SMTP_HOST = "smtp.gmail.com";
        const string CLIENT_ORDER_TEXT = "Zamówienie klienta '";
        const string ORDER_COST_TEXT = "Łączny koszt zamówienia: ";
        const string ORDER_REMARKS_TEXT = "Uwagi do zamówienia: ";
        const string DOUBLE_LINE_BREAK = "\r\n\r\n";
        const string SUMMARYLINE = "__________________________________";

        public const int GMAIL_SMTP_PORT = 587;

        public ICommand SendMailCommand { get; }
        public ICommand BackCommand { get;}

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

        public MenuViewModel MenuViewModel { get; set; }

        private bool sendEnabled;
        public bool SendEnabled
        {
            get { return sendEnabled; }
            set 
            {
                SetProperty(ref sendEnabled, value);
            }
        }

        //public OrderPOCO PlacedOrder { get; set; }

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
                    SingleCustomer.GetInstance().Email = sender;
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
            Sender = SingleCustomer.GetInstance().Email;
            //SendMailCommand = new CommandHandler(SendMail, () => true);

            eventAggregator.GetEvent<OrderMessageSentEvent>().Subscribe(OrderMessageReceived);
        }

        private void OrderMessageReceived(OrderPOCO obj)
        {
            order = obj;
            OrderSummaryProducts = new ObservableCollection<ProductPOCO>(order.Products);
        }

        //TODO: Why create new order if its already made in MenuViewModel?
        public void MakeOrder()
        {
            OrderPOCO order = new OrderPOCO();
            //OrderCost = order.GetOrderCost<ProductPOCO>(OrderSummaryProducts);
            //order.FinalCost = order.GetOrderCost<ProductPOCO>(Order.Products);
            SaveOrderToDb();
        }

        // This logic is in the codebehind for security reasons
        //public void SendMail()
        //{
        //    OrderPOCO order = new OrderPOCO();
        //    OrderCost = order.GetOrderCost<ProductPOCO>(OrderSummaryProducts);

        //    GetOrderCost();
        //    SaveOrderToDb();

        //    var emailBody = ComposeEmailBody();

        //    MailMessage email = new MailMessage(Sender, Recipent, EMAIL_SUBJECT, emailBody); // instead EmailBody
        //    SmtpClient smtpClient = new SmtpClient(GMAIL_SMTP_HOST, GMAIL_SMTP_PORT);
        //    if (Sender != null && PassPhrase != null)
        //    {
        //        smtpClient.UseDefaultCredentials = true;
        //        smtpClient.Credentials = new NetworkCredential(Sender, PassPhrase);
        //    }
        //    smtpClient.EnableSsl = true;
        //    smtpClient.Timeout = 10000;
        //    try
        //    {
        //        smtpClient.Send(email);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        throw;
        //    }
        //}

        private void SaveOrderToDb()
        {
            using (var dbContext = new RestaurantDataContext())
            {
                Customer newCustomer = new Customer(Sender);
                Order newlyPlacedOrder = new Order(Order.FinalCost, Order.Description, DateTime.Now);

                newlyPlacedOrder.Customer = newCustomer;
                //newlyPlacedOrder.FinalCost = OrderCost;
                //newlyPlacedOrder.Description = Order.Description;

                foreach (var prod in OrderSummaryProducts)
                {
                    var orderItem = MapToOrderItem(prod);
                    newlyPlacedOrder.OrderItem.Add(orderItem);
                }
                dbContext.Orders.Add(newlyPlacedOrder);
                dbContext.SaveChanges();
            }
        }

        private OrderItem MapToOrderItem(ProductPOCO pocoProduct)
        {
            return new OrderItem(pocoProduct.Name, pocoProduct.Price, pocoProduct.Quantity, pocoProduct.Description, pocoProduct.Remarks);
        }

        // TODO: change OrderCost to decimal and apply formatting for currency in XAML instead appending it here 
        // (edit - has to be here cause its for email not displaing in app)
        public string ComposeEmailBody()
        {
            var currency = CultureInfo.CreateSpecificCulture("pl-PL").NumberFormat.CurrencySymbol;

            StringBuilder orderContent = new StringBuilder();
            foreach (var product in OrderSummaryProducts)
            {
                orderContent
                    .Append(product.Name)
                    .Append(" - ")
                    .Append(product.Price)
                    .Append($" {currency} x ")
                    .Append(product.Quantity)
                    .Append(" = ")
                    .Append(product.Quantity * product.Price).AppendLine($" {currency}");
            }
            orderContent
                .Append(SUMMARYLINE)
                .Append(string.Concat(DOUBLE_LINE_BREAK, ORDER_COST_TEXT, Order.FinalCost, $" {currency}"))
                .Append(string.Concat(DOUBLE_LINE_BREAK, ORDER_REMARKS_TEXT, Order.Description));
            //EmailBody = string.Concat(CLIENT_ORDER_TEXT, Sender, "':", DOUBLE_LINE_BREAK, orderContent);
            var emailBody = string.Concat(CLIENT_ORDER_TEXT, Sender, "':", DOUBLE_LINE_BREAK, orderContent);
            return emailBody;
        }
    }
}
