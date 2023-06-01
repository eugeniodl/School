using System.ComponentModel.DataAnnotations;

namespace SchoolAPI.Models.Dto
{
    public class StudentDto
    {
        public int StudentId { get; set; }
        [Required]
        public string? StudentName { get; set; }
        public DateTime DateOfBirth { get; set; }
        [Required]
        public int GradeId { get; set; }
    }
}
