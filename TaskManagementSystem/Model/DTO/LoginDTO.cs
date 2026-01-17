using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Model.DTO
{
    public class LoginDTO
    {
        [Required, MaxLength(256)]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
