using System.ComponentModel.DataAnnotations;

namespace PloomesCsharpChallenge.Dto
{
  public class ChatCreateDto
  {
    [Required]
    public string? Title { get; set; }
  }
}
