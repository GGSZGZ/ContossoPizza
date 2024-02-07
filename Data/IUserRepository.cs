using ContosoPizza.Models;

namespace ContosoPizza.Data
{
    public interface IUserRepository
    {
        void AddUser(User user);
        User GetUser(int id);
        Order GetUserByOrder(int id);
        void UpdateUser(User user);
        void DeleteUser(int id);

        List<User> GetAll();
    }
}
