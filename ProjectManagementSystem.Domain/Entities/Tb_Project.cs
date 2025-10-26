using System.ComponentModel.DataAnnotations;

public enum ProjectStatus
{
    Planned,
    Active,
    Completed,
    OnHold
}
public class Tb_Project
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public ProjectStatus Status { get; set; } = ProjectStatus.Planned;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


    public virtual ICollection<Tb_TaskItem>? TaskItems { get; set; }

    //// (Optional) Foreign key to User who created this project
    //public int UserId { get; set; }

    //[ForeignKey("UserId")]
    //public virtual Tb_User? User { get; set; }
}