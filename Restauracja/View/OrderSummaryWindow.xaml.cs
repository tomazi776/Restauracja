using Restauracja.Model;
using Restauracja.Utilities;
using Restauracja.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Restauracja.View
{
    /// <summary>
    /// Interaction logic for OrderSummaryWindow.xaml
    /// </summary>
    public partial class OrderSummaryWindow : Window
    {
        OrderSummaryViewModel summaryOrderVm;
        public OrderSummaryWindow()
        {
            InitializeComponent();
            summaryOrderVm = DataContext as OrderSummaryViewModel;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Window main = new MainWindow();
            main.Show();
            this.Close();
        }

        private void OrderHistoryButton_Click(object sender, RoutedEventArgs e)
        {
            Window orderHistoryWindow = new OrderHistoryWindow();
            orderHistoryWindow.Show();
            //this.Close();
        }

        //Uncoment for enable/disable btn to work (commentd for testing)
        private void RecipentTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ////summaryOrderVm.SendEnabled = EmailValidator.IsValidEmailAddress(RecipentTextBox.Text);
            //var viewModel = this.DataContext;
            //var orderSummaryVm = viewModel as OrderSummaryViewModel;

            //if (orderSummaryVm != null)
            //{
            //    orderSummaryVm.SendEnabled = EmailValidator.IsValidEmailAddress(RecipentTextBox.Text);
            //}
        }

        private void SendMail_BtnClick(object sender, RoutedEventArgs e)
        {
            var viewModel = this.DataContext;
            var orderSummaryVm = viewModel as OrderSummaryViewModel;

            if (orderSummaryVm is null)
            {
                Console.WriteLine("Viewmodel is null!");
                return;
            }

            //Add event for when the order is saved in DB
            orderSummaryVm.MakeOrder();

            //TODO: Send mail only if it's properly saved in the DB
            //if (orderSummaryVm.OrderSaved)
            //{
            //    //do the rest
            //}

            orderSummaryVm.Recipent = "tomaszurbaniak776@gmail.com"; // for testing
            orderSummaryVm.Sender = "urban776@gmail.com"; // for testing

            EmailService emailGenerator = new EmailService(orderSummaryVm.Sender, orderSummaryVm.Recipent, orderSummaryVm.Order);
            var emailBodyInHtml = emailGenerator.GenerateEmail();

            MailMessage email = new MailMessage(orderSummaryVm.Sender, orderSummaryVm.Recipent); // instead EmailBody
            email.IsBodyHtml = true;
            email.Body = emailBodyInHtml;
            email.Subject = OrderSummaryViewModel.EMAIL_SUBJECT;
      
            SmtpClient smtpClient = new SmtpClient(EmailService.GMAIL_SMTP_HOST, EmailService.GMAIL_SMTP_PORT);

            //if (orderSummaryVm.Sender != null && PassBox.SecurePassword != null)
            if (orderSummaryVm.Sender != null)
            {
                smtpClient.UseDefaultCredentials = true;
                //smtpClient.Credentials = new NetworkCredential(orderSummaryVm.Sender, PassBox.SecurePassword);
                smtpClient.Credentials = new NetworkCredential(orderSummaryVm.Sender, "Junior77");

            }
            smtpClient.EnableSsl = true;
            smtpClient.Timeout = 10000;
            try
            {
                smtpClient.Send(email);
            }
            // handle timeout ex and authorization ex and connection ex
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
