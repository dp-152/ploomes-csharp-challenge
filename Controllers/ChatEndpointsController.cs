using AutoMapper;

using Microsoft.AspNetCore.Mvc;

using PloomesCsharpChallenge.Dto;
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

    // GET /api/net3/chat
    [HttpGet("api/net3/chat")]
    public ActionResult<IEnumerable<ChatReadDto>> GetAllChats()
    {
      throw new NotImplementedException();
    }

    // GET /api/net3/chat/{chatId}
    [HttpGet("/api/net3/chat/{chatId}")]
    public ActionResult<ChatReadDto> GetChat(int chatId)
    {
      throw new NotImplementedException();
    }

    // DELETE /api/net3/chat/{chatId}
    [HttpDelete("/api/net3/chat/{chatId}")]
    public ActionResult<ChatReadDto> DeleteChat(int chatId)
    {
      throw new NotImplementedException();
    }

    // POST /api/net3/chat/private
    [HttpPost("/api/net3/chat/private")]
    public ActionResult<ChatReadDto> CreatePrivateChat([FromBody] ChatCreateDto chatData)
    {
      throw new NotImplementedException();
    }

    // POST /api/net3/chat/group
    [HttpPost("/api/net3/chat/group")]
    public ActionResult<ChatReadDto> CreateGroupChat([FromBody] ChatCreateDto chatData)
    {
      throw new NotImplementedException();
    }

    // GET /api/net3/chat/{chatId}/members
    [HttpGet("/api/net3/chat/{chatId}/members")]
    public ActionResult<IEnumerable<UserReadDto>> GetChatMembers(int chatId)
    {
      throw new NotImplementedException();
    }

    // GET /api/net3/chat/{chatId}/members/{userId}
    [HttpGet("/api/net3/chat/{chatId}/members/{userId}")]
    public ActionResult<ChatReadDto> GetUserMembership(int chatId, int userId)
    {
      throw new NotImplementedException();
    }

    // POST /api/net3/chat/{chatId}/members/{userId}
    [HttpPost("/api/net3/chat/{chatId}/members/{userId}")]
    public ActionResult<ChatReadDto> AddUserToChat(int chatId, int userId)
    {
      throw new NotImplementedException();
    }

    // DELETE /api/net3/chat/{chatId}/members/{userId}
    [HttpDelete("/api/net3/chat/{chatId}/user/{userId}")]
    public ActionResult<ChatReadDto> RemoveUserFromChat(int chatId, int userId)
    {
      throw new NotImplementedException();
    }

    // POST /api/net3/chat/{chatId}/members/{userId}/admin
    [HttpPost("/api/net3/chat/{chatId}/members/{userId}/admin")]
    public ActionResult<ChatReadDto> MakeUserAdmin(int chatId, int userId, [FromBody] ChatMembershipSetAdminDto isAdmin)
    {
      throw new NotImplementedException();
    }
  }
}
