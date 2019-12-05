
namespace Restauracja.Model
{
    public class MainCourseSideDish : ProductPOCO
    {
        public MainCourseSideDish(string name, int price, string description = "Main course side dish", int quantity = 1, string remarks = "")
    : base(name, price, quantity, description, remarks)
        {

        }
    }
}
