
namespace Restauracja.Model
{
    public class PizzaTopping : ProductPOCO, IProduct
    {
        public PizzaTopping(string name, decimal prodPrice = 2m, string description = "Pizza topping", string remarks = "", ProductType prod_type = ProductType.PizzaTopping, int quantity = 1)
    : base(name, prodPrice, description, remarks, prod_type, quantity)
        {

        }
    }
}
