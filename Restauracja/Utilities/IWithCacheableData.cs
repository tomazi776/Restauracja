using Restauracja.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restauracja.Utilities
{
    public interface IWithCacheableData
    {
        OrderPOCO Order { get; set; }
        ObservableCollection<ProductPOCO> OrderProducts { get; set; }
    }
}
