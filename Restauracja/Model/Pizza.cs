

namespace Restauracja.Model
{
    public class Pizza : ProductPOCO
    {
        public Pizza( string name, int price, string remarks = "", string description = "Pizza", int quantity = 1)
            : base(name, price, quantity, description, remarks)
        {

        }

        private void AddToppings(params string[] toppings)
        {

        }
    }
}
