using Restauracja.Model;
using Restauracja.Utilities;
using Restauracja.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Mail;
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

        private void RecipentTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //summaryOrderVm.SendEnabled = EmailValidator.IsValidEmailAddress(RecipentTextBox.Text);
            var viewModel = this.DataContext;
            var orderSummaryVm = viewModel as OrderSummaryViewModel;

            if (orderSummaryVm != null)
            {
                orderSummaryVm.SendEnabled = EmailValidator.IsValidEmailAddress(RecipentTextBox.Text);
            }
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

            orderSummaryVm.MakeOrder();
            var emailBody = orderSummaryVm.ComposeEmailBody();

            MailMessage email = new MailMessage(orderSummaryVm.Sender, orderSummaryVm.Recipent, OrderSummaryViewModel.EMAIL_SUBJECT, emailBody); // instead EmailBody
            SmtpClient smtpClient = new SmtpClient(OrderSummaryViewModel.GMAIL_SMTP_HOST, OrderSummaryViewModel.GMAIL_SMTP_PORT);

            if (orderSummaryVm.Sender != null && PassBox.SecurePassword != null)
            {
                smtpClient.UseDefaultCredentials = true;
                smtpClient.Credentials = new NetworkCredential(orderSummaryVm.Sender, PassBox.SecurePassword);
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
    }
}
