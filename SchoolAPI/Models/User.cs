using System.ComponentModel.DataAnnotations;

namespace SchoolAPI.Models
{
    public class User
    {
        [Key]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
