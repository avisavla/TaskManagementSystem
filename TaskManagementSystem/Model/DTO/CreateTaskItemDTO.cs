using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Model.DTO
{
    public class CreateTaskItemDTO
    {
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string Description { get; set; } = null!;
    }
}
