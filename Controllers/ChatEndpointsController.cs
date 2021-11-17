using AutoMapper;

using Microsoft.AspNetCore.Mvc;

using PloomesCsharpChallenge.Dto;
using PloomesCsharpChallenge.Models;
using PloomesCsharpChallenge.Repositories;

namespace PloomesCsharpChallenge.Controllers
{
  [ApiController]
  [Route("api/chat")]
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

    // GET /api/chat
    [HttpGet]
    public ActionResult<IEnumerable<ChatReadDto>> GetAllChats()
    {
      return Ok(_mapper.Map<IEnumerable<ChatReadDto>>(_chatRepository.GetAll()));
    }

    // GET /api/chat/{chatId}
    [HttpGet("{chatId}", Name = "GetChat")]
    public ActionResult<ChatReadDto> GetChat(int chatId)
    {
      var chat = _chatRepository.GetById(chatId);

      return chat == null ? NotFound() : (ActionResult<ChatReadDto>)Ok(_mapper.Map<ChatReadDto>(chat));
    }

    // DELETE /api/chat/{chatId}
    [HttpDelete("{chatId}")]
    public ActionResult<ChatReadDto> DeleteChat(int chatId)
    {
      ValidateToken(out User? user);
      if (user is null)
      {
        return Unauthorized(new { error = "Must provide a valid user token to delete a chat" });
      }

      var chat = _chatRepository.GetById(chatId);
      if (chat == null)
      {
        return NotFound();
      }

      var membership = _chatRepository.GetSingleMembership(new ChatMembership { ChatId = chat.Id, UserId = user.Id });
      if (membership is null)
      {
        return Unauthorized(new { error = "User is not a member of this chat" });
      }

      if (!membership.IsAdmin)
      {
        return Unauthorized(new { error = "User is not an admin of this chat" });
      }

      _chatRepository.Delete(chat);
      if (!_chatRepository.SaveChanges())
      {
        return StatusCode(500, new { error = "A problem happened while handling your request." });
      }

      return NoContent();
    }

    // POST /api/chat/private
    [HttpPost("private")]
    public ActionResult<ChatReadDto> CreatePrivateChat([FromBody] ChatCreatePrivateDto chatData)
    {
      ValidateToken(out User? user);
      if (user is null)
      {
        return Unauthorized(new { error = "Must provide a valid user token to create a chat" });
      }

      var secondParty = _userRepository.GetById(chatData.SecondPartyId);
      if (secondParty is null)
      {
        return NotFound(new { error = "Second party user does not exist" });
      }

      var chat = new Chat { Title = $"{user.Username} x {secondParty.Username}", Type = "private" };
      var createdChat = _chatRepository.Create(chat);

      _chatRepository.AddUser(new ChatMembership { ChatId = createdChat.Id, Chat = createdChat, UserId = user.Id, User = user, IsAdmin = false });
      _chatRepository.AddUser(new ChatMembership { ChatId = createdChat.Id, Chat = createdChat, UserId = secondParty.Id, User = secondParty, IsAdmin = false });
      if (!_chatRepository.SaveChanges())
      {
        return StatusCode(500, new { error = "A problem happened while handling your request." });
      }

      return CreatedAtAction(nameof(GetChat), new { chatId = createdChat.Id }, _mapper.Map<ChatReadDto>(createdChat));
    }

    // POST /api/chat/group
    [HttpPost("group")]
    public ActionResult<ChatReadDto> CreateGroupChat([FromBody] ChatCreateGroupDto chatData)
    {
      ValidateToken(out User? user);
      if (user is null)
      {
        return Unauthorized(new { error = "Must provide a valid user token to create a chat" });
      }

      var chat = _mapper.Map<Chat>(chatData);
      chat.Type = "group";

      var createdChat = _chatRepository.Create(chat);
      _chatRepository.AddUser(new ChatMembership { ChatId = createdChat.Id, Chat = createdChat, UserId = user.Id, User = user, IsAdmin = true });
      if (!_chatRepository.SaveChanges())
      {
        return StatusCode(500, new { error = "A problem happened while handling your request." });
      }

      return CreatedAtRoute(nameof(GetChat), new { chatId = createdChat.Id }, createdChat);
    }

    // GET /api/chat/{chatId}/members
    [HttpGet("{chatId}/members")]
    public ActionResult<IEnumerable<ChatMembershipReadDto>> GetChatMembers(int chatId)
    {
      ValidateToken(out User? user);
      if (user is null)
      {
        return Unauthorized(new { error = "Must provide a valid user token to view chat members" });
      }

      var chat = _chatRepository.GetById(chatId);
      if (chat == null)
      {
        return NotFound();
      }

      var membership = _chatRepository.GetSingleMembership(new ChatMembership { ChatId = chat.Id, UserId = user.Id });
      if (membership is null)
      {
        return Unauthorized(new { error = "User is not a member of this chat" });
      }

      return Ok(_mapper.Map<IEnumerable<ChatMembershipReadDto>>(_chatRepository.GetMembershipsByChat(chatId)));
    }

    // GET /api/chat/mine
    [HttpGet("mine")]
    public ActionResult<IEnumerable<ChatReadDto>> GetMyChats()
    {
      ValidateToken(out User? user);
      if (user is null)
      {
        return Unauthorized(new { error = "Must provide a valid user token to view chat members" });
      }

      return Ok(_mapper.Map<IEnumerable<ChatMembershipReadDto>>(_chatRepository.GetMembershipsByUser(user.Id)));
    }

