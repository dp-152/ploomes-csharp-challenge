namespace PloomesCsharpChallenge.Dto
{
  class ChatCreateGroupDto
  {
    public string? Title { get; set; }
    public string Type { get; } = "group";
  }
}
