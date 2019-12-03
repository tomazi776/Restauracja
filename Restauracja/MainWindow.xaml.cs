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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Restauracja
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
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

            //PizzasListView.ItemsSource = ProductsNames;

            //OrderPOCO clientOrder = new OrderPOCO();        // Move to method for creating order

            //clientOrder.Products.Add(newCola);
            //clientOrder.Products.Add(vegetariana);
            //clientOrder.Products.Add(coffe);
        }

        private void DeleteProductClick(object sender, RoutedEventArgs e)
        {
            menuVm.RemoveSelectedProductFromOrder();
            menuVm.GetOrderCost();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PlaceOrder_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
