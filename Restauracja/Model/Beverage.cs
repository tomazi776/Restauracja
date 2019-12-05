using Restauracja.Model;

namespace Restauracja
{
    public class Beverage : ProductPOCO
    {
        public Beverage( string name, int price = 5, string description = "Beverage", int quantity = 1, string remarks = "")
    : base(name, price, quantity, description, remarks)
        {

        }
    }
}
