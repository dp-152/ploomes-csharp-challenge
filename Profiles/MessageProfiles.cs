using AutoMapper;
using PloomesCsharpChallenge.Dto;
using PloomesCsharpChallenge.Models;

namespace PloomesCsharpChallenge.Profiles
{
  public class MessageProfile : Profile
  {
    public MessageProfile()
    {
      // Read (internal -> client)
      CreateMap<Message, MessageReadDto>();

      // Create, Update (client -> internal)
      CreateMap<MessageCreateDto, Message>();
    }
  }
}
