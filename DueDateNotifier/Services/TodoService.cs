using DueDateNotifier.Dtos;
using DueDateNotifier.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DueDateNotifier.Services
{
    public class TodoService : ITodoService
    {
        private readonly IMongoCollection<ApplicationTodos> _todoCollection;

        public TodoService(IMongoDatabase database)
        {
            _todoCollection = database.GetCollection<ApplicationTodos>("todos");
        }

        public async Task AddTodoAsync(TodoCreateDto todoDto)
        {
            var todo = new ApplicationTodos
            {
                TaskName = todoDto.TaskName,
                Description = todoDto.Description,
                DueDate = todoDto.DueDate,
                UserId = todoDto.UserId,
                // Set other properties as needed
            };

            // Insert the Todo directly into the MongoDB collection
            await _todoCollection.InsertOneAsync(todo);
        }

        public async Task<List<TodoCreateDto>> GetTodosByUserIdAsync(string userId)
        {
            var filter = Builders<ApplicationTodos>.Filter.Eq("UserId", userId);

            // Retrieve Todos for a specific user from the MongoDB collection
            var todos = await _todoCollection.Find(filter).ToListAsync();

            // Map Todos to TodoCreateDto or use AutoMapper
            return todos.Select(todo => new TodoCreateDto
            {
                TaskName = todo.TaskName,
                Description = todo.Description,
                DueDate = todo.DueDate,
                UserId = todo.UserId,
                // Map other properties as needed
            }).ToList();
        }

        public async Task<TodoCreateDto> GetTodoByIdAndUserIdAsync(string todoId, string userId)
        {
            var filter = Builders<ApplicationTodos>.Filter.And(
                Builders<ApplicationTodos>.Filter.Eq("Id", todoId),
                Builders<ApplicationTodos>.Filter.Eq("UserId", userId)
            );

            // Retrieve a specific Todo for a user from the MongoDB collection
            var todo = await _todoCollection.Find(filter).FirstOrDefaultAsync();

            // Map Todo to TodoCreateDto or use AutoMapper
            return new TodoCreateDto
            {
                TaskName = todo?.TaskName,
                Description = todo?.Description,
                DueDate = todo?.DueDate ?? DateTime.MinValue,
                UserId = todo?.UserId ?? Guid.NewGuid().ToString(),
                // Map other properties as needed
            };
        }

        public async Task UpdateTodoAsync(TodoCreateDto todoDto)
        {
            var filter = Builders<ApplicationTodos>.Filter.Eq("Id", todoDto.Id);
            var update = Builders<ApplicationTodos>.Update
                .Set("TaskName", todoDto.TaskName)
                .Set("Description", todoDto.Description)
                .Set("DueDate", todoDto.DueDate);

            // Update Todo in the MongoDB collection
            await _todoCollection.UpdateOneAsync(filter, update);
        }

        public async Task<bool> DeleteTodoByIdAndUserIdAsync(string todoId, string userId)
        {
            var filter = Builders<ApplicationTodos>.Filter.And(
                Builders<ApplicationTodos>.Filter.Eq("Id", todoId),
                Builders<ApplicationTodos>.Filter.Eq("UserId", userId)
            );

            // Retrieve and delete a specific Todo for a user from the MongoDB collection
            var result = await _todoCollection.DeleteOneAsync(filter);

            // Check if any document was deleted
            return result.DeletedCount > 0;
        }
    }
}
