using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;
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

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetPaginatedTaskResult(int pageNo,int pageSize)
        {
            if(pageNo<1 || pageSize < 1)
            {
                return BadRequest("Invalid pageNo or pageSize");
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var query = dbContext.Tasks.Where(t => t.UserId == userId).OrderBy(t=>t.Id);
            var totalCount = await query.CountAsync();
            
            var paginatedOutput = await query.Skip((pageNo - 1) * pageSize).Take(pageSize).Select
                (
                    t => new TaskItemDTO
                    { 
                        Name = t.Name,
                        Description = t.Description,
                        Id = t.Id,
                        Status  = t.Status
                    }
                ).ToListAsync();

            return Ok(new
            {
                PageNo= pageNo,
                PageSize = pageSize,
                Items= paginatedOutput,
                TotalCount =totalCount
            });
        }

        [Authorize]
        [HttpGet("FilterByStatus")]
        public async Task<IActionResult> GetFilteredTasksByStatus([FromQuery] FilterDTO dto)
        {
            if (dto.PageNo < 1 || dto.PageSize < 1)
            {
                return BadRequest("Invalid pageNo or pageSize");
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var query = dbContext.Tasks.
                Where(t => t.UserId == userId);

            if(!string.IsNullOrWhiteSpace(dto.Status))
            {
                if (!System.Enum.TryParse<Status>(dto.Status, true, out var statusEnum))
                {
                    return BadRequest("Invalid task status value");
                }

                query = query.Where(t => t.Status == statusEnum).OrderBy(t => t.Id);
            }
            else
            {
                return BadRequest("Status is mandatory");
            }
            
            var totalCount = await query.CountAsync();
            var filteredTasksByStatus = await query.Skip((dto.PageNo - 1) * dto.PageSize).Take(dto.PageSize).Select(
                    t => new TaskItemDTO
                    {
                        Id = t.Id,
                        Name = t.Name,
                        Description = t.Description,
                        Status = t.Status
                    }).ToListAsync();

            return Ok(new { 
                Items = filteredTasksByStatus,
                PageNo = dto.PageNo,
                PageSize = dto.PageSize,
                TotalCount = totalCount
            });
        }

        [Authorize]
        [HttpGet("FilterByText")]
        public async Task<IActionResult> GetFilteredTasksByText([FromQuery] FilterDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Text))
                return BadRequest("Text Cannot be blank");

            if (dto.PageNo < 1 || dto.PageSize < 1)
            {
                return BadRequest("Invalid pageNo or pageSize");
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var query = dbContext.Tasks.Where(t => t.UserId == userId);

            query = query.Where(t => t.Name.Contains(dto.Text) || t.Description.Contains(dto.Text)).OrderBy(t => t.Id);
            
            var totalCount = await query.CountAsync();
            var filteredTasksByText = await query.Skip((dto.PageNo - 1) * dto.PageSize).Take(dto.PageSize).Select(
                t => new TaskItemDTO
                {
                    Id = t.Id,
                    Name = t.Name,
                    Description= t.Description,
                    Status = t.Status

                }).ToListAsync();

            return Ok(new
            {
                Items = filteredTasksByText,
                PageNo = dto.PageNo,
                PageSize = dto.PageSize,
                TotalCount = totalCount
            });
        }
    }
}
