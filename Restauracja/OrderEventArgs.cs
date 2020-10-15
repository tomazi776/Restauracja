using Restauracja.Model;
using System;

namespace Restauracja
{
    public class OrderEventArgs : EventArgs
    {
        public OrderPOCO Order { get; set; }
    }
}
