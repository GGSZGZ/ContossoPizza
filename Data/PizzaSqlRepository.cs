using System.Text.Json;
using System.Data.SqlClient;
using ContosoPizza.Models;
using System.Data;

namespace ContosoPizza.Data
{
    public class PizzaSqlRepository : IPizzaRepository
    {

        private readonly string _connectionString;

        public PizzaSqlRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        
        public void AddPizza(Pizza pizza)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Insertar la pizza en la tabla Pizza
                string insertPizzaQuery = "INSERT INTO Pizza (Id, Name, Veggie) VALUES (@Id, @Name, @Veggie)";
                using (SqlCommand command = new SqlCommand(insertPizzaQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", pizza.Id);
                    command.Parameters.AddWithValue("@Name", pizza.Name);
                    command.Parameters.AddWithValue("@Veggie", pizza.Veggie);
                    command.ExecuteNonQuery();
                }

                foreach (Ingredients ingredient in pizza.listIngr)
                {
                    // Verificar si el ingrediente ya existe en la tabla Ingredients
                    string checkIngredientQuery = "SELECT COUNT(*) FROM Ingredients WHERE Name = @Name";
                    using (SqlCommand command = new SqlCommand(checkIngredientQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Name", ingredient.Name);
                        int count = (int)command.ExecuteScalar();

                        if (count == 0)
                        {
                            // Insertar el ingrediente en la tabla Ingredients solo si no existe
                            string insertIngredientQuery = "INSERT INTO Ingredients (Name, Calories, Origin, IsGlutenFree) VALUES (@Name, @Calories, @Origin, @IsGlutenFree)";
                            using (SqlCommand insertCommand = new SqlCommand(insertIngredientQuery, connection))
                            {
                                insertCommand.Parameters.AddWithValue("@Name", ingredient.Name);
                                insertCommand.Parameters.AddWithValue("@Calories", ingredient.Calories);
                                insertCommand.Parameters.AddWithValue("@Origin", ingredient.Origin);
                                insertCommand.Parameters.AddWithValue("@IsGlutenFree", ingredient.IsGlutenFree);
                                insertCommand.ExecuteNonQuery();
                            }
                        }

                        // Insertar la relación en la tabla PizzaIngredients
                        string insertPizzaIngredientsQuery = "INSERT INTO PizzaIngredients (Pizza_Id, Ingredient_Name) VALUES (@Pizza_Id, @Ingredient_Name)";
                        using (SqlCommand insertCommand = new SqlCommand(insertPizzaIngredientsQuery, connection))
                        {
                            insertCommand.Parameters.AddWithValue("@Pizza_Id", pizza.Id);
                            insertCommand.Parameters.AddWithValue("@Ingredient_Name", ingredient.Name);
                            insertCommand.ExecuteNonQuery();
                        }
                    }
                }
            }
        }



        public List<Pizza> GetAll()
        {
            var pizzas = new List<Pizza>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var sqlString = @"
                    SELECT P.Id, P.Name, P.Veggie, I.Name AS IngredientName, I.Calories, I.Origin, I.IsGlutenFree
                    FROM Pizza P
                    LEFT JOIN PizzaIngredients PI ON P.Id = PI.Pizza_Id
                    LEFT JOIN Ingredients I ON PI.Ingredient_Name = I.Name";
                    
                var command = new SqlCommand(sqlString, connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Verificar si ya existe la pizza en la lista
                        var pizzaId = Convert.ToInt32(reader["Id"]);
                        var existingPizza = pizzas.FirstOrDefault(p => p.Id == pizzaId);

                        // Si la pizza no existe, crear una nueva instancia
                        if (existingPizza == null)
                        {
                            existingPizza = new Pizza
                            {
                                Id = pizzaId,
                                Name = reader["Name"].ToString(),
                                Veggie = Convert.ToBoolean(reader["Veggie"]),
                                listIngr = new List<Ingredients>()  // Inicializar la lista de ingredientes
                            };

                            pizzas.Add(existingPizza);
                        }

                        // Añadir el ingrediente a la lista de ingredientes de la pizza
                        if (reader["IngredientName"] != DBNull.Value)
                        {
                            var ingredient = new Ingredients
                            {
                                Name = reader["IngredientName"].ToString(),
                                Calories = Convert.ToDecimal(reader["Calories"]),
                                Origin = reader["Origin"].ToString(),
                                IsGlutenFree = Convert.ToBoolean(reader["IsGlutenFree"])
                            };

                            existingPizza.listIngr.Add(ingredient);
                        }
                    }
                }
            }

            return pizzas;
        }


       public Pizza GetPizza(int id)
        {
            var pizza = new Pizza();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var sqlString = @"
                    SELECT P.Id, P.Name, P.Veggie, I.Name AS IngredientName, I.Calories, I.Origin, I.IsGlutenFree
                    FROM Pizza P
                    LEFT JOIN PizzaIngredients PI ON P.Id = PI.Pizza_Id
                    LEFT JOIN Ingredients I ON PI.Ingredient_Name = I.Name
                    WHERE P.Id="+id;

                var command = new SqlCommand(sqlString, connection);


                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Utiliza el parámetro id en lugar de pizzaId
                        if (pizza.Id == 0) // Verificar si la pizza ya ha sido creada
                        {
                            pizza.Id = id;
                            pizza.Name = reader["Name"].ToString();
                            pizza.Veggie = Convert.ToBoolean(reader["Veggie"]);
                            pizza.listIngr = new List<Ingredients>();
                        }

                        if (reader["IngredientName"] != DBNull.Value)
                        {
                            var ingredient = new Ingredients
                            {
                                Name = reader["IngredientName"].ToString(),
                                Calories = Convert.ToDecimal(reader["Calories"]),
                                Origin = reader["Origin"].ToString(),
                                IsGlutenFree = Convert.ToBoolean(reader["IsGlutenFree"])
                            };

                            pizza.listIngr.Add(ingredient);
                        }
                    }
                }
            }

            return pizza;
        }
        public void UpdatePizza(Pizza pizza)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Actualizar la pizza en la tabla Pizza
                string updatePizzaQuery = "UPDATE Pizza SET Name = @Name, Veggie = @Veggie WHERE Id = @Id";
                using (SqlCommand command = new SqlCommand(updatePizzaQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", pizza.Id);
                    command.Parameters.AddWithValue("@Name", pizza.Name);
                    command.Parameters.AddWithValue("@Veggie", pizza.Veggie);
                    command.ExecuteNonQuery();
                }

                // Eliminar los ingredientes asociados a la pizza en la tabla PizzaIngredients
                string deletePizzaIngredientsQuery = "DELETE FROM PizzaIngredients WHERE Pizza_Id = @Pizza_Id";
                using (SqlCommand command = new SqlCommand(deletePizzaIngredientsQuery, connection))
                {
                    command.Parameters.AddWithValue("@Pizza_Id", pizza.Id);
                    command.ExecuteNonQuery();
                }

                foreach (Ingredients ingredient in pizza.listIngr)
                {
                    // Verificar si el ingrediente ya existe en la tabla Ingredients
                    string checkIngredientQuery = "SELECT COUNT(*) FROM Ingredients WHERE Name = @Name";
                    using (SqlCommand command = new SqlCommand(checkIngredientQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Name", ingredient.Name);
                        int count = (int)command.ExecuteScalar();

                        if (count == 0)
                        {
                            // Insertar el ingrediente en la tabla Ingredients solo si no existe
                            string insertIngredientQuery = "INSERT INTO Ingredients (Name, Calories, Origin, IsGlutenFree) VALUES (@Name, @Calories, @Origin, @IsGlutenFree)";
                            using (SqlCommand insertCommand = new SqlCommand(insertIngredientQuery, connection))
                            {
                                insertCommand.Parameters.AddWithValue("@Name", ingredient.Name);
                                insertCommand.Parameters.AddWithValue("@Calories", ingredient.Calories);
                                insertCommand.Parameters.AddWithValue("@Origin", ingredient.Origin);
                                insertCommand.Parameters.AddWithValue("@IsGlutenFree", ingredient.IsGlutenFree);
                                insertCommand.ExecuteNonQuery();
                            }
                        }

                        // Insertar la relación en la tabla PizzaIngredients
                        string insertPizzaIngredientsQuery = "INSERT INTO PizzaIngredients (Pizza_Id, Ingredient_Name) VALUES (@Pizza_Id, @Ingredient_Name)";
                        using (SqlCommand insertCommand = new SqlCommand(insertPizzaIngredientsQuery, connection))
                        {
                            insertCommand.Parameters.AddWithValue("@Pizza_Id", pizza.Id);
                            insertCommand.Parameters.AddWithValue("@Ingredient_Name", ingredient.Name);
                            insertCommand.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        public void DeletePizza(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Eliminar las relaciones de ingredientes asociadas a la pizza en la tabla PizzaIngredients
                string deletePizzaIngredientsQuery = "DELETE FROM PizzaIngredients WHERE Pizza_Id = "+id;
                using (SqlCommand command = new SqlCommand(deletePizzaIngredientsQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                // Eliminar la pizza de la tabla Pizza
                string deletePizzaQuery = "DELETE FROM Pizza WHERE Id = "+id;
                using (SqlCommand command = new SqlCommand(deletePizzaQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }


    }
}
