using Restauracja.View;
using Restauracja.ViewModel;
using System.Windows;

namespace Restauracja
{
    public partial class MainWindow : Window
    {
        MenuViewModel menuVm;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = menuVm = new MenuViewModel();
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            menuVm.AddSelectedProductToOrder();
            menuVm.GetOrderCost();
        }

        private void DeleteProductClick(object sender, RoutedEventArgs e)
        {
            menuVm.RemoveSelectedProductFromOrder();
            menuVm.GetOrderCost();
        }

        private void PlaceOrder_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(MenuViewModel.CONFIRMATION_PROMPT_CONTENT, MenuViewModel.CONFIRMATION_PROMPT_HEADER, MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    Window orderWindow = new OrderSummaryWindow(menuVm.OrderProducts, menuVm.OrderRemarks);
                    orderWindow.Show();
                    this.Close();
                    MessageBox.Show(MenuViewModel.WELCOME_MESSAGE_CONTENT, MenuViewModel.WELCOME_MESSAGE_HEADER);
                    break;
            }
        }
    }
}
