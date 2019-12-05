using Restauracja.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restauracja
{
    public class MainCourse : ProductPOCO
    {
        public MainCourse(int id, string name, int price, string description = "Main course", int quantity = 1, string remarks = "")
    : base(id, name, price, quantity, description, remarks)
        {

        }
    }
}
