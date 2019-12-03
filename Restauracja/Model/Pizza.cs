using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restauracja.Model
{
    public class Pizza : ProductPOCO
    {
        public Pizza(int id, string name, int price, int quantity, string description, string remarks)
            : base(id, name, price, quantity, description, remarks)
        {

        }

        private void AddToppings(params string[] toppings)
        {

        }
    }
}
