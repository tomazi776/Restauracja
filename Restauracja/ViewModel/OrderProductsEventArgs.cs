using Restauracja.Model;
using System;
using System.Collections.ObjectModel;

namespace Restauracja.ViewModel
{
    public partial class MenuViewModel
    {
        public class OrderProductsEventArgs : EventArgs
        {
            public ObservableCollection<ProductPOCO> OrderProducts { get; set; }
        }
    }
}
