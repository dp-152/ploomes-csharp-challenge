using System.ComponentModel.DataAnnotations;

namespace PloomesCsharpChallenge.Dto
{
  public class MessageBaseDto
  {
    [Required]
    public int ChatId { get; set; }

    [Required]
    public string? MessageBody { get; set; }
  }
}
