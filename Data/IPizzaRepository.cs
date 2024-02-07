using ContosoPizza.Models;

namespace ContosoPizza.Data
{
    public interface IPizzaRepository
    {
        void AddPizza(Pizza pizza);
        Pizza GetPizza(int id);
        void UpdatePizza(Pizza pizza);
        void DeletePizza(int id);

        List<Pizza> GetAll();
    }
}
