using System.ComponentModel.DataAnnotations;

namespace PloomesCsharpChallenge.Dto
{
  class MessageBaseDto
  {
    [Required]
    public int ChatId { get; set; }

    [Required]
    public int SenderId { get; set; }

    [Required]
    public string? MessageBody { get; set; }
  }
}
