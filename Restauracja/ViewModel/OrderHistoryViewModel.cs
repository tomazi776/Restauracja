using Restauracja.Model;
using Restauracja.Model.Entities;
using Restauracja.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Restauracja.ViewModel
{
    public class OrderHistoryViewModel : BaseViewModel
    {
        public ICommand SelectOrderCommand { get; }

        public List<FullOrderInfo> Orders { get; set; } = new List<FullOrderInfo>();

        private ObservableCollection<OrderItem> orderItems = new ObservableCollection<OrderItem>();
        public ObservableCollection<OrderItem> OrderItems
        {
            get { return orderItems; }
            set
            {
                SetProperty(ref orderItems, value);
            }
        }

        //public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        private bool detailsDisplayed;
        public bool DetailsDisplayed
        {
            get { return detailsDisplayed; }
            set
            {
                SetProperty(ref detailsDisplayed, value);
            }
        }

        private bool itemSelected;
        public bool ItemSelected
        {
            get { return itemSelected; }
            set
            {
                SetProperty(ref itemSelected, value);
                //SetProperty(ref detailsDisplayed, value);   //swieze

                RaisePropertyChanged(nameof(DetailsDisplayed));
            }
        }

        private FullOrderInfo order;
        public FullOrderInfo Order
        {
            get { return order; }
            set
            {
                SetProperty(ref order, value);

                SetProperty(ref itemSelected, true);
                SetProperty(ref detailsDisplayed, false);   //swieze

                RaisePropertyChanged(nameof(ItemSelected));

                OrderItems.Clear();
            }
        }
        public OrderHistoryViewModel()
        {
            GetOrderHistory();

            SelectOrderCommand = new CommandHandler(SelectOrder, () => true);

        }

        private void SelectOrder()
        {
            if (!detailsDisplayed)
            {
                GetOrderItems(Order.CustomerName);
                DetailsDisplayed = true;
            }
        }

        private void GetOrderHistory()
        {
            using (var dbContext = new RestaurantDataContext())
            {
                var query = from ord in dbContext.Orders
                            join cust in dbContext.Customers on ord.Id equals cust.Customer_Id
                            select new FullOrderInfo{CustomerName = cust.CustomerEmail, Date = ord.Date, FinalCost = ord.FinalCost, Description = ord.Description };

                Orders = query.ToList();
                var ddd = "sssss";
            }
        }

        private void GetOrderItems(string customerEmail)
        {
            using (var dbContext = new RestaurantDataContext())
            {
                var query = from item in dbContext.OrderItems
                            where item.Order.Customer.CustomerEmail == customerEmail
                            select item;
                var orderItemsList = query.ToList();

                foreach (var item in orderItemsList)
                {
                    OrderItems.Add(item);
                }
            }
        }
    }
}
