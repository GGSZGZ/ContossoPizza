using ContosoPizza.Models;

namespace ContosoPizza.Business
{
    public interface IOrderService
    {
        void AddOrder(Order order);
        List<Order> GetAll();
        Order GetOrder(int id);
        List<Pizza> GetPizzasInOrder(int id);
        void UpdateOrder(Order order);
        void DeleteOrder(int id);

        
    }
}