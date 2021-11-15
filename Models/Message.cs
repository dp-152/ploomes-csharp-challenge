using System.ComponentModel.DataAnnotations;

namespace PloomesCsharpChallenge.Models
{
  class Message
  {
    [Key]
    public int Id { get; set; }

    [Required]
    public int ChatId { get; set; }

    [Required]
    public int SenderId { get; set; }

    [Required]
    public string? MessageBody { get; set; }

    [Required]
    public DateTime Created { get; set; }

    [Required]
    public DateTime LastChanged { get; set; }
  }
}
