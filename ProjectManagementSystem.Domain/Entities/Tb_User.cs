using System.ComponentModel.DataAnnotations;

public class Tb_User
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string UserName { get; set; } = string.Empty;

    [Required, MaxLength(200)]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? Role { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

   
}