using ContosoPizza.Models;

namespace ContosoPizza.Business
{
    public interface IUserService
    {
        void AddUser(User user);
        User GetUser(int id);
        Order GetUserByOrder(int id);
        void UpdateUser(User user);
        void DeleteUser(int id);

        List<User> GetAll();
    }
}
