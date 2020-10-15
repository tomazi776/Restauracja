
namespace Restauracja.Model
{
    public class Pizza : ProductPOCO, IProduct
    {
        public Pizza( string name, decimal price,
            string description = "", string remarks = "", ProductType prod_type = ProductType.Pizza, int quantity = 1)
            : base(name, price, description, remarks, prod_type, quantity)
        {

        }
    }
}
