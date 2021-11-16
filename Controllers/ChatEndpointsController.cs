using AutoMapper;

using Microsoft.AspNetCore.Mvc;

using PloomesCsharpChallenge.Repositories;

namespace PloomesCsharpChallenge.Controllers
{
  public class ChatEndpointsController : ControllerBase
  {
    private readonly IUserRepository _userRepository;
    private readonly IChatRepository _chatRepository;
    private readonly IMapper _mapper;

    public ChatEndpointsController(
      IUserRepository userRepository,
      IChatRepository chatRepository,
      IMapper mapper)
    {
      _userRepository = userRepository;
      _chatRepository = chatRepository;
      _mapper = mapper;
    }
  }
}
