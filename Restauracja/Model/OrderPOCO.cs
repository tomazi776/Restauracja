using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Restauracja.Model
{
    public class OrderPOCO
    {
        public int Id { get; set; }
        public int FinalCost { get; set; }
        public string Description { get; set; }
        public List<ProductPOCO> Products { get; set; }
        public OrderPOCO()
        {
            Products = new List<ProductPOCO>();
        }

        public int GetOrderCost<ProductPOCO>(ObservableCollection<Model.ProductPOCO> orderSummaryProducts)
        {
            int orderCost = 0;
            foreach (var prod in orderSummaryProducts)
            {
                for (int i = 0; i < prod.Quantity; i++)
                {
                    orderCost += prod.Price;
                }
            }
            return orderCost;
        }
    }
}
