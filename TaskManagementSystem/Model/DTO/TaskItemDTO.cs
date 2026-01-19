using TaskManagementSystem.Enum;

namespace TaskManagementSystem.Model.DTO
{
    public class TaskItemDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public Status Status { get; set; }
    }
}
