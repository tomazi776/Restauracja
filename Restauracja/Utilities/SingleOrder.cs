using Restauracja.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restauracja.Utilities
{
    public class SingleOrder
    {
        public  ObservableCollection<ProductPOCO> OrderProducts { get; set; }
        public OrderPOCO Order { get; set; }
        private SingleOrder() { }

        private static SingleOrder instance = null;
        private static readonly object padlock = new object();
        public static SingleOrder Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new SingleOrder();
                    }
                    return instance;
                }
            }
        }
    }
}
