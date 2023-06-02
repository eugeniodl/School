using System.ComponentModel.DataAnnotations;

namespace SchoolAPI.Models.Dto
{
    public class StudentDto
    {
        public int StudentId { get; set; }
        [Required]
        [StringLength(50)]
        public string? StudentName { get; set; }
    }
}
