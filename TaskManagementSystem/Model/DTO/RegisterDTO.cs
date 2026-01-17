using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Model.DTO
{
    public class RegisterDTO
    {
        [Required, EmailAddress, MaxLength(256)]
        public string Email { get; set; }

        [Required,MaxLength(256)]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
