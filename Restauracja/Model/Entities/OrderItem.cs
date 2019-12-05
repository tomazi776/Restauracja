using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restauracja.Model.Entities
{
    [Table("OrderItem")]
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }
        //public int FinalCost { get; set; }

        public string Name { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }


        public int Quantity { get; set; }

        public string Remarks { get; set; }


        public int OrderId { get; set; }  // Foreign Key to Order
        public Order Order { get; set; }


        public OrderItem(string name, int price, int quantity = 1, string description = "", string remarks = "")
        {
            //Id = id;
            Name = name;
            Price = price;
            Quantity = quantity;
            Remarks = remarks;
            //FinalCost = finalCost;
            Description = description;
            //this.Products = new HashSet<Product>();
        }
    }
}
