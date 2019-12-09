using System.Collections.Generic;

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
    }
}
