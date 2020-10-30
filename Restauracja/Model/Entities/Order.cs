using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restauracja.Model.Entities
{
    [Table("Cust_Order")]
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public decimal FinalCost { get; set; }
        public string Description { get; set; }
        public Customer Customer { get; set; }

        public DateTime Date { get; set; }
        public int Sent { get; set; }

        public virtual ICollection<OrderItem> OrderItem { get; set; }   // Navigation property to OrderItem

        public Order(Customer customer, decimal finalCost, string description, DateTime date, int sent = 0)
        {
            FinalCost = finalCost;
            Description = description;
            Customer = customer;
            Sent = sent;
            Date = date;
            this.OrderItem = new HashSet<OrderItem>();
        }

        public Order() { }

    }
}
