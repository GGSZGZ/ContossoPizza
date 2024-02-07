
using ContosoPizza.Models;

namespace ContosoPizza.Business
{
    public interface IPizzaService
    {
        
        public Pizza GetPizza(int id);

        public List<Pizza> GetAll();

        void AddPizza(Pizza pizza);
        void UpdatePizza(Pizza pizza);
        void DeletePizza(int id);
    }
}
