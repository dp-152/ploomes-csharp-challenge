namespace PloomesCsharpChallenge.Dto
{
  public class ChatMembershipReadDto
  {
    public int Id { get; set; }
    public int ChatId { get; set; }
    public int UserId { get; set; }
    public bool IsAdmin { get; set; }
  }
}
