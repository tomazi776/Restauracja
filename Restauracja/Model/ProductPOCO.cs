using Restauracja.ViewModel;

namespace Restauracja.Model
{
    public abstract class ProductPOCO : BaseViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public ProductType ProductType { get; set; }

        private int quantity;
        public int Quantity
        {
            get { return quantity; }
            set
            {
                SetProperty(ref quantity, value);
            }
        }
        public string Description { get; set; }
        public string Remarks { get; set; }

        public ProductPOCO(string name, int price, string description = "", string remarks = "", ProductType prod_type = ProductType.MainCourse , int quantity = 1)
        {
            Name = name;
            Price = price;
            ProductType = prod_type;
            Quantity = quantity;
            Description = description;
            Remarks = remarks;
        }

        public override string ToString()
        {
            return Name.ToString();
        }
    }
}
