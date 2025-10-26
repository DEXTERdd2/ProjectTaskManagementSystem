using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


public enum TaskStatus
{
    Pending,
    InProgress,
    Done
}
public class Tb_TaskItem
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }
    public string? AssignedTo { get; set; }
    public DateTime? DueDate { get; set; }

    public TaskStatus Status { get; set; } = TaskStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

   
    public int ProjectId { get; set; }

    [ForeignKey("ProjectId")]
    public virtual Tb_Project? Project { get; set; }

    //// Optional: track which user created/owns this task
    //public int UserId { get; set; }

    //[ForeignKey("UserId")]
    //public virtual Tb_User? User { get; set; }
}