
namespace Restauracja.Model
{
    public class MainCourseSideDish : ProductPOCO
    {
        public MainCourseSideDish(string name, int price, string description = "", string remarks = "", ProductType prod_type = ProductType.MainCourseSideDish, int quantity = 1)
    : base(name, price, description, remarks, prod_type, quantity)
        {

        }
    }
}
