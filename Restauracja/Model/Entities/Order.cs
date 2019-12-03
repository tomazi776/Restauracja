using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restauracja.Model.Entities
{
    [Table("Customer_Order")]
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public int FinalCost { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public Order(int finalCost, string description)
        {
            FinalCost = finalCost;
            Description = description;
            this.Products = new HashSet<Product>();
        }
    }
}
