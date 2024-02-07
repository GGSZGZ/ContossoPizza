namespace ContosoPizza.Data;

using ContosoPizza.Models;
public class OrderRepository : IOrderRepository{

    public List<Order> Orders;
    public int nextId = 3;

    public OrderRepository(){
        createOrderData();
    }
    public void createOrderData(){

        Orders = new List<Order>{
            new Order{Id= 1, Price = 14.85M, orderDate = DateTime.Now.ToString(),estimatedDelivery = DateTime.Now.AddHours(1).ToString("HH:mm"), 
                pizzas = new List<Pizza>{
                    new Pizza { Id = 1, Name = "Classic Italian", Veggie = false,
                        listIngr = new List<Ingredients>{
                            new Ingredients{Name = "Tomato Sauce", Calories=29M, Origin= "Spain", IsGlutenFree=false},
                            new Ingredients{Name = "Basil", Calories=22M, Origin= "Italy", IsGlutenFree=false},
                            new Ingredients{Name = "Cheese", Calories=402M, Origin= "France", IsGlutenFree=false},
                            new Ingredients{Name = "Pepperoni", Calories=494M, Origin= "Italy", IsGlutenFree=false},
                            new Ingredients{Name = "Flour", Calories=620M, Origin= "Italy", IsGlutenFree=false}
                        }},
                    new Pizza { Id = 2, Name = "Veggie", Veggie = true ,
                        listIngr = new List<Ingredients>{
                            new Ingredients{Name = "Tomato Sauce", Calories=29M, Origin= "Spain", IsGlutenFree=false},
                            new Ingredients{Name = "Basil", Calories=22M, Origin= "Italy", IsGlutenFree=false},
                            new Ingredients{Name = "Cheese", Calories=402M, Origin= "France", IsGlutenFree=true},
                            new Ingredients{Name = "Flour", Calories=358M, Origin= "Italy", IsGlutenFree=true}
                    }}
                }
            },
            new Order{Id = 2, Price = 10.50M, orderDate = DateTime.Now.ToString(),estimatedDelivery = DateTime.Now.AddHours(1).ToString("HH:mm"), 
                pizzas = new List<Pizza>{
                    new Pizza { Id = 3, Name = "Barbacue", Veggie = false, 
                        listIngr = new List<Ingredients>{
                            new Ingredients{Name = "Tomato Sauce", Calories=29M, Origin= "Spain", IsGlutenFree=false},
                            new Ingredients{Name = "Barbacue Sauce", Calories=172M, Origin= "Germany", IsGlutenFree=false},
                            new Ingredients{Name = "Cheese", Calories=402M, Origin= "France", IsGlutenFree=false},
                            new Ingredients{Name = "Bacon", Calories=541M, Origin= "France", IsGlutenFree=false},
                            new Ingredients{Name = "Jam", Calories=145M, Origin= "Spain", IsGlutenFree=false},
                            new Ingredients{Name = "Minced Meat", Calories=241M, Origin= "Spain", IsGlutenFree=false},
                            new Ingredients{Name = "Flour", Calories=620M, Origin= "Italy", IsGlutenFree=false}
                    }}
                }
            }
        };
        
    }


    public List<Order> GetAll() => Orders;

    public Order? GetOrder(int id) => Orders.FirstOrDefault(p => p.Id == id); 
    public List<Pizza>? GetPizzasInOrder(int id)
    {
        var order = Orders.FirstOrDefault(o => o.Id == id);

        if (order != null)
        {
            return order.pizzas; // Suponiendo que hay una propiedad "Pizzas" en la clase Order que almacena la lista de pizzas en esa orden.
        }
        else
        {
            return null; // O podrías devolver una lista vacía o manejar de otra manera según tu lógica de negocio.
        }
    }


    public void AddOrder(Order order)
    {
        order.Id = nextId++;
        Orders.Add(order);
    }

    public void DeleteOrder(int id)
    {
        var order = GetOrder(id);
        if(order is null)
            return;

        Orders.Remove(order);
    }

    public void UpdateOrder(Order order)
    {
        var index = Orders.FindIndex(p => p.Id == order.Id);
        if(index == -1)
            return;

        Orders[index] = order;
    }

}