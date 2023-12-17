using ProductStore.Model;

namespace ProductStore.Service.IService
{
    public interface IUser
    {
        Task<User> GetUserByEmail(string Email);
        Task<string> AddUser(User newUser);
    }
}
