namespace PloomesCsharpChallenge.Dto
{
  class ChatCreatePrivateDto
  {
    public string? Title { get; set; }
    public string Type { get; } = "private";
  }
}
