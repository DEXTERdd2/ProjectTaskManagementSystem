namespace ProjectManagementSystem.Application.DTOs.TaskItemDtos
{
    public class UpdateTaskRequest
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? AssignedTo { get; set; }
        public DateTime? DueDate { get; set; }
        public string? Status { get; set; }
    }
}
