
namespace Restauracja.Model
{
    public class Soup : ProductPOCO, IProduct
    {
        public Soup(string name, int price, ProductType prod_type = ProductType.Soup, string description = "", int quantity = 1, string remarks = "")
    : base(name, price, description, remarks, prod_type, quantity)
        {

        }
    }
}
