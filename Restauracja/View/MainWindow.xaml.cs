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

        private void MenuVm_OrderPlaced(object sender, System.EventArgs e)
        {

            //TODO: Move this to code behind as it violates MVVM (no view info in VM!)
            Window orderWindow = new OrderSummaryWindow();
            orderWindow.DataContext = menuVm.OrderSummaryViewModel;
            orderWindow.Show();
            this.Close();
        }
    }
}
