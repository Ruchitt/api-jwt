using System.ComponentModel.DataAnnotations;

namespace Api_Identity_JWT.Models
{
    public class Register
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
