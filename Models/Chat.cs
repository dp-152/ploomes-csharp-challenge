using System.ComponentModel.DataAnnotations;

namespace PloomesCsharpChallenge.Models
{
  public class Chat
  {
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string? Title { get; set; }

    [Required]
    [MaxLength(7)]
    public string? Type { get; set; }
  }
}
