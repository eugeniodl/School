using SchoolAPI.Models;

namespace SchoolAPI.Repository.IRepository
{
    public interface IStudentRepository : IRepository<Student>
    {
        Task<Student> Update(Student entity);  
    }
}
