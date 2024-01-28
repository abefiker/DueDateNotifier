using DueDateNotifier.Dtos;
using DueDateNotifier.Models;
using DueDateNotifier.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DueDateNotifier.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _todoService;
        private readonly UserManager<ApplicationUser> _userManager;

        public TodoController(ITodoService todoService, UserManager<ApplicationUser> userManager)
        {
            _todoService = todoService;
            _userManager = userManager;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddTodo([FromBody] TodoCreateDto todoDto)
        {
            try
            {
                // Get the UserId of the current user
                var userId = GetCurrentUserId();
                if (userId == null)
                {
                    return Unauthorized();
                }


                // Map DTO to Todo model
                var todo = new TodoCreateDto
                {
                    TaskName = todoDto.TaskName,
                    Description = todoDto.Description,
                    DueDate = todoDto.DueDate,
                    UserId = userId
                };

                // Add the todo
                await _todoService.AddTodoAsync(todo);

                return Ok(new { message = "Todo added successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error adding todo: {ex.Message}" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetTodos()
        {
            try
            {
                // Get the UserId of the current user
                var userId = GetCurrentUserId();
                if (userId == null)
                {
                    return Unauthorized();
                }

                // Retrieve todos for the user
                var todos = await _todoService.GetTodosByUserIdAsync(userId);

                return Ok(todos);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error retrieving todos: {ex.Message}" });
            }
        }

        [HttpGet("{todoId}")]
        public async Task<IActionResult> GetTodoById(string todoId)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId != null)
            {
                // Perform the call to GetTodosByUserIdAsync only if userId is not null
                var todos = await _todoService.GetTodosByUserIdAsync(userId);
                return Ok(todos);
            }
            else
            {
                // Handle the case where userId is null
                return BadRequest(new { message = "User ID not found" });
            }

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error retrieving todo: {ex.Message}" });
            }
        }

        [HttpPut("{todoId}")]
        public async Task<IActionResult> UpdateTodo(string todoId, [FromBody] TodoCreateDto updatedTodoDto)
        {
            try
            {
                // Get the UserId of the current user
                var userId = GetCurrentUserId();
                if (userId == null)
                {
                    return Unauthorized();
                }

                // Retrieve the specific todo for the user
                var existingTodo = await _todoService.GetTodoByIdAndUserIdAsync(todoId, userId);

                if (existingTodo == null)
                {
                    return NotFound(new { message = "Todo not found" });
                }

                // Update the properties
                existingTodo.TaskName = updatedTodoDto.TaskName;
                existingTodo.Description = updatedTodoDto.Description;
                existingTodo.DueDate = updatedTodoDto.DueDate;

                // Update the todo
                await _todoService.UpdateTodoAsync(existingTodo);

                return Ok(new { message = "Todo updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error updating todo: {ex.Message}" });
            }
        }

        [HttpDelete("{todoId}")]
        public async Task<IActionResult> DeleteTodo(string todoId)
        {
            try
            {
                // Get the UserId of the current user
                var userId = GetCurrentUserId();
                if (userId == null)
                {
                    return Unauthorized();
                }

                // Delete the specific todo for the user
                var deleted = await _todoService.DeleteTodoByIdAndUserIdAsync(todoId, userId);

                if (!deleted)
                {
                    return NotFound(new { message = "Todo not found" });
                }

                return Ok(new { message = "Todo deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error deleting todo: {ex.Message}" });
            }
        }

        private string? GetCurrentUserId()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userClaims = identity.Claims;
                return userClaims.FirstOrDefault(x => x.Type == ClaimTypes.DenyOnlyPrimarySid)?.Value;
            }
            return null;

        }
    }
}
