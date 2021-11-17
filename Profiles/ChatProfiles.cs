using AutoMapper;
using PloomesCsharpChallenge.Dto;
using PloomesCsharpChallenge.Models;

namespace PloomesCsharpChallenge.Profiles
{
  public class ChatProfile : Profile
  {
    public ChatProfile()
    {
      // Read (internal -> client)
      CreateMap<Chat, ChatReadDto>();

      // Create (client -> internal)
      CreateMap<ChatCreateGroupDto, Chat>();

      // ## MEMBERSHIP ##
      // Read (internal -> client)
      CreateMap<ChatMembership, ChatMembershipReadDto>();

      // Update (client -> internal)
      CreateMap<ChatMembershipSetAdminDto, ChatMembership>();
    }
  }
}
