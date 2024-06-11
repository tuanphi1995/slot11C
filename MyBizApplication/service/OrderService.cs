using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyBizApplication.model;
using MySql.Data.MySqlClient;

namespace MyBizApplication.service
{

    public class OrderService : IOrderRepository
    {
        private readonly string connectionString;
        public OrderService(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Order> AllOrders => throw new NotImplementedException();

        public void AddOrder(Order order)
        {
            //Insert dong thoi hai bang order va order details. 
            //dua vao 1 transaction de dam bao cac giao dich dien ra
            // throw new NotImplementedException();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.Transaction = transaction;
                    cmd.CommandText = "INSERT INTO orders(customer_id, order_date, status) values(@customer_id, @order_date, @status)";
                    cmd.Parameters.AddWithValue("@customer_id", order.CustomerId);
                    cmd.Parameters.AddWithValue("@order_date", order.OrderDate);
                    cmd.Parameters.AddWithValue("@status", order.Status);
                    cmd.ExecuteNonQuery();
                    int orderId = (int)cmd.LastInsertedId; //lay ra ID cua bang orders
                    foreach (var detail in order.OrderDetails)
                    {
                        MySqlCommand detailcmd = conn.CreateCommand();
                        detailcmd.Transaction = transaction;
                        detailcmd.CommandText = "INSERT INTO order_details(order_id, product_id, quantity) values(@order_id, @product_id, @quantity)";
                        detailcmd.Parameters.AddWithValue("@order_id", orderId);
                        detailcmd.Parameters.AddWithValue("@product_id", detail.ProductId);
                        detailcmd.Parameters.AddWithValue("@quantity", detail.Quantity);
                        detailcmd.ExecuteNonQuery();
                    }




                    transaction.Commit();
                }

            }
        }

        public List<Order> GetAllOrders()
        {
            // throw new NotImplementedException();
            List<Order> orders = new List<Order>();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM orders";
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Order order = new Order
                        {
                            Id = reader.GetInt32("Id"),
                            CustomerId = reader.GetInt32("Customer_id"),
                            OrderDate = reader.GetDateTime("Order_date"),
                            OrderDetails = new List<OrderDetail>()
                        };
                        orders.Add(order);
                    }
                }
                //sau khi load duo order tu bang orders thi can lay ra chi tiet cua od
                foreach (var order in orders)
            {
                MySqlCommand detailcmd = conn.CreateCommand();
                detailcmd.CommandText = "SELECT * FROM order_details WHERE order_id = @order_id";
                detailcmd.Parameters.AddWithValue("@order_id", order.Id);
                using(MySqlDataReader detailreader = detailcmd.ExecuteReader()){
                    while(detailreader.Read()){
                       OrderDetail detail= new OrderDetail{
                       
                        OrderId = detailreader.GetInt32("Order_id"),
                        ProductId = detailreader.GetInt32("Product_id"),
                        Quantity = detailreader.GetInt32("Quantity")
                       };
                       order.OrderDetails.Add(detail);
                    }
                }
                
                
            }

            }
            return orders;
            
        }

        public Order GetOrderById(int id)
        {
            throw new NotImplementedException();
        }

        public void UpdateOrderStatus(int orderId, string status)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE orders SET status = @status WHERE id = @order_id";
                cmd.Parameters.AddWithValue("@status", status);
                cmd.Parameters.AddWithValue("@order_id", orderId);
                cmd.ExecuteNonQuery();
            }
        }
    }
}