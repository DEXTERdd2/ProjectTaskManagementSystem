namespace ProjectManagementSystem.Application.DTOs.TaskItemDtos
{
    public class CreateTaskRequest
    {
        public string Title { get; set; } = string.Empty;
        public int ProjectId { get; set; }
        public string? Description { get; set; }
        public string? AssignedTo { get; set; }
        public DateTime? DueDate { get; set; }
        public string? Status { get; set; }
    }
}
