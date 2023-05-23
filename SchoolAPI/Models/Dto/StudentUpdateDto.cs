using System.ComponentModel.DataAnnotations;

namespace SchoolAPI.Models.Dto
{
    public class StudentUpdateDto
    {
        [Required]
        public int StudentId { get; set; }
        [Required]
        [MaxLength(30)]
        public string? StudentName { get; set; }
    }
}
