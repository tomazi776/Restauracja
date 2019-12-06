using Restauracja.Model;
using Restauracja.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    pocoProduct = new Pizza(product.Name, product.Price, product.Remarks);      // Can add decorator to add additions and toppings
                    break;

                case ProductType.PizzaTopping:
                    pocoProduct = new PizzaTopping(product.Name);
                    break;

                case ProductType.MainCourse:
                    pocoProduct = new MainCourse(product.Name, product.Price);
                    break;

                case ProductType.MainCourseSideDish:
                    pocoProduct = new MainCourseSideDish(product.Name, product.Price);
                    break;

                case ProductType.Beverage:
                    pocoProduct = new Beverage(product.Name);
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
