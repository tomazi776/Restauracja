﻿using Prism.Events;
using Restauracja.Model;
using Restauracja.Services;
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
        public OrderSummaryWindow()
        {
            InitializeComponent();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Window main = new MainWindow();
            main.Show();
            this.Close();
        }

        private void OrderHistoryButton_Click(object sender, RoutedEventArgs e)
        {
            Window historyWindow = new OrderHistoryWindow();
            historyWindow.Show();
            this.Close();
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

            orderSummaryVm.Sender = "urban776@gmail.com"; // for testing


            //Add event for when the order is saved in DB
            orderSummaryVm.MakeOrder();

            //TODO: Send mail only if it's properly saved in the DB
            //if (orderSummaryVm.OrderSaved)
            //{
            //    //do the rest
            //}

            orderSummaryVm.Recipent = "tomaszurbaniak776@gmail.com"; // for testing

            EmailService emailService = new EmailService(orderSummaryVm.Sender, orderSummaryVm.Recipent, orderSummaryVm.Order);
            var emailBodyInHtml = emailService.GenerateEmail();

            MailMessage email = new MailMessage(orderSummaryVm.Sender, orderSummaryVm.Recipent); // instead EmailBody
            email.IsBodyHtml = true;
            email.Body = emailBodyInHtml;
            email.Subject = EmailService.EMAIL_SUBJECT;
      
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
                Logger.Log(LogTarget.File, ex);
                Logger.Log(LogTarget.EventLog, ex);

                // Handle cases of exceptions to inform user of the problem
                MessageBox.Show(@"Could not send an email - ensure you have internet connection. If so - check Logs.txt file in '\Data' installation foler or in Windows Event Viewer for details.", "Error!");
            }
        }
    }
}
