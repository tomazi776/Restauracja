using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restauracja.Model.Entities
{
    [Table("Product")]
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public string Remarks { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public Product(int id, string name, int price, int quantity = 1, string description = "", string remarks = "")
        {
            Id = id;
            Name = name;
            Price = price;
            Quantity = quantity;
            Description = description;
            Remarks = remarks;

            this.Orders = new HashSet<Order>();
        }

        public Product()
        {
            this.Orders = new HashSet<Order>();

        }
    }
}
