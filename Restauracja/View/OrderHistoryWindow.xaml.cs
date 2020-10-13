using Prism.Events;
using Restauracja.ViewModel;
using System.Windows;


namespace Restauracja.View
{
    public partial class OrderHistoryWindow : Window
    {
        IEventAggregator ea = new EventAggregator();

        public OrderHistoryWindow()
        {
            InitializeComponent();
            DataContext = new OrderHistoryViewModel();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            OrderSummaryWindow orderSummaryWindow = new OrderSummaryWindow();
            orderSummaryWindow.DataContext = new OrderSummaryViewModel(ea);
            orderSummaryWindow.Show();
            this.Close();
        }
    }
}
