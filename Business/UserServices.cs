namespace ContosoPizza.Business;

using ContosoPizza.Models;
using ContosoPizza.Data;
using System.Collections.Generic;

public class UserService : IUserService
{

    private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }
    public void AddUser(User user)
    {
        _repository.AddUser(user);
    }

    public void DeleteUser(int id)
    {
        _repository.DeleteUser(id);
    }

    public List<User> GetAll()
    {
        var users = _repository.GetAll();
             
            
        return users;
    }

    public User GetUser(int id)
    {
        var user = _repository.GetUser(id);
             if (String.IsNullOrEmpty(user.Name)) {
                throw new KeyNotFoundException("Pizza not found.");
             }
            
            return user;
    }

    public void UpdateUser(User user)
    {
        _repository.UpdateUser(user);
    }

    public Order GetUserByOrder(int id){
        var user = _repository.GetUserByOrder(id);
        return user;
    }
}