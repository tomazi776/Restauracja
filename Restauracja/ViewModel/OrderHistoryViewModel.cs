﻿using Restauracja.Model;
using Restauracja.Model.Entities;
using Restauracja.Utilities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Restauracja.ViewModel
{
    public class OrderHistoryViewModel : BaseViewModel
    {
        public ICommand SelectOrderDetailsCommand { get; }
        public List<FullOrderInfo> Orders { get; set; } = new List<FullOrderInfo>();

        private ObservableCollection<OrderItem> orderItems = new ObservableCollection<OrderItem>();
        public ObservableCollection<OrderItem> OrderItems
        {
            get => orderItems;
            set
            {
                SetProperty(ref orderItems, value);
            }
        }

        private bool detailsDisplayed;
        public bool DetailsDisplayed
        {
            get => detailsDisplayed;
            set
            {
                SetProperty(ref detailsDisplayed, value);
            }
        }

        private bool itemSelected;
        public bool ItemSelected
        {
            get => itemSelected;
            set
            {
                SetProperty(ref itemSelected, value);
                RaisePropertyChanged(nameof(DetailsDisplayed));
            }
        }

        private FullOrderInfo order;
        public FullOrderInfo Order
        {
            get => order;
            set
            {
                SetProperty(ref order, value);
                SetProperty(ref itemSelected, true);
                SetProperty(ref detailsDisplayed, false);
                RaisePropertyChanged(nameof(ItemSelected));
                OrderItems.Clear();
            }
        }


        public OrderHistoryViewModel()
        {
            GetAllOrders();
            SelectOrderDetailsCommand = new CommandHandler(SelectOrderDetails, () => true);
        }

        private void SelectOrderDetails()
        {
            if (!detailsDisplayed)
            {
                GetOrderItems(Order.CustomerName);
                DetailsDisplayed = true;
            }
        }

        private void GetAllOrders()
        {
            using (var dbContext = new RestaurantDataContext())
            {
                var query = from order in dbContext.Orders
                            select new FullOrderInfo { Id = order.Id, CustomerName = order.Customer.CustomerEmail, Date = order.Date, FinalCost = order.FinalCost, Description = order.Description, Sent = order.Sent == 1? "Tak" : "Nie" };
                Orders = query.ToList();
            }
        }

        private void GetOrderItems(string sender)
        {
            using (var dbContext = new RestaurantDataContext())
            {
                var query = from item in dbContext.OrderItems
                            where item.Order.Customer.CustomerEmail == sender
                            where item.OrderId == Order.Id
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
