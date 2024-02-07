using System.Text.Json;
using System.Data.SqlClient;
using ContosoPizza.Models;
using System.Globalization;

using System.Data;

namespace ContosoPizza.Data{
    public class UserSqlRepository : IUserRepository
    {

         private readonly string _connectionString;

        public UserSqlRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        

      public void AddUser(User user)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Crear la orden asociada al usuario
                var orderSql = new OrderSqlRepository(_connectionString);
                orderSql.AddOrder(user.order);
                // Insertar detalles del usuario en la tabla Users
                var insertUserQuery = @"
                    INSERT INTO Users (Id,Name, Surname, Direction, Order_Id)
                    VALUES (@Id,@Name, @Surname, @Direction, @OrderId)";

                var command = new SqlCommand(insertUserQuery, connection);
                command.Parameters.AddWithValue("@Id", user.Id);
                command.Parameters.AddWithValue("@Name", user.Name);
                command.Parameters.AddWithValue("@Surname", user.Surname);
                command.Parameters.AddWithValue("@Direction", user.Direction);
                command.Parameters.AddWithValue("@OrderId", user.order.Id);

                command.ExecuteNonQuery();
            }
        }

      public void DeleteUser(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Obtener el Order_Id asociado al usuario
                var getOrderQuery = "SELECT Order_Id FROM Users WHERE Id = @UserId";
                var getOrderCommand = new SqlCommand(getOrderQuery, connection);
                getOrderCommand.Parameters.AddWithValue("@UserId", id);

                object orderIdObject = getOrderCommand.ExecuteScalar();

                if (orderIdObject != null)
                {
                    int orderId = Convert.ToInt32(orderIdObject);

                    // Eliminar la relación entre el usuario y la orden en la tabla Users
                    var deleteUserOrderRelationQuery = "UPDATE Users SET Order_Id = NULL WHERE Order_Id = @OrderId";
                    var deleteUserOrderRelationCommand = new SqlCommand(deleteUserOrderRelationQuery, connection);
                    deleteUserOrderRelationCommand.Parameters.AddWithValue("@OrderId", orderId);
                    deleteUserOrderRelationCommand.ExecuteNonQuery();

                    // Eliminar la orden y sus detalles a través del método existente
                    var orderRepo = new OrderSqlRepository(_connectionString);
                    orderRepo.DeleteOrder(orderId);

                    // Finalmente, eliminar el usuario en la tabla Users
                    var deleteUserQuery = "DELETE FROM Users WHERE Id = @UserId";
                    var deleteUserCommand = new SqlCommand(deleteUserQuery, connection);
                    deleteUserCommand.Parameters.AddWithValue("@UserId", id);
                    deleteUserCommand.ExecuteNonQuery();
                }
            }
        }





        public List<User> GetAll()
        {
            var users = new List<User>();

            using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            var sqlString = @"
                SELECT U.Id AS UserId, U.Name, U.Surname, U.Direction,
                    O.Id AS OrderId, O.Price, O.OrderDate, O.EstimatedDelivery,
                    P.Id AS PizzaId, P.Name AS PizzaName, P.Veggie
                FROM Users U
                LEFT JOIN Orders O ON U.Order_Id = O.Id
                LEFT JOIN OrderPizzaDetails OPD ON O.Id = OPD.Order_Id
                LEFT JOIN Pizza P ON OPD.Pizza_Id = P.Id
                WHERE U.Order_Id IS NULL OR O.Id IS NOT NULL";


            var command = new SqlCommand(sqlString, connection);

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var userId = Convert.ToInt32(reader["UserId"]);
                    var existingUser = users.FirstOrDefault(u => u.Id == userId);
                    int? orderId = reader["OrderId"] as int?;
                    var orderRepo = new OrderSqlRepository(_connectionString);

                    if (existingUser == null)
                    {
                        existingUser = new User
                        {
                            Id = userId,
                            Name = reader["Name"].ToString(),
                            Surname = reader["Surname"].ToString(),
                            Direction = reader["Direction"].ToString(),
                            order = orderId.HasValue ? orderRepo.GetOrder(orderId.Value) : null!
                        };
                        users.Add(existingUser);
                    }
                }
            }
        }
        return users;
    }


       public User GetUser(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var sqlString = @"
                    SELECT U.Id AS UserId, U.Name, U.Surname, U.Direction,
                        O.Id AS OrderId, O.Price, O.OrderDate, O.EstimatedDelivery,
                        P.Id AS PizzaId, P.Name AS PizzaName, P.Veggie
                    FROM Users U
                    LEFT JOIN Orders O ON U.Order_Id = O.Id
                    LEFT JOIN OrderPizzaDetails OPD ON O.Id = OPD.Order_Id
                    LEFT JOIN Pizza P ON OPD.Pizza_Id = P.Id
                    WHERE U.Id = "+id;

                var command = new SqlCommand(sqlString, connection);

                using (var reader = command.ExecuteReader())
                {
                    User foundUser = null!;

                    while (reader.Read())
                    {
                        int? orderId = reader["OrderId"] as int?;
                        var orderRepo = new OrderSqlRepository(_connectionString);

                        foundUser = new User
                        {
                            Id = id,
                            Name = reader["Name"].ToString(),
                            Surname = reader["Surname"].ToString(),
                            Direction = reader["Direction"].ToString(),
                            order = orderId.HasValue ? orderRepo.GetOrder(orderId.Value) : null!
                        };
                    }

                    return foundUser;
                }
            }
        }



        public Order GetUserByOrder(int userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var sqlString = @"
                SELECT O.Id AS OrderId, O.Price, O.OrderDate, O.EstimatedDelivery,
                    P.Id AS PizzaId, P.Name AS PizzaName, P.Veggie
                FROM Orders O
                LEFT JOIN OrderPizzaDetails OPD ON O.Id = OPD.Order_Id
                LEFT JOIN Pizza P ON OPD.Pizza_Id = P.Id
                LEFT JOIN Users U ON U.Order_Id = O.Id
                WHERE U.Id = @userId";

                var command = new SqlCommand(sqlString, connection);
                command.Parameters.AddWithValue("@userId", userId);
                

                Order userOrder = null!;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {   
                        int orderId = Convert.ToInt32(reader["OrderId"]);
                        var orderRepo = new OrderSqlRepository(_connectionString);
                        userOrder = orderRepo.GetOrder(orderId);
                    }
                }

                return userOrder;
            }
        }


       public void UpdateUser(User user)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Actualizar detalles del usuario en la tabla Users
                var updateUserQuery = @"
                    UPDATE Users
                    SET Name = @Name, Surname = @Surname, Direction = @Direction
                    WHERE Id = @UserId";

                var command = new SqlCommand(updateUserQuery, connection);
                command.Parameters.AddWithValue("@UserId", user.Id);
                command.Parameters.AddWithValue("@Name", user.Name);
                command.Parameters.AddWithValue("@Surname", user.Surname);
                command.Parameters.AddWithValue("@Direction", user.Direction);
                command.ExecuteNonQuery();

                var orderRepo = new OrderSqlRepository(_connectionString);
                orderRepo.UpdateOrder(user.order);
            }
        }

    }
}