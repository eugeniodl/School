using SchoolAPI.Models;

namespace SchoolAPI.Repository.IRepository
{
    public interface IUserRepository
    {
        bool IsUser(string username, string password);
        Task<User> GetUser(string username, string password);
    }
}
