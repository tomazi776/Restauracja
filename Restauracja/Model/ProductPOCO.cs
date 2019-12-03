using Restauracja.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restauracja.Model
{
    public abstract class ProductPOCO : BaseViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //private int price;
        //public int Price
        //{
        //    get { return price; }
        //    set
        //    {
        //        SetProperty(ref price, value);
        //    }
        //}

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

        public ProductPOCO(int id, string name, int price, int quantity = 1, string description = "", string remarks = "")
        {
            Id = id;
            Name = name;
            Price = price;
            Quantity = quantity;
            Description = description;
            Remarks = remarks;
        }

        //public event PropertyChangedEventHandler PropertyChanged;

        public string getName()
        {
            return Name;
        }

        public void AddProducts()
        {

        }

        public override string ToString()
        {
            return Name.ToString();
        }
    }
}
