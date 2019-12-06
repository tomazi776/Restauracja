using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Restauracja.Model.Entities
{
    [Table("Customer")]
    public class Customer
    {
        [Key]
        public int Customer_Id { get; set; }
        public string CustomerEmail { get; set; }

        public virtual ICollection<Order> Order { get; set; }       // Navigation property to Order
        public Customer(string email)
        {
            CustomerEmail = email;

            this.Order = new HashSet<Order>();
        }
    }
}
