using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Restauracja.Model
{
    public class OrderPOCO : INotifyPropertyChanged
    {
        public int Id { get; set; }
        private decimal finalCost;
        public decimal FinalCost
        {
            get { return finalCost; }
            set
            {
                if (finalCost != value)
                {
                    finalCost = value;
                    RaisePropertyChanged(nameof(FinalCost));
                }
            }
        }
        public string Description { get; set; }
        public List<ProductPOCO> Products { get; set; }
        public OrderPOCO()
        {
            Products = new List<ProductPOCO>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        //protected bool SetProperty<T>(ref T backingField, T value, [CallerMemberName] string property = null)
        //{
        //    if (EqualityComparer<T>.Default.Equals(backingField, value))
        //        return false;

        //    backingField = value;
        //    RaisePropertyChanged(property);
        //    return true;
        //}

        public decimal GetOrderCost<ProductPOCO>(ObservableCollection<Model.ProductPOCO> orderSummaryProducts)
        {
            decimal orderCost = 0;
            foreach (var prod in orderSummaryProducts)
            {
                for (int i = 0; i < prod.Quantity; i++)
                {
                    orderCost += prod.Price;
                }
            }
            return orderCost;
        }

        //public int GetOrderCost<ProductPOCO>(ObservableCollection<Model.ProductPOCO> orderSummaryProducts)
        //{
        //    int orderCost = 0;
        //    foreach (var prod in orderSummaryProducts)
        //    {
        //        for (int i = 0; i < prod.Quantity; i++)
        //        {
        //            orderCost += prod.Price;
        //        }
        //    }
        //    return orderCost;
        //}
    }
}
