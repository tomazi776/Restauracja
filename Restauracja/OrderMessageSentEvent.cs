using Prism.Events;
using Restauracja.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restauracja
{
    public class OrderMessageSentEvent : PubSubEvent<OrderPOCO>
    {
    }
}
