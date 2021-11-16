using AutoMapper;
using PloomesCsharpChallenge.Dto;
using PloomesCsharpChallenge.Models;

namespace PloomesCsharpChallenge.Profiles
{
  public class UserProfile : Profile
  {
    public UserProfile()
    {
      // Read (internal -> client)
      CreateMap<User, UserReadDto>();

      // Create (client -> internal)
      CreateMap<UserCreateDto, User>();
    }
  }
}
