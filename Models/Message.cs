using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PloomesCsharpChallenge.Models
{
  public class Message
  {
    [Key]
    public int Id { get; set; }

    [Required]
    [ForeignKey("Chat")]
    public int ChatId { get; set; }

    public Chat Chat { get; set; }

    [Required]
    [ForeignKey("User")]
    public int SenderId { get; set; }

    public User User { get; set; }

    [Required]
    public string MessageBody { get; set; }

    [Required]
    public DateTime Created { get; set; }

    [Required]
    public DateTime LastChanged { get; set; }
  }
}
