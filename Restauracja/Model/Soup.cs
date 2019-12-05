
namespace Restauracja.Model
{
    public class Soup : ProductPOCO
    {
        public Soup(string name, int price, string description = "Soup", int quantity = 1, string remarks = "")
    : base(name, price, quantity, description, remarks)
        {

        }
    }
}
