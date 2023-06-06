using System.ComponentModel.DataAnnotations;

namespace SchoolAPI.Models.Dto
{
    public class StudentCreateDto
    {
        [Required]
        [MaxLength(30)]
        public string StudentName { get; set; }
        [Required]
        public int GradeId { get; set; }
    }
}
