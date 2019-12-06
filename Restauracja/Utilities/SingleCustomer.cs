using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restauracja.Utilities
{
    public class SingleCustomer
    {
        public int Id { get; set; }
        public string Email { get; set; }

        private static SingleCustomer instance = new SingleCustomer();

        private SingleCustomer() 
        {
            if (instance != null)
            {
                throw new Exception("Use GetInstance() method to get the single instance of this class.");
            }
        }

        public static SingleCustomer GetInstance()
        {
            if (instance == null)
            {
                instance = new SingleCustomer();
            }
            return instance;
        }
    }
}
