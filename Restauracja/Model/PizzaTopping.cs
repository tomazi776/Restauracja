
namespace Restauracja.Model
{
    public class PizzaTopping : ProductPOCO
    {
        public PizzaTopping(string name, int price = 2, int quantity = 1, string description = "Pizza topping", string remarks = "")
    : base(name, price, quantity, description, remarks)
        {

        }
    }
}
