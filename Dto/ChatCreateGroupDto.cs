using System.ComponentModel.DataAnnotations;

namespace PloomesCsharpChallenge.Dto
{
  class ChatCreateGroupDto
  {
    [Required]
    public string? Title { get; set; }
  }
}
