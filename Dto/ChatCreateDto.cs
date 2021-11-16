using System.ComponentModel.DataAnnotations;

namespace PloomesCsharpChallenge.Dto
{
  class ChatCreateDto
  {
    [Required]
    public string? Title { get; set; }
  }
}
