using System.ComponentModel.DataAnnotations;
using TaskManagementSystem.Enum;

namespace TaskManagementSystem.Model.Entities
{
    public class TaskItem
    {
        public int Id { get; set; }
        [MaxLength(1000)]
        public string Name { get; set; } = null!;
        [MaxLength(200)]
        public string Description { get; set; } = null!;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? LastUpdatedDate { get; set; }
        public int UserId {  get; set; }
        public User User { get; set; } = null!;
        public Status Status { get; set; }= Status.Pending;
    }
}
