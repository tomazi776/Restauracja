using Restauracja.Model;

namespace Restauracja
{
    public class MainCourse : ProductPOCO, IProduct
    {
        public MainCourse(string name, decimal prodPrice, string description = "Main course", string remarks = "", ProductType prod_type = ProductType.MainCourse, int quantity = 1)
    : base(name, prodPrice, description, remarks, prod_type, quantity)
        {

        }
    }
}
