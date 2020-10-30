using Restauracja.Services;
using Restauracja.Utilities;
using Restauracja.ViewModel;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Windows;
using System.Windows.Controls;

namespace Restauracja.View
{
    public partial class OrderSummaryWindow : Window
    {
        public OrderSummaryWindow()
        {
            InitializeComponent();
            this.Loaded += OrderSummaryWindow_Loaded;
        }

        private void OrderSummaryWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var orderSummaryVm = this.DataContext as OrderSummaryViewModel;
            if (orderSummaryVm != null)
            {
                orderSummaryVm.OrderSaved += SendEmailOnOrderSaved;
            }
            //Console.WriteLine("Viewmodel is null!");
            //return;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Window main = new MainWindow();
            main.Show();
            var menuVm = main.DataContext as MenuViewModel;
            if (menuVm != null)
            {
                menuVm.IsSameOrder = true;
            }
            this.Close();
        }

        private void OrderHistoryButton_Click(object sender, RoutedEventArgs e)
        {
            Window historyWindow = new OrderHistoryWindow();
            historyWindow.Show();
            this.Close();
        }

        private void RecipentTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var viewModel = this.DataContext;
            var orderSummaryVm = viewModel as OrderSummaryViewModel;
            if (orderSummaryVm != null)
            {
                orderSummaryVm.SendEnabled = EmailValidator.IsValidEmailAddress(RecipentTextBox.Text);
            }
        }

        private void SendEmailOnOrderSaved(object sender, EventArgs e)
        {
            OrderSummaryViewModel orderSummaryVm = sender as OrderSummaryViewModel;
            if (orderSummaryVm != null)
            {
                orderSummaryVm.TrySendEmail(PassBox.SecurePassword);
            }
        }


        //private void SendEmailOnOrderSaved(object sender, EventArgs e)
        //{
        //    OrderSummaryViewModel orderSummaryVm = sender as OrderSummaryViewModel;
        //    if (orderSummaryVm is null)
        //    {
        //        Console.WriteLine("OrderSummaryViewModel sender is null!!!");
        //        return;
        //    }

        //    EmailService emailService = new EmailService(orderSummaryVm.Sender, orderSummaryVm?.Order);
        //    string emailBodyInHtml;

        //    try
        //    {
        //        emailBodyInHtml = emailService.GenerateEmail();
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Log(LogTarget.File, ex);
        //        Logger.Log(LogTarget.EventLog, ex);
        //        MessageBox.Show(@"Error while generating email - check Logs.txt file in '\Data' installation foler or in Windows Event Viewer for details.", "Error!");
        //        throw;
        //    }

        //    try
        //    {
        //        emailService.SendEmail(orderSummaryVm, emailBodyInHtml, PassBox.SecurePassword);
        //        AlterOrder(sent:true);
        //        MessageBox.Show($"Twoje zamówienie zostało zapisane i wysłane na email '{orderSummaryVm.Recipent}'", "Sukces!");
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Log(LogTarget.File, ex);
        //        Logger.Log(LogTarget.EventLog, ex);

        //        AlterOrder(sent:false);
        //        MessageBox.Show(@"Could not send an email - ensure you have internet connection. If so - check Logs.txt file in '\Data' installation foler or in Windows Event Viewer for details.", "Error!");
        //    }
        //}

        //private void AlterOrder(bool sent)
        //{

        //    var orderSummaryVm = this.DataContext as OrderSummaryViewModel;
        //    if (orderSummaryVm is null)
        //    {
        //        Console.WriteLine("Viewmodel is null!");
        //        return;
        //    }


        //    alter table to add isSent info to order
        //    using (var dbContext = new RestaurantDataContext())
        //    {

        //        var currentOrder = dbContext.Orders
        //            .OrderByDescending(o => o.Id)
        //            .Select(order => order).FirstOrDefault();
        //        currentOrder.Sent = sent ? 1 : 0;
        //        currentOrder.Description = SingleOrder.Instance.Order.Description;
        //        dbContext.SaveChanges();
        //    }
        //}
    }
}
