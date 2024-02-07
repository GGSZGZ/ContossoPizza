using System.Text.Json;
using System.Data.SqlClient;
using ContosoPizza.Models;
using System.Globalization;
using System.Data;
using System.Runtime.CompilerServices;

namespace ContosoPizza.Data
{
    public class OrderSqlRepository : IOrderRepository
    {

        private readonly string _connectionString;

        public OrderSqlRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        
        public void AddOrder(Order order)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Insertar la pizza en la tabla Orders
                string insertPizzaQuery = "INSERT INTO Orders (Id, Price, OrderDate, EstimatedDelivery) VALUES (@Id, @Price, @OrderDate, @EstimatedDelivery)";
                using (SqlCommand command = new SqlCommand(insertPizzaQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", order.Id);
                    command.Parameters.AddWithValue("@Price", order.Price);
                    command.Parameters.AddWithValue("@OrderDate", DateTime.ParseExact(order.orderDate, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture));
                    command.Parameters.AddWithValue("@EstimatedDelivery", DateTime.ParseExact(order.estimatedDelivery, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture));
                    command.ExecuteNonQuery();
                }

                foreach (Pizza pizza in order.pizzas)
                {
                    // Verificar si el ingrediente ya existe en la tabla Ingredients
                    string checkPizzaQuery = "SELECT COUNT(*) FROM Pizza WHERE Id = @Id";
                    using (SqlCommand command = new SqlCommand(checkPizzaQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Id", pizza.Id);
                        int count = (int)command.ExecuteScalar();

                        if (count == 0)
                        {
                            var pizzaRepo = new PizzaSqlRepository(_connectionString);
                            pizzaRepo.AddPizza(pizza);
                        }

                        // Insertar la relación en la tabla PizzaIngredients
                        string insertPizzaIngredientsQuery = "INSERT INTO OrderPizzaDetails (Order_Id, Pizza_Id) VALUES (@Order_Id, @Pizza_Id)";
                        using (SqlCommand insertCommand = new SqlCommand(insertPizzaIngredientsQuery, connection))
                        {
                            insertCommand.Parameters.AddWithValue("@Order_Id", order.Id);
                            insertCommand.Parameters.AddWithValue("@Pizza_Id", pizza.Id);
                            insertCommand.ExecuteNonQuery();
                        }
                    }
                }
            }
        }



        public List<Order> GetAll()
        {
            var orders = new List<Order>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var sqlString = @"
                    SELECT O.Id, O.Price, O.OrderDate, O.EstimatedDelivery, P.Id AS PizzaId, P.Name, P.Veggie
                    FROM Orders O
                    LEFT JOIN OrderPizzaDetails OPD ON O.Id = OPD.Order_Id
                    LEFT JOIN Pizza P ON OPD.Pizza_Id = P.Id";
                    
                var command = new SqlCommand(sqlString, connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Verificar si ya existe la entrega en la lista
                        var orderId = Convert.ToInt32(reader["Id"]);
                        var existingOrder = orders.FirstOrDefault(o => o.Id == orderId);

                        // Si la entrega no existe, crear una nueva instancia
                        if (existingOrder == null)
                        {
                            existingOrder = new Order
                            {
                                Id = orderId,
                                Price = Convert.ToDecimal(reader["Price"]),
                                orderDate = reader["OrderDate"].ToString(),
                                estimatedDelivery = reader["EstimatedDelivery"].ToString(),
                                pizzas = new List<Pizza>()  // Inicializar la lista de pizzas
                            };

                            orders.Add(existingOrder);
                        }

                        // Añadir la pizza a la lista de pizzas de la entrega
                        if (reader["PizzaId"] != DBNull.Value)
                        {
                            int pizzaId = Convert.ToInt32(reader["PizzaId"]);
                            var pizzaRepo = new PizzaSqlRepository(_connectionString);
                            var pizza = pizzaRepo.GetPizza(pizzaId);
                            existingOrder.pizzas.Add(pizza);
                        }
                        }
                }
            }

