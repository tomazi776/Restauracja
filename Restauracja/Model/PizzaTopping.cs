
namespace Restauracja.Model
{
    public class PizzaTopping : ProductPOCO
    {
        public PizzaTopping(string name, int price = 2, string description = "Pizza topping", string remarks = "", ProductType prod_type = ProductType.PizzaTopping, int quantity = 1)
    : base(name, price, description, remarks, prod_type, quantity)
        {

        }
    }
}
