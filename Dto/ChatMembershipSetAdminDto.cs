using System.ComponentModel.DataAnnotations;

namespace PloomesCsharpChallenge.Dto
{
  public class ChatMembershipSetAdminDto
  {
    [Required]
    public bool IsAdmin { get; set; }
  }
}
