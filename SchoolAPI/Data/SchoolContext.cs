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

        protected override void OnModelCreating(ModelBuilder modelBuider)
        {
            modelBuider.Entity<Grade>().HasData(
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
            modelBuider.Entity<Student>().HasData(
                new Student()
                {
                    StudentId = 1,
                    StudentName = "Norman López",
                    DateOfBirth = new DateTime(2017, 5, 23),
                    GradeId = 1
                },
                new Student()
                {
                    StudentId = 2,
                    StudentName = "Martha Pérez",
                    DateOfBirth = new DateTime(2016, 3, 17),
                    GradeId = 2
                },
                new Student()
                {
                    StudentId = 3,
                    StudentName = "Luis Padilla",
                    DateOfBirth = new DateTime(2015, 4, 19),
                    GradeId = 1
                });
        }

    }
}
