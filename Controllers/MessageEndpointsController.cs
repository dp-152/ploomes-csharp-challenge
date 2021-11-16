using AutoMapper;

using Microsoft.AspNetCore.Mvc;

using PloomesCsharpChallenge.Repositories;

namespace PloomesCsharpChallenge.Controllers
{
  public class MessageEndpointsController : ControllerBase
  {
    private readonly IMessageRepository _messageRepository;
    private readonly IUserRepository _userRepository;
    private readonly IChatRepository _chatRepository;
    private readonly IMapper _mapper;

    public MessageEndpointsController(
      IMessageRepository messageRepository,
      IUserRepository userRepository,
      IChatRepository chatRepository,
      IMapper mapper)
    {
      _messageRepository = messageRepository;
      _userRepository = userRepository;
      _chatRepository = chatRepository;
      _mapper = mapper;
    }
  }
}
