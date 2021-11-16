using System.ComponentModel.DataAnnotations;

namespace PloomesCsharpChallenge.Dto
{
  public class UserBaseDto
  {
    [Required]
    [MaxLength(30)]
    public string FirstName { get; set; }

    [MaxLength(30)]
    public string LastName { get; set; }

    [Required]
    [MaxLength(30)]
    public string Username { get; set; }
  }
}
