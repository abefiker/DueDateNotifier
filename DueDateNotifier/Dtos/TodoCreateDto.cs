using DueDateNotifier.Services;
namespace DueDateNotifier.Dtos
{
    public class TodoCreateDto
    {
        public int Id { get; set; }
        public string? TaskName { get; set; }
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
        public string? UserId { get; set; } 
    }
}
