using System;

namespace Restauracja.Model
{
    public class FullOrderInfo
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public int FinalCost { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public FullOrderInfo() { }

        public override string ToString()
        {
            return CustomerName.ToString();
        }
    }
}
