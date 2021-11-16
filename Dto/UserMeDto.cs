namespace PloomesCsharpChallenge.Dto
{
  public class UserMeDto : UserBaseDto
  {
    public int Id { get; set; }
    public string? AuthToken { get; set; }
  }
}
