using System.ComponentModel.DataAnnotations;

namespace PloomesCsharpChallenge.Dto
{
  public class ChatCreatePrivateDto
  {
    [Required]
    public int SecondPartyId { get; set; }
  }
}
