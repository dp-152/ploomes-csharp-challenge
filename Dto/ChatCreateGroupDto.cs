using System.ComponentModel.DataAnnotations;

namespace PloomesCsharpChallenge.Dto
{
  public class ChatCreateGroupDto
  {
    [Required]
    public string Title { get; set; }
  }
}
