using Restauracja.Model;

namespace Restauracja
{
    public class Beverage : ProductPOCO
    {
        public Beverage( string name, int price = 5, string description = "", string remarks = "", ProductType prod_type = ProductType.Beverage, int quantity = 1)
    : base(name, price, description, remarks, prod_type, quantity)
        {

        }
    }
}
