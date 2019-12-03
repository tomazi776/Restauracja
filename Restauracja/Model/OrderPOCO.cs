using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restauracja.Model
{
    public class OrderPOCO
    {
        public int Id { get; set; }
        public int FinalCost { get; set; }
        public string Description { get; set; }
        public List<ProductPOCO> Products { get; set; }

        //public readonly int MyProperty;
        //public OrderCategory Category { get; set; }

        public OrderPOCO()
        {
            //Id = id;
            Products = new List<ProductPOCO>();
        }

        public int GetFinalCost(List<ProductPOCO> products)
        {
            var cost = 0;

            foreach (var item in products)
            {
                cost += item.Price;
            }

            return cost;

            //if (Order.)
            //{

            //}
        }

        //public enum OrderCategory
        //{
        //    ToStay,
        //    ToGo
        //}
    }
}
