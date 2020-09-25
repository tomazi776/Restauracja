using Prism.Events;
using Restauracja.View;
using Restauracja.ViewModel;
using System.Windows;

namespace Restauracja.View
{
    public partial class MainWindow : Window
    {
        MenuViewModel menuVm;
        IEventAggregator ea = new EventAggregator();
        public MainWindow()
        {
            //ea = eventAggregator;

            InitializeComponent();

            DataContext = menuVm = new MenuViewModel(ea);
        }

        //private void AddProduct_Click(object sender, RoutedEventArgs e)
        //{
        //    menuVm.AddSelectedProductToOrder();
        //    menuVm.GetSummaryCost();
        //}

        //private void DeleteProductClick(object sender, RoutedEventArgs e)
        //{
        //    menuVm.RemoveSelectedProductFromOrder();
        //    menuVm.GetSummaryCost();
        //}

        //private void PlaceOrder_Click(object sender, RoutedEventArgs e)
        //{
        //    MessageBoxResult result = MessageBox.Show(MenuViewModel.CONFIRMATION_PROMPT_CONTENT, MenuViewModel.CONFIRMATION_PROMPT_HEADER, MessageBoxButton.YesNo);
        //    switch (result)
        //    {
        //        case MessageBoxResult.Yes:
        //            Window orderWindow = new OrderSummaryWindow(menuVm.OrderProducts, menuVm.OrderRemarks);
        //            orderWindow.Show();
        //            this.Close();
        //            MessageBox.Show(MenuViewModel.WELCOME_MESSAGE_CONTENT, MenuViewModel.WELCOME_MESSAGE_HEADER);
        //            break;
        //    }
        //}
    }
}
