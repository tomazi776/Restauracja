using Restauracja.ViewModel;
using System.Windows;


namespace Restauracja.View
{
    public partial class OrderHistoryWindow : Window
    {
        public OrderHistoryWindow()
        {
            InitializeComponent();
            DataContext = new OrderHistoryViewModel();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            OrderSummaryWindow orderSummaryWindow = new OrderSummaryWindow();
            orderSummaryWindow.Show();
            this.Close();
        }
    }
}
