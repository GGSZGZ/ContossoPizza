using ContosoPizza.Models;

namespace ContosoPizza.Data
{
    public interface IOrderRepository
    {
        void AddOrder(Order order);
        Order GetOrder(int id);
        List<Pizza> GetPizzasInOrder(int id);
        void UpdateOrder(Order order);
        void DeleteOrder(int id);

        List<Order> GetAll();
    }
}
