namespace SchoolAPI.Repository.IRepository
{
    public interface IUserRepository
    {
        bool IsUser(string username, string pass);
    }
}
