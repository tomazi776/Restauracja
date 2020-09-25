using Restauracja.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restauracja.Utilities
{
    public class ServiceLocator : IServiceLocator
    {
        private IDictionary<object, object> services;

        internal ServiceLocator()
        {
            services = new Dictionary<object, object>();

            this.services.Add(typeof(IWindowService), new WindowService());
        }

        public T GetService<T>()
        {
            try
            {
                return (T)services[typeof(T)];
            }
            catch (Exception)
            {
                throw new NotImplementedException("Service not available");
            }
        }
    }
}
