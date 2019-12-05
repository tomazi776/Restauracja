using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restauracja.Model
{
    public class MainCourseSideDish : ProductPOCO
    {
        public MainCourseSideDish(int id, string name, int price, string description = "Main course side dish", int quantity = 1, string remarks = "")
    : base(id, name, price, quantity, description, remarks)
        {

        }
    }
}
