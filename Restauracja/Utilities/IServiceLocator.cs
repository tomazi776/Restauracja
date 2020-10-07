using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restauracja.Utilities
{
    public interface IServiceLocator
    {
        T GetService<T>();
    }
}
