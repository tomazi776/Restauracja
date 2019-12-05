using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restauracja.Model
{
    public class Soup : ProductPOCO
    {
        public Soup(int id, string name, int price, string description = "Soup", int quantity = 1, string remarks = "")
    : base(id, name, price, quantity, description, remarks)
        {

        }
    }
}
