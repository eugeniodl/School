using SchoolAPI.Models;

namespace SchoolAPI.Repository.IRepository
{
    public interface IUserRepository
    {
        bool IsUser(string username, string pass);
        Task<User> GetUser(string username, string pass);
    }
}
