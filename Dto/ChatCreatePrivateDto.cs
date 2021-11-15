using System.ComponentModel.DataAnnotations;

namespace PloomesCsharpChallenge.Dto
{
  class ChatCreatePrivateDto
  {
    [Required]
    public string? Title { get; set; }
  }
}
