using System;

namespace Restauracja.Utilities
{
    public class SingleCustomer
    {
        public string Email { get; set; }

        private static SingleCustomer instance = null;
        private static readonly object padlock = new object();

        private SingleCustomer() { }

        public static SingleCustomer Instance
        {
            get
            {
                lock (padlock)
                {
                    //if (instance is null)
                    //{
                    //    instance = new SingleCustomer();
                    //}
                    //return instance;
                    return instance ?? (instance = new SingleCustomer());
                }
            }
        }
    }
}
