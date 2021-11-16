using System.ComponentModel.DataAnnotations;

namespace PloomesCsharpChallenge.Dto
{
  class ChatMembershipSetAdminDto
  {
    [Required]
    public bool IsAdmin { get; set; }
  }
}
