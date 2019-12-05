using Restauracja.ViewModel;

namespace Restauracja.Model
{
    public abstract class ProductPOCO : BaseViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }

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

        public ProductPOCO(string name, int price, int quantity = 1, string description = "", string remarks = "")
        {
            Name = name;
            Price = price;
            Quantity = quantity;
            Description = description;
            Remarks = remarks;
        }

        //public event PropertyChangedEventHandler PropertyChanged;

        public override string ToString()
        {
            return Name.ToString();
        }
    }
}
