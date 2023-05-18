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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().HasData(
                new Student()
                {
                    StudentId = 1,
                    StudentName = "Marcela Padilla"
                },
                new Student()
                {
                    StudentId = 2,
                    StudentName = "Leonardo Amador"
                },
                new Student()
                {
                    StudentId = 3,
                    StudentName = "Karla Reyes"
                });
        }
    }
}
