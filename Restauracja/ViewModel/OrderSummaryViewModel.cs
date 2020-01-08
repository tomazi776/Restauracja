using Restauracja.Model;
using Restauracja.Model.Entities;
using Restauracja.Utilities;
using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Mail;
using System.Windows.Input;

namespace Restauracja.ViewModel
{
    public class OrderSummaryViewModel : BaseViewModel
    {
        const string EMAIL_SUBJECT = "Nowe zamówienie klienta";
        const string GMAIL_SMTP_HOST = "smtp.gmail.com";
        const int GMAIL_SMTP_PORT = 587;
        public ICommand SendMailCommand { get; }

        private ObservableCollection<ProductPOCO> orderProducts = new ObservableCollection<ProductPOCO>();
        public ObservableCollection<ProductPOCO> OrderSummaryProducts
        {
            get { return orderProducts; }
            set
            {
                SetProperty(ref orderProducts, value);
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

        public OrderPOCO PlacedOrder { get; set; }

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
                SetProperty(ref sender, value);
                SingleCustomer.GetInstance().Email = sender;
            }
        }

        private string passPhrase;
        public string PassPhrase
        {
            get { return passPhrase; }
            set
            {
                SetProperty(ref passPhrase, value); 
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

        public OrderSummaryViewModel(ObservableCollection<ProductPOCO> products, string remarks)
        {
            OrderSummaryProducts = products;
            OrderRemarks = remarks;
            Sender = SingleCustomer.GetInstance().Email;
            SendMailCommand = new CommandHandler(SendMail, () => true);
        }

        public void SendMail()
        {
            GetOrderCost();
            SaveOrderToDb();
            ComposeEmailBody();

            MailMessage email = new MailMessage(Sender, Recipent, EMAIL_SUBJECT, EmailBody);
            SmtpClient smtpClient = new SmtpClient(GMAIL_SMTP_HOST, GMAIL_SMTP_PORT);
            if (Sender != null && PassPhrase != null)
            {
                smtpClient.Credentials = new NetworkCredential(Sender, PassPhrase);
            }
            smtpClient.EnableSsl = true;
            smtpClient.Timeout = 10000;
            try
            {
                smtpClient.Send(email);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        private void SaveOrderToDb()
        {
            using (var dbContext = new RestaurantDataContext())
            {
                Customer newCustomer = new Customer(Sender);
                Order newlyPlacedOrder = new Order(OrderCost, OrderRemarks, DateTime.Now);

                newlyPlacedOrder.Customer = newCustomer;
                newlyPlacedOrder.FinalCost = OrderCost;
                newlyPlacedOrder.Description = OrderRemarks;

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

        private void ComposeEmailBody()
        {
            EmailBody = "Zamówienie klienta '" + Sender + "':" + "\n";
            foreach (var product in OrderSummaryProducts)
            {
                EmailBody += "\n" + product.Name + " - " + product.Price.ToString() + "zł x " + product.Quantity +  " = " + product.Quantity * product.Price + " zł";
            }
            EmailBody += "\n" + "\n" + "Łączny koszt zamówienia: " + OrderCost + " zł";
            EmailBody += "\n" + "\n" + "Uwagi do zamówienia: " + OrderRemarks;
        }

        private void GetOrderCost()
        {
            int orderCost = 0;
            foreach (var prod in OrderSummaryProducts)
            {
                for (int i = 0; i < prod.Quantity; i++)
                {
                    orderCost += prod.Price;
                }
            }
            OrderCost = orderCost;
        }
    }
}
