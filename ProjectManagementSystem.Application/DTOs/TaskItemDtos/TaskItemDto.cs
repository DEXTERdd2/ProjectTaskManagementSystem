namespace ProjectManagementSystem.Application.DTOs.TaskItemDtos
{
    public class TaskItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? AssignedTo { get; set; }
        public DateTime? DueDate { get; set; }
        public string Status { get; set; } = "Pending";
        public DateTime CreatedAt { get; set; }
        public int ProjectId { get; set; }
    }

}
