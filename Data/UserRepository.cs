namespace ContosoPizza.Data;

using ContosoPizza.Models;

public class UserRepository : IUserRepository{
     public List<User> Users;
    public int nextId = 3;
    public UserRepository()
    {
        createUserData();
    }

    public List<User>? GetAll() => Users;

    public User GetUser(int id) => Users.FirstOrDefault(p => p.Id == id);

    public Order? GetUserByOrder(int id)
    {
        var user = Users.FirstOrDefault(o => o.Id == id);

        if (user != null)
        {
            return user.order; // Suponiendo que hay una propiedad "Pizzas" en la clase Order que almacena la lista de pizzas en esa orden.
        }
        else
        {
            return null; // O podrías devolver una lista vacía o manejar de otra manera según tu lógica de negocio.
        }
    }

    public void AddUser(User user)
    {
        user.Id = nextId++;
        Users.Add(user);
    }

    public void DeleteUser(int id)
    {
        var user = GetUser(id);
        if(user is null)
            return;

        Users.Remove(user);
    }

    public void UpdateUser(User user)
    {
        var index = Users.FindIndex(p => p.Id == user.Id);
        if(index == -1)
            return;

        Users[index] = user;
    }
    public void createUserData(){
        Users = new List<User>
        {
            new User{Id = 1, Name="Jeffrey", Surname="Epstein", Direction="C/Alfonso IV Meridio, 14, 4ºG", 
                order = new Order{Id= 1, Price = 14.85M, orderDate = DateTime.Now.ToString(),estimatedDelivery = DateTime.Now.AddHours(1).ToString("HH:mm"), 
                    pizzas = new List<Pizza>{
                        new Pizza { Id = 1, Name = "Classic Italian", Veggie = false,
                            listIngr = new List<Ingredients>{
                                new Ingredients{Name = "Tomato Sauce", Calories=29M, Origin= "Spain", IsGlutenFree=false},
                                new Ingredients{Name = "Basil", Calories=22M, Origin= "Italy", IsGlutenFree=false},
                                new Ingredients{Name = "Cheese", Calories=402M, Origin= "France", IsGlutenFree=false},
                                new Ingredients{Name = "Pepperoni", Calories=494M, Origin= "Italy", IsGlutenFree=false},
                                new Ingredients{Name = "Flour", Calories=620M, Origin= "Italy", IsGlutenFree=false},
                            }},
                        new Pizza { Id = 2, Name = "Veggie", Veggie = true ,
                            listIngr = new List<Ingredients>{
                                new Ingredients{Name = "Tomato Sauce", Calories=29M, Origin= "Spain", IsGlutenFree=false},
                                new Ingredients{Name = "Basil", Calories=22M, Origin= "Italy", IsGlutenFree=false},
                                new Ingredients{Name = "Cheese", Calories=402M, Origin= "France", IsGlutenFree=true},
                                new Ingredients{Name = "Flour", Calories=358M, Origin= "Italy", IsGlutenFree=true},
                        }}
                    }
                }
            },
            new User{Id = 2, Name = "Stephen", Surname = "Hawkins", Direction="C/Santa Madona, 13, 33ºE",
                order = new Order{Id = 2, Price = 10.50M, orderDate = DateTime.Now.ToString(),estimatedDelivery = DateTime.Now.AddHours(1).ToString("HH:mm"), 
                    pizzas = new List<Pizza>{
                        new Pizza { Id = 3, Name = "Barbacue", Veggie = false,
                            listIngr = new List<Ingredients>{
                                new Ingredients{Name = "Tomato Sauce", Calories=29M, Origin= "Spain", IsGlutenFree=false},
                                new Ingredients{Name = "Barbacue Sauce", Calories=172M, Origin= "Germany", IsGlutenFree=false},
                                new Ingredients{Name = "Cheese", Calories=402M, Origin= "France", IsGlutenFree=false},
                                new Ingredients{Name = "Bacon", Calories=541M, Origin= "France", IsGlutenFree=false},
                                new Ingredients{Name = "Jam", Calories=145M, Origin= "Spain", IsGlutenFree=false},
                                new Ingredients{Name = "Minced Meat", Calories=241M, Origin= "Spain", IsGlutenFree=false},
                                new Ingredients{Name = "Flour", Calories=620M, Origin= "Italy", IsGlutenFree=false},
                        }}
                    }
                }
            }
        };
        
    }
}