using System.ComponentModel.DataAnnotations;

namespace PloomesCsharpChallenge.Models
{
  public class User
  {
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(30)]
    public string? FirstName { get; set; }

    [MaxLength(30)]
    public string? LastName { get; set; }

    [Required]
    [MaxLength(30)]
    public string? Username { get; set; }

    [Required]
    public string? AuthToken { get; set; }
  }
}
