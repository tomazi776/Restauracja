using Restauracja.ViewModel;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for OrderHistoryWindow.xaml
    /// </summary>
    public partial class OrderHistoryWindow : Window
    {
        OrderHistoryViewModel orderHistoryVm;
        public OrderHistoryWindow()
        {
            InitializeComponent();

            DataContext = orderHistoryVm = new OrderHistoryViewModel();

        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            OrderSummaryWindow orderSummaryWindow = new OrderSummaryWindow();
            orderSummaryWindow.Show();
            this.Close();
        }
    }
}
