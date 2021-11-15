using System.ComponentModel.DataAnnotations;

namespace PloomesCsharpChallenge.Models
{
  public class ChatMembership
  {
    [Key]
    public int Id { get; set; }

    [Required]
    public int ChatId { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public bool IsAdmin { get; set; }
  }
}
