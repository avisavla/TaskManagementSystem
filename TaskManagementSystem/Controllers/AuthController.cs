using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Data;
using TaskManagementSystem.Model.DTO;
using TaskManagementSystem.Model.Entities;

namespace TaskManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public AuthController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await dbContext.Users.AnyAsync(user=>user.UserName == dto.UserName))
            {
                return BadRequest("Username already exists");
            }

            if (await dbContext.Users.AnyAsync(user => user.Email == dto.Email))
            {
                return BadRequest("Email already exists");
            }

            var userEntity = new User()
            {
                Email = dto.Email,
                UserName = dto.UserName,
                CreatedAt = DateTime.Now,
                IsActive = true
            };

            var hasher = new PasswordHasher<User>();
            var hashedPassword = hasher.HashPassword(userEntity, dto.Password);

            userEntity.PasswordHash = hashedPassword;

            dbContext.Users.Add(userEntity);
            await dbContext.SaveChangesAsync();            

            return CreatedAtAction(nameof(Register),new UserResponseDTO()
            {
                Id = userEntity.Id,
                Email = userEntity.Email,
                UserName = userEntity.UserName
            });
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userEntity = await dbContext.Users.FirstOrDefaultAsync(user => user.UserName == dto.UserName);

            if (userEntity == null)
            {
                return BadRequest("Incorrect username or password");
            }
            var hashedPassword = userEntity.PasswordHash;
            var hasher = new PasswordHasher<User>();
            var unhashedPasswordEnumResult = hasher.VerifyHashedPassword(userEntity, hashedPassword, dto.Password);
            if(unhashedPasswordEnumResult == PasswordVerificationResult.Failed)
            {
                return BadRequest("Incorrect Password");
            }

            userEntity.LastLoginAt = DateTime.Now;
            await dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
