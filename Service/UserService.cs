using Microsoft.EntityFrameworkCore;
using ProductStore.Data;
using ProductStore.Model;
using ProductStore.Service.IService;

namespace ProductStore.Service
{
    public class UserService : IUser
    {
        private readonly ProductStoreContext _context;

        public UserService(ProductStoreContext context)
        {
            _context = context;
        }
        public async Task<string> AddUser(User newUser)
        {
            var usersCount = _context.Users.Count();

            // First user created is an Admin
            if (usersCount == 0)
            {
                newUser.Role = "Admin";
            }

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            return "User Created successfully";
        }

        public async Task<User> GetUserByEmail(string Email)
        {
           var existingUser = await (from user in _context.Users
                               where user.Email == Email
                               select user)
                               .FirstOrDefaultAsync();
            return existingUser;
        }
    }
}
