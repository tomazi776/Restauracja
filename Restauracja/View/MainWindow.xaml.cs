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
            InitializeComponent();

            DataContext = menuVm = new MenuViewModel(ea);

            menuVm.OrderPlaced += MenuVm_OrderPlaced;
        }

        //private void OrderSummaryViewModel_BackBtnPressed(object sender, System.EventArgs e)
        //{
        //    menuVm.GetCachedOrderOnBackBtnPressed();
        //    //add event to viewmodel which will notify MenuViewmodel and handle getting data from Order Singleton

        //}

        private void MenuVm_OrderPlaced(object sender, System.EventArgs e)
        {

            //TODO: Move this to code behind as it violates MVVM (no view info in VM!)
            Window orderWindow = new OrderSummaryWindow();
            orderWindow.DataContext = menuVm.OrderSummaryViewModel;
            orderWindow.Show();

            this.Close();
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
