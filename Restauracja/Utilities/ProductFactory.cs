using Restauracja.Model;
using Restauracja.Model.Entities;

namespace Restauracja.Utilities
{
    public class ProductFactory
    {
        public IProduct CreatePOCOProduct(Product product)
        {
            IProduct pocoProduct = null;
            switch (product.ProductType)
            {
                case ProductType.Pizza:
                    pocoProduct = new Pizza(product.Name, product.Price, product.Remarks);
                    break;

                case ProductType.PizzaTopping:
                    pocoProduct = new PizzaTopping(product.Name, product.Price);
                    break;

                case ProductType.MainCourse:
                    pocoProduct = new MainCourse(product.Name, product.Price);
                    break;

                case ProductType.MainCourseSideDish:
                    pocoProduct = new MainCourseSideDish(product.Name, product.Price);
                    break;

                case ProductType.Beverage:
                    pocoProduct = new Beverage(product.Name, product.Price);
                    break;

                case ProductType.Soup:
                    pocoProduct = new Soup(product.Name, product.Price);
                    break;

                default:
                    break;
            }
            return pocoProduct;
        }
    }
}
