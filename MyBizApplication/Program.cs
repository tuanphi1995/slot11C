using MyBizApplication.controller;
using MyBizApplication.model;
using MyBizApplication.service;

namespace MyBizApplication
{
    class Program
    {
        public static void Main(string[] args)
        {
            string connetionString = "server=localhost;database=prodb;user=root;password=phi";
            //model CRUD product
            IProductRepository productServivce = new ProductService(connetionString);
            ProductController productController = new ProductController(productServivce);
            //model order management
            IOrderRepository orderService = new OrderService(connetionString);
            OrderController orderController = new OrderController(orderService);

            while (true)
            {
                Console.WriteLine("My Biz Application");
                Console.WriteLine("1. Add product");
                Console.WriteLine("2. Display all products");
                Console.WriteLine("3. Add Order");
                Console.WriteLine("4. Display all orders");
                Console.WriteLine("5. Update Order Status");
                Console.WriteLine("6. Exit");

                Console.WriteLine("Enter your choice: ");
                int choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        Console.WriteLine("Enter product name: ");
                        string name = Console.ReadLine();
                        Console.WriteLine("Enter price: ");
                        decimal price = Convert.ToDecimal(Console.ReadLine());
                        Console.WriteLine("Enter Decription: ");
                        string description = Console.ReadLine();
                        Product newProduct = new Product { Name = name, Price = price, Description = description };
                        // Product newProduct = new Product();
                        // newProduct.Name = name;
                        // newProduct.Price = price;
                        productController.AddProduct(newProduct);
                        Console.WriteLine("Product added successfully!!!");
                        break;
                    case 2:
                        List<Product> products = productController.GetAllProducts();
                        foreach (var product in products)
                        {
                            Console.WriteLine($"ID: {product.Id}, Name: {product.Name}, Price: {product.Price}, Description: {product.Description}");
                        }

                        break;
                    case 3:
                        Console.WriteLine("Enter customer id: ");
                        int orderCustomerId = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Enter number off product: ");
                        int numberOffProducts = Convert.ToInt32(Console.ReadLine());
                        List<OrderDetail> orderDetails = new List<OrderDetail>();
                        for (int i = 0; i < numberOffProducts; i++)
                        {
                            Console.WriteLine($"Enter product Id for product {i + 1}");
                            int orderIdProduct = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Enter quantity for product");
                            int quantity = Convert.ToInt32(Console.ReadLine());
                            orderDetails.Add(new OrderDetail { ProductId = orderIdProduct, Quantity = quantity });
                        }
                        Order order = new Order
                        {
                            CustomerId = orderCustomerId,
                            OrderDate = DateTime.Now,
                            OrderDetails = orderDetails
                        };
                        orderController.AddOrder(order);
                        Console.WriteLine("Order added successfully!!!");

                        break;

                    case 4:
                    List<Order> orders = orderController.GetAllOrders();
                    foreach (var order1 in orders)
                    {
                        Console.WriteLine($"ID: {order1.Id}, CustomerId: {order1.CustomerId}, OrderDate: {order1.OrderDate}");
                        //list ra cac don hang dang co
                        foreach (var detail in order1.OrderDetails) //list ra cac chi tiet don hang cua moi don hang torng foreach oh trn
                        {
                            Console.WriteLine($"Product id: {detail.ProductId}, Quantity:{detail.Quantity}");

                            
                        }

                    }

                        break;



                    case 5:
                        Console.Write("Enter order id: ");
                        int orderId = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Select new status:");
                        Console.WriteLine("1. Chờ xử lý");
                        Console.WriteLine("2. Đã xử lý");
                        Console.WriteLine("3. Đang vận chuyển");
                        Console.WriteLine("4. Hoàn thành");
                        Console.Write("Enter your choice: ");
                        int statusChoice = Convert.ToInt32(Console.ReadLine());
                        string status = statusChoice switch
                        {
                            1 => "Chờ xử lý",
                            2 => "Đã xử lý",
                            3 => "Đang vận chuyển",
                            4 => "Hoàn thành",
                            _ => throw new ArgumentException("Invalid status choice")
                        };
                        orderController.UpdateOrderStatus(orderId, status);
                        Console.WriteLine("Order status updated successfully!!!");
                        break;
                    
                    case 6:
                    return;
                    default:
                        Console.WriteLine("Invalid choice! pls try again");
                        break;
                }



            }

        }
    }
}