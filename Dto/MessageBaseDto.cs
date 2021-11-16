using System.ComponentModel.DataAnnotations;

namespace PloomesCsharpChallenge.Dto
{
  public class MessageBaseDto
  {
    [Required]
    public string MessageBody { get; set; }
  }
}
