using Restauracja.Model;
using Restauracja.Model.Entities;
using Restauracja.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Restauracja.ViewModel
{
    public class OrderSummaryViewModel : BaseViewModel
    {
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

        private string sender = "urban776@gmail.com";
        public string Sender
        {
            get { return sender; }
            set
            {
                SetProperty(ref sender, value);
            }
        }


        private string subject;
        public string Subject
        {
            get { return subject; }
            set
            {
                SetProperty(ref subject, value);
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

            SendMailCommand = new CommandHandler(SendMail, () => true);
        }



        public void SendMail()
        {
            GetOrderCost();
            SaveOrderToDb();
            //ComposeEmailBody();

            //MailMessage email = new MailMessage(Sender, Recipent, Subject, EmailBody);
            //SmtpClient smtpClient = new SmtpClient(GMAIL_SMTP_HOST, GMAIL_SMTP_PORT);
            //smtpClient.Credentials = new NetworkCredential(Sender, "Junior77");
            //smtpClient.EnableSsl = true;
            //smtpClient.Timeout = 10000;
            //try
            //{
            //    smtpClient.Send(email);
            //}
            //catch (Exception ex )
            //{
            //    Console.WriteLine(ex.Message);
            //    throw;
            //}

            // Dodaj info o wysłaniu zamówienia
        }

        private void SaveOrderToDb()
        {
            using (var dbContext = new RestaurantDataContext())
            {
                Order newlyPlacedOrder = new Order(OrderCost, OrderRemarks);


                foreach (var prod in OrderSummaryProducts)
                {
                    var mappedProduct = MapToProduct(prod);
                    newlyPlacedOrder.Products.Add(mappedProduct);
                }

                dbContext.Orders.Add(newlyPlacedOrder);     
                dbContext.SaveChanges();
            }
        }

        private Product MapToProduct(ProductPOCO pocoProduct)
        {
            return new Product(pocoProduct.Name, pocoProduct.Price, pocoProduct.Quantity, pocoProduct.Description, pocoProduct.Remarks);
        }

        private void ComposeEmailBody()
        {
            EmailBody = "Zamówienie klienta '" + Sender + "':" + "\n";
            foreach (var product in OrderSummaryProducts)
            {
                EmailBody += "\n" + product.Name + " - " + product.Price.ToString() + " zł";
            }
            EmailBody += "\n" + "\n" + "Łączny koszt zamówienia: " + OrderCost + " zł";
            EmailBody += "\n" + "\n" + "Uwagi do zamówienia: " + OrderRemarks;
        }

        private void GetOrderCost()      // Abstract that method to other class (repetition in MenuViewModel)
        {
            int orderCost = 0;
            foreach (var prod in OrderSummaryProducts)         //Change to show summary cost from DB
            {
                for (int i = 0; i < prod.Quantity; i++)
                {
                    orderCost += prod.Price;
                }
            }
            OrderCost = orderCost;
        }


        private static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

        private static byte[] GetHash(string inputString)
        {
            HashAlgorithm algorithm = SHA256.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }
    }
}
