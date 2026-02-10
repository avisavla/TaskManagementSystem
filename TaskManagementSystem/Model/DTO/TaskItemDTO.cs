using System.ComponentModel.DataAnnotations;
using TaskManagementSystem.Enum;

namespace TaskManagementSystem.Model.DTO
{
    public class TaskItemDTO
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string Description { get; set; } = null!;
        public Status Status { get; set; }
    }
}
