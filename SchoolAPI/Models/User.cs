using System.ComponentModel.DataAnnotations;

namespace SchoolAPI.Models
{
    public class User
    {
        [Key]
        public string? Name { get; set; }
        public string? Password { get; set; }
    }
}