    // POST /api/chat/{chatId}/members/{userId}
    [HttpPost("{chatId}/members/{userId}")]
    public ActionResult<ChatReadDto> AddUserToChat(int chatId, int userId)
    {
      ValidateToken(out User? user);
      if (user is null)
      {
        return Unauthorized(new { error = "Must provide a valid user token to view chat members" });
      }

      var chat = _chatRepository.GetById(chatId);
      if (chat == null)
      {
        return NotFound();
      }

      var membership = _chatRepository.GetSingleMembership(new ChatMembership { ChatId = chat.Id, UserId = user.Id });
      if (membership is null)
      {
        return Unauthorized(new { error = "User is not a member of this chat" });
      }

      if (!membership.IsAdmin)
      {
        return Unauthorized(new { error = "User is not an admin of this chat" });
      }

      var userToAdd = _userRepository.GetById(userId);
      if (userToAdd is null)
      {
        return NotFound(new { error = "The user you tried to add does not exist" });
      }

      var newMembership = new ChatMembership { ChatId = chat.Id, Chat = chat, UserId = userToAdd.Id, User = userToAdd, IsAdmin = false };
      var existingMembership = _chatRepository.GetSingleMembership(newMembership);
      if (existingMembership is not null)
      {
        return BadRequest(new { error = "The user you tried to add is already a member of this chat" });
      }

      _chatRepository.AddUser(newMembership);
      if (!_chatRepository.SaveChanges())
      {
        return StatusCode(500, new { error = "A problem happened while handling your request." });
      }

      return NoContent();
    }

    // DELETE /api/chat/{chatId}/members/{userId}
    [HttpDelete("{chatId}/members/{userId}")]
    public ActionResult<ChatReadDto> RemoveUserFromChat(int chatId, int userId)
    {
      ValidateToken(out User? user);
      if (user is null)
      {
        return Unauthorized(new { error = "Must provide a valid user token to view chat members" });
      }

      var chat = _chatRepository.GetById(chatId);
      if (chat == null)
      {
        return NotFound();
      }

      var membership = _chatRepository.GetSingleMembership(new ChatMembership { ChatId = chat.Id, UserId = user.Id });
      if (membership is null)
      {
        return Unauthorized(new { error = "User is not a member of this chat" });
      }

      if (!membership.IsAdmin)
      {
        return Unauthorized(new { error = "User is not an admin of this chat" });
      }

      var userToAdd = _userRepository.GetById(userId);
      if (userToAdd is null)
      {
        return NotFound(new { error = "The user you tried to remove does not exist" });
      }

      var existingMembership = _chatRepository.GetSingleMembership(new ChatMembership { ChatId = chat.Id, UserId = userToAdd.Id });
      if (existingMembership is null)
      {
        return NotFound(new { error = "The user you tried to remove is not a member of this chat" });
      }

      _chatRepository.RemoveUser(existingMembership);
      if (!_chatRepository.SaveChanges())
      {
        return StatusCode(500, new { error = "A problem happened while handling your request." });
      }

      return NoContent();
    }

    // POST /api/chat/{chatId}/members/{userId}/admin
    [HttpPost("{chatId}/members/{userId}/admin")]
    public ActionResult<ChatReadDto> SetUserAdmin(int chatId, int userId, [FromBody] ChatMembershipSetAdminDto adminMembership)
    {
      ValidateToken(out User? user);
      if (user is null)
      {
        return Unauthorized(new { error = "Must provide a valid user token to view chat members" });
      }

      var chat = _chatRepository.GetById(chatId);
      if (chat == null)
      {
        return NotFound();
      }

      var membership = _chatRepository.GetSingleMembership(new ChatMembership { ChatId = chat.Id, UserId = user.Id });
      if (membership is null)
      {
        return Unauthorized(new { error = "User is not a member of this chat" });
      }

      if (!membership.IsAdmin)
      {
        return Unauthorized(new { error = "User is not an admin of this chat" });
      }

      var userToAdd = _userRepository.GetById(userId);
      if (userToAdd is null)
      {
        return NotFound(new { error = "The user you tried to modify does not exist" });
      }

      var newMembership = new ChatMembership { ChatId = chat.Id, Chat = chat, UserId = userToAdd.Id, User = userToAdd, IsAdmin = adminMembership.IsAdmin };
      var existingMembership = _chatRepository.GetSingleMembership(newMembership);
      if (existingMembership is null)
      {
        return BadRequest(new { error = "The user you tried to modify is not a member of this chat" });
      }

      _chatRepository.SetAdmin(newMembership);
      if (!_chatRepository.SaveChanges())
      {
        return StatusCode(500, new { error = "A problem happened while handling your request." });
      }

      return NoContent();
    }

    internal User? GetUser(string token)
    {
      return _userRepository.GetByToken(token);
    }

    internal void ValidateToken(out User? user)
    {
      Request.Headers.TryGetValue("Authorization", out var token);
      if (token.Count < 1 || string.IsNullOrWhiteSpace(token[0]))
      {
        user = null;
        return;
      }

      user = GetUser(token[0]);
    }
  }
}
