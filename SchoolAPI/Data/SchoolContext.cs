using Microsoft.EntityFrameworkCore;
using SchoolAPI.Models;

namespace SchoolAPI.Data
{
    public class SchoolContext : DbContext
    {
        public SchoolContext(DbContextOptions<SchoolContext> options) : base(options)
        {

        }

        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuider)
        {
            modelBuider.Entity<Student>().HasData(
                new Student()
                {
                    StudentId = 1,
                    StudentName = "Norman López"
                },
                new Student()
                {
                    StudentId = 2,
                    StudentName = "Martha Pérez"
                },
                new Student()
                {
                    StudentId = 3,
                    StudentName = "Luis Padilla"
                });
        }

    }
}
