using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyBizApplication.model;
using MyBizApplication.service;

namespace MyBizApplication.controller
{
    public class OrderController
    {
        private IOrderRepository orderService;
        public OrderController(IOrderRepository orderService){
            this.orderService = orderService;
        }
        public void AddOrder(Order order){
            orderService.AddOrder(order);
        }
        public List<Order> GetAllOrders(){
            return orderService.GetAllOrders();
        }
        public Order GetOrderById(int id){
            return orderService.GetOrderById(id);
        }
        public void UpdateOrderStatus(int orderId, string status)
        {
            orderService.UpdateOrderStatus(orderId, status);
        }

        
    }
}