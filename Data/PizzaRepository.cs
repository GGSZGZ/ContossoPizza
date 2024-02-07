namespace ContosoPizza.Data;

using ContosoPizza.Models;

public class PizzaRepository : IPizzaRepository{

     public List<Pizza> Pizzas;
    public int nextId = 4;
    public PizzaRepository()
    {
        createPizzaData();
    }
   
    public void createPizzaData(){
        Pizzas = new List<Pizza>
        {
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
                new Ingredients{Name = "Flour", Calories=358M, Origin= "Italy", IsGlutenFree=true}
             }},
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
        };
        
    }
    
     public List<Pizza>? GetAll() => Pizzas;

    public Pizza GetPizza(int id) => Pizzas.FirstOrDefault(p => p.Id == id);

    public void AddPizza(Pizza pizza)
    {
        pizza.Id = nextId++;
        Pizzas.Add(pizza);
    }

    public void DeletePizza(int id)
    {
        var pizza = GetPizza(id);
        if(pizza is null)
            return;

        Pizzas.Remove(pizza);
    }

    public void UpdatePizza(Pizza pizza)
    {
        var index = Pizzas.FindIndex(p => p.Id == pizza.Id);
        if(index == -1)
            return;

        Pizzas[index] = pizza;
    }

    
}