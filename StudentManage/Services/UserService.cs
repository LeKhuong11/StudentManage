using StudentManage.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StudentManage.Models;
using BCrypt;


namespace StudentManage.Services
{
    public class UserService
    {
        private readonly StudentManageContext _context;

        public UserService(StudentManageContext context)
        {
            _context = context;
        }

        public async Task<S_User> RegisterUser(S_User user)
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            _context.S_Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<S_User> ValidateUser(string email, string password)
        {
            var user = await _context.S_Users.SingleOrDefaultAsync(u => u.Email == email);
            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return user;
            }
            return null;
        }
    }
}
