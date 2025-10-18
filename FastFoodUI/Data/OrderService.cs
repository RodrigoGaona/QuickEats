using System.Collections.Generic;
using FastFoodUI.Data;

namespace FastFoodUI.Data
{
    public class OrderService
    {
        private List<Order> _orders = new List<Order>();

        public void AddOrder(string orderId)
        {
            _orders.Add(new Order { OrderId = orderId, CreationDate = DateTime.Now });
        }

        public List<Order> GetOrders()
        {
            return _orders;
        }
    }
}