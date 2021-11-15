using System.ComponentModel.DataAnnotations;

namespace PloomesCsharpChallenge.Dto
{
  class ChatMembershipSetAdmin
  {
    [Required]
    public bool IsAdmin { get; set; }
  }
}
