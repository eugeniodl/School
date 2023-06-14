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
        public DbSet<Grade> Grades { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Grade>().HasData(
                new Grade()
                {
                    Id = 1,
                    GradeName = "Primero",
                    Section = 'A'
                },
                new Grade()
                {
                    Id = 2,
                    GradeName = "Primero",
                    Section = 'B'
                });
            modelBuilder.Entity<Student>().HasData(
                new Student()
                {
                    StudentId = 1,
                    StudentName = "Moisés Alemán",
                    DateOfBirth = new DateTime(2017, 9, 4),
                    GradeId = 1
                },
                new Student()
                {
                    StudentId = 2,
                    StudentName = "Marcia Escobar",
                    DateOfBirth = new DateTime(2018, 12, 20),
                    GradeId = 2
                },
                new Student()
                {
                    StudentId = 3,
                    StudentName = "Kevin Dávila",
                    DateOfBirth = new DateTime(2019, 7, 13),
                    GradeId = 1
                });
            modelBuilder.Entity<User>().HasData(
                new User()
                {
                    Id = 1,
                    UserName = "administrator",
                    Password = "1234.",
                    Role = "Administrator"
                },
                new User()
                {
                    Id = 2,
                    UserName = "teacher",
                    Password = "1234.",
                    Role = "Teacher"
                });
        }


    }
}