            return orders;
        }


       public Order GetOrder(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var sqlString = @"
                    SELECT O.Id, O.Price, O.OrderDate, O.EstimatedDelivery, P.Id AS PizzaId, P.Name, P.Veggie
                    FROM Orders O
                    LEFT JOIN OrderPizzaDetails OPD ON O.Id = OPD.Order_Id
                    LEFT JOIN Pizza P ON OPD.Pizza_Id = P.Id
                    WHERE O.Id = "+id;

                var command = new SqlCommand(sqlString, connection);

                using (var reader = command.ExecuteReader())
                {
                    Order foundOrder = null!;

                    while (reader.Read())
                    {
                        if (foundOrder == null)
                        {
                            foundOrder = new Order
                            {
                                Id = id,
                                Price = Convert.ToDecimal(reader["Price"]),
                                orderDate = reader["OrderDate"].ToString(),
                                estimatedDelivery = reader["EstimatedDelivery"].ToString(),
                                pizzas = new List<Pizza>()
                            };
                        }

                        if (reader["PizzaId"] != DBNull.Value)
                        {
                            int pizzaId = Convert.ToInt32(reader["PizzaId"]);
                            var pizzaRepo = new PizzaSqlRepository(_connectionString);
                            var pizza = pizzaRepo.GetPizza(pizzaId);
                            foundOrder.pizzas.Add(pizza);
                        }
                    }

                    return foundOrder;
                }
            }
        }

        public void UpdateOrder(Order order)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Update order details in the Orders table
                var updateOrderQuery = @"
                    UPDATE Orders
                    SET Price = @Price, OrderDate = @OrderDate, EstimatedDelivery = @EstimatedDelivery
                    WHERE Id = @OrderId";

                var command = new SqlCommand(updateOrderQuery, connection);
                command.Parameters.AddWithValue("@OrderId", order.Id);
                command.Parameters.AddWithValue("@Price", order.Price);
                command.Parameters.AddWithValue("@OrderDate", DateTime.ParseExact(order.orderDate, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture));
                command.Parameters.AddWithValue("@EstimatedDelivery", DateTime.ParseExact(order.estimatedDelivery, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture));

                command.ExecuteNonQuery();

                // Delete existing pizza details for the order in OrderPizzaDetails table
                var deletePizzaDetailsQuery = "DELETE FROM OrderPizzaDetails WHERE Order_Id = @OrderId";
                command = new SqlCommand(deletePizzaDetailsQuery, connection);
                command.Parameters.AddWithValue("@OrderId", order.Id);
                command.ExecuteNonQuery();

                // Insert updated pizza details for the order in OrderPizzaDetails table
                foreach (var pizza in order.pizzas)
                {
                    var insertPizzaDetailsQuery = "INSERT INTO OrderPizzaDetails (Order_Id, Pizza_Id) VALUES (@OrderId, @PizzaId)";
                    command = new SqlCommand(insertPizzaDetailsQuery, connection);
                    command.Parameters.AddWithValue("@OrderId", order.Id);
                    command.Parameters.AddWithValue("@PizzaId", pizza.Id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteOrder(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Eliminar las relaciones de ingredientes asociadas a la pizza en la tabla PizzaIngredients
                string deletePizzaIngredientsQuery = "DELETE FROM OrderPizzaDetails WHERE Order_Id = "+id;
                using (SqlCommand command = new SqlCommand(deletePizzaIngredientsQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                
                string deleteOrderUserQuery = "UPDATE Users SET Order_Id = NULL WHERE Order_Id = " + id;

                using (SqlCommand command = new SqlCommand(deleteOrderUserQuery, connection))
                {
                    command.ExecuteNonQuery();
                }




                // Eliminar la pizza de la tabla Pizza
                string deletePizzaQuery = "DELETE FROM Orders WHERE Id = "+id;
                using (SqlCommand command = new SqlCommand(deletePizzaQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
        
        public List<Pizza> GetPizzasInOrder(int id){
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var sqlString = @"
                    SELECT P.Id AS PizzaId, P.Name, P.Veggie
                    FROM Pizza P
                    LEFT JOIN OrderPizzaDetails OPD ON P.Id = OPD.Pizza_Id
                    LEFT JOIN Orders O ON OPD.Order_Id = O.Id
                    WHERE O.Id = "+id;

                var command = new SqlCommand(sqlString, connection);

                var pizzas = new List<Pizza>();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int pizzaId = Convert.ToInt32(reader["PizzaId"]);
                        var pizzaRepo = new PizzaSqlRepository(_connectionString);
                        var pizza = pizzaRepo.GetPizza(pizzaId);
                        pizzas.Add(pizza);
                    }

                    return pizzas;
                }
            }
        }
    }
}