using Restauracja.Model;

namespace Restauracja
{
    public class MainCourse : ProductPOCO
    {
        public MainCourse(string name, int price, string description = "Main course", int quantity = 1, string remarks = "")
    : base(name, price, quantity, description, remarks)
        {

        }
    }
}
