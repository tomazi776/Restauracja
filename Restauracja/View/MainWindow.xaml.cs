using Restauracja.ViewModel;
using System.Windows;

namespace Restauracja.View
{
    public partial class MainWindow : Window
    {
        MenuViewModel menuVm;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = menuVm = new MenuViewModel();
            menuVm.OrderPlaced += MenuVm_OrderPlaced;
        }

        private void MenuVm_OrderPlaced(object sender, System.EventArgs e)
        {
            Window orderWindow = new OrderSummaryWindow();
            orderWindow.DataContext = menuVm.OrderSummaryViewModel;
            orderWindow.Show();
            this.Close();
        }
    }
}
