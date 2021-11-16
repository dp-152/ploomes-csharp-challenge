using PloomesCsharpChallenge.Models;

namespace PloomesCsharpChallenge.Dto
{
  public class ChatReadDto
  {
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Type { get; set; }
    public ICollection<User>? Users { get; set; }
  }
}
