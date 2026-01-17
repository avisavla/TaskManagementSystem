using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Model.DTO
{
    public class UserResponseDTO
    {
        public int Id { get; set; }
        [MaxLength(256)]
        public string Email { get; set; } = null!;

        [MaxLength(256)]
        public string UserName { get; set; } = null!;
    }
}
