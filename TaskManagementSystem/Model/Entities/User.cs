using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Model.Entities
{
    [Index(nameof(Email), IsUnique = true)]
    [Index(nameof(UserName), IsUnique = true)]

    public class User
    {
        public int Id { get; set; }
        
        [MaxLength(256)]
        public string Email { get; set; } = null!;  
        
        [MaxLength(256)]
        public string UserName { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;

        public DateTime CreatedAt { get; set; }     
        public DateTime? LastLoginAt { get; set; }  

        public bool IsActive { get; set; } = true;  
    }
}
