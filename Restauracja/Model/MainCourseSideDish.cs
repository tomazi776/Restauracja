
namespace Restauracja.Model
{
    public class MainCourseSideDish : ProductPOCO, IProduct
    {
        public MainCourseSideDish(string name, decimal prodPrice, string description = "", string remarks = "", ProductType prod_type = ProductType.MainCourseSideDish, int quantity = 1)
    : base(name, prodPrice, description, remarks, prod_type, quantity)
        {

        }
    }
}
