using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public string Remarks { get; set; }


        public Product(string name, int price, string description = "", string remarks = "")
        {
            Name = name;
            Price = price;
            Description = description;
            Remarks = remarks;
        }

        public Product() { }
    }
}
