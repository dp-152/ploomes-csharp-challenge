using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PloomesCsharpChallenge.Models
{
  public class ChatMembership
  {
    [Key]
    public int Id { get; set; }

    [Required]
    [ForeignKey("Chat")]
    public int ChatId { get; set; }

    public Chat Chat { get; set; }

    [Required]
    [ForeignKey("User")]
    public int UserId { get; set; }

    public User User { get; set; }

    [Required]
    public bool IsAdmin { get; set; }
  }
}
