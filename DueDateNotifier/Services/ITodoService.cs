using DueDateNotifier.Dtos;

namespace DueDateNotifier.Services
{
    public interface ITodoService
    {
            Task AddTodoAsync(TodoCreateDto todo);
            Task<List<TodoCreateDto>> GetTodosByUserIdAsync(string userId);
            Task<TodoCreateDto> GetTodoByIdAndUserIdAsync(string todoId, string userId);
            Task UpdateTodoAsync(TodoCreateDto todo);
            Task<bool> DeleteTodoByIdAndUserIdAsync(string todoId, string userId);  
    }
}
