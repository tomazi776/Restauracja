

namespace Restauracja.Model
{
    public class Pizza : ProductPOCO
    {
        public Pizza( string name, int price, string description = "", string remarks = "", ProductType prod_type = ProductType.Pizza, int quantity = 1)
            : base(name, price, description, remarks, prod_type, quantity)
        {

        }

        private void AddToppings(params string[] toppings)
        {

        }
    }
}
