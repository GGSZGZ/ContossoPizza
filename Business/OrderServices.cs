namespace ContosoPizza.Business;

using ContosoPizza.Models;
using ContosoPizza.Data;
using System.Collections.Generic;

public class OrderService : IOrderService
{

    private readonly IOrderRepository _repository;

    public OrderService(IOrderRepository repository)
        {
            _repository = repository;
        }

    public void AddOrder(Order order)
    {
        _repository.AddOrder(order);
    }

    public void DeleteOrder(int id)
    {
        _repository.DeleteOrder(id);
    }

    public List<Order> GetAll()
    {
        var order = _repository.GetAll();
             
        return order;
    }

    public Order GetOrder(int id)
    {
         var order = _repository.GetOrder(id);
             
            
            return order;
    }

    public List<Pizza> GetPizzasInOrder(int id){
        var order = _repository.GetPizzasInOrder(id);
        return order;
    }

    public void UpdateOrder(Order order)
    {
        _repository.UpdateOrder(order);
    }
}