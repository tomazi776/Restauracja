using Restauracja.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restauracja
{
    public class Beverage : ProductPOCO
    {
        public Beverage(int id, string name, int price = 5, string description = "Beverage", int quantity = 1, string remarks = "")
    : base(id, name, price, quantity, description, remarks)
        {

        }
    }
}
