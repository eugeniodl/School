using Microsoft.EntityFrameworkCore;
using SchoolAPI.Data;
using SchoolAPI.Models;
using SchoolAPI.Repository.IRepository;

namespace SchoolAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly SchoolContext _db;

        public UserRepository(SchoolContext db)
        {
            _db = db;
        }

        public async Task<User> GetUser(string username, string password)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.UserName == username
            && u.Password == password);
        }

        public bool IsUser(string username, string password)
        {
            var users = _db.Users.ToList();
            return users.Where(u => u.UserName == username
            && u.Password == password).Count() > 0;
        }
    }
}
