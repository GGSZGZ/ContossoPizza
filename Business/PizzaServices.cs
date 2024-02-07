namespace ContosoPizza.Business;

using ContosoPizza.Models;
using ContosoPizza.Data;

public class PizzaService : IPizzaService
{

    private readonly IPizzaRepository _repository;

        public PizzaService(IPizzaRepository repository)
        {
            _repository = repository;
        }

   public Pizza GetPizza(int Id)
        {
           // var account = _repository.GetAccount(accountNumber) ?? throw new KeyNotFoundException("Account not found.");
             var pizza = _repository.GetPizza(Id);
             if (String.IsNullOrEmpty(pizza.Name)) {
                throw new KeyNotFoundException("Pizza not found.");
             }
            
            return pizza;
        }

        public List<Pizza> GetAll()
        {
           // var account = _repository.GetAccount(accountNumber) ?? throw new KeyNotFoundException("Account not found.");
             var pizzas = _repository.GetAll();
             
            
            return pizzas;
        }



    public void AddPizza(Pizza pizza)
    {
       _repository.AddPizza(pizza);
    }

    public void DeletePizza(int id)
    {
        _repository.DeletePizza(id);
    }

    public void UpdatePizza(Pizza pizza)
    {
       _repository.UpdatePizza(pizza);
    }

   
}