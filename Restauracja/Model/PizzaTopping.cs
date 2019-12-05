using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restauracja.Model
{
    public class PizzaTopping : ProductPOCO
    {
        public PizzaTopping(int id, string name, int price = 2, int quantity = 1, string description = "Pizza topping", string remarks = "")
    : base(id, name, price, quantity, description, remarks)
        {

        }
    }
}
