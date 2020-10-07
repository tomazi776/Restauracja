using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Restauracja.Model.Entities
{
    [Table("OrderItem")]
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public string Remarks { get; set; }
        public int OrderId { get; set; }  // Foreign Key to Order
        public Order Order { get; set; }


        public OrderItem(string name, decimal price, int quantity = 1, string description = "", string remarks = "")
        {
            Name = name;
            Price = price;
            Quantity = quantity;
            Remarks = remarks;
            Description = description;
        }

        public OrderItem()
        {

        }
    }
}
