using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using System.Security.Claims;
using TaskManagementSystem.Data;
using TaskManagementSystem.Model.DTO;
using TaskManagementSystem.Model.Entities;
using TaskManagementSystem.Services;

namespace TaskManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public TaskController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [Authorize]
        [HttpPost("CreateTask")]
        public async Task<IActionResult> CreateTask(CreateTaskItemDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!); 

            var taskItemEntity = new TaskItem
            {
                Name = dto.Name,
                Description = dto.Description,
                UserId = userId
            };

            dbContext.Tasks.Add(taskItemEntity);
            await dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTaskById), new { Id = taskItemEntity.Id }, new TaskItemDTO()
            {
                Id = taskItemEntity.Id,
                Name = taskItemEntity.Name,
                Description = taskItemEntity.Description,
                Status = taskItemEntity.Status
            });
        }

        [Authorize]
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetTaskById(int Id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var task = await dbContext.Tasks.FindAsync(Id);

            if (task == null)
            {
                return NotFound();
            }

            if (task.UserId != userId) 
            { 
                return Forbid();
            }

            return Ok(new TaskItemDTO{
                Id = task.Id,
                Name = task.Name,
                Description = task.Description,
                Status = task.Status
            });
        }
    }
}
