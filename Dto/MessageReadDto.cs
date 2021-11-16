namespace PloomesCsharpChallenge.Dto
{
  public class MessageReadDto : MessageBaseDto
  {
    public int Id { get; set; }
    public int ChatId { get; set; }
    public int SenderId { get; set; }
    public DateTime Created { get; set; }
    public DateTime LastChanged { get; set; }
  }
}
