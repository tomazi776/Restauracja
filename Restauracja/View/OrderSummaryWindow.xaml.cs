using Restauracja.Model;
using Restauracja.Utilities;
using Restauracja.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        public OrderSummaryWindow(ObservableCollection<ProductPOCO> prod = null, string orderRemarks = "")
        {
            InitializeComponent();
            DataContext = summaryOrderVm = new OrderSummaryViewModel(prod, orderRemarks);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }

        private void OrderHistoryButton_Click(object sender, RoutedEventArgs e)
        {
            Window orderHistoryWindow = new OrderHistoryWindow();
            orderHistoryWindow.Show();
            this.Close();
        }

        private void RecipentTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            summaryOrderVm.SendEnabled = EmailValidator.IsValidEmailAddress(RecipentTextBox.Text);
        }
    }
}
