using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using System.Security.Claims;
using TaskManagementSystem.Data;
using TaskManagementSystem.Enum;
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
        [HttpPost]
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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var task = await dbContext.Tasks.FindAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            if (task.UserId != userId)
            {
                return Forbid();
            }

            return Ok(new TaskItemDTO {
                Id = task.Id,
                Name = task.Name,
                Description = task.Description,
                Status = task.Status
            });
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, TaskItemDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (dto.Id != id)
                return BadRequest("Task ID mismatch");

            if (!System.Enum.IsDefined(typeof(Status),dto.Status))
            {
                return BadRequest("Invalid task status value");
            }


            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var task = await dbContext.Tasks.FindAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            if (task.UserId != userId)
            {
                return Forbid();
            }

            task.Name = dto.Name;
            task.Description = dto.Description;
            task.Status = dto.Status;
            task.LastUpdatedDate = DateTime.Now;


            await dbContext.SaveChangesAsync();

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var task = await dbContext.Tasks.FindAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            if (task.UserId != userId)
            {
                return Forbid();
            }

            dbContext.Tasks.Remove(task);
            await dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
