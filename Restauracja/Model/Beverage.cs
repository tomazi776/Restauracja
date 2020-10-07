using Restauracja.Model;

namespace Restauracja
{
    public class Beverage : ProductPOCO, IProduct
    {
        public Beverage( string name, decimal prodPrice = 5m, string description = "", string remarks = "", ProductType prod_type = ProductType.Beverage, int quantity = 1)
    : base(name, prodPrice, description, remarks, prod_type, quantity)
        {

        }
    }
}
