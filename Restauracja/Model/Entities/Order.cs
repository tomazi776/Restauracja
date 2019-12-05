﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restauracja.Model.Entities
{
    [Table("Cust_Order")]
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public int FinalCost { get; set; }
        public string Description { get; set; }
        //public string CustomerName { get; set; }  // Maybe necessary to add
        public Customer Customer { get; set; }

        public virtual ICollection<OrderItem> OrderItem { get; set; }   // Navigation property to OrderItem

        public Order( int finalCost, string description)
        {
            FinalCost = finalCost;
            Description = description;

            this.OrderItem = new HashSet<OrderItem>();
        }
    }
}
