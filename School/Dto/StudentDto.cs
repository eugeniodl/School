using System.ComponentModel.DataAnnotations;

namespace School.Dto
{
    public class StudentDto
    {
        public int StudentId { get; set; }
        [Required]
        [MaxLength(30)]
        public string? StudentName { get; set; }
    }
}
