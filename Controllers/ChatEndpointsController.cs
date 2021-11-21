using AutoMapper;

using Microsoft.AspNetCore.Mvc;

using PloomesCsharpChallenge.Dto;
using PloomesCsharpChallenge.Models;
using PloomesCsharpChallenge.Repositories;
using PloomesCsharpChallenge.Util;

namespace PloomesCsharpChallenge.Controllers
{
  [ApiController]
  [Route("api/chat")]
  public class ChatEndpointsController : ControllerBase
  {
    private readonly IUserRepository _userRepository;
    private readonly IMessageRepository _messageRepository;
    private readonly IChatRepository _chatRepository;
    private readonly IMapper _mapper;
    private readonly ChatValidator _chatValidator;
    private readonly UserValidator _userValidator;

    public ChatEndpointsController(
      IUserRepository userRepository,
      IMessageRepository messageRepository,
      IChatRepository chatRepository,
      IMapper mapper,
      ChatValidator chatValidator,
      UserValidator userValidator)
    {
      _userRepository = userRepository;
      _messageRepository = messageRepository;
      _chatRepository = chatRepository;
      _mapper = mapper;
      _chatValidator = chatValidator;
      _userValidator = userValidator;
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
      _userValidator.ValidateUserToken(out User? user, Request.Headers, ModelState);

      _chatValidator.ValidateChatExists(out Chat? chat, chatId, ModelState);

      if (user is null || chat is null || ModelState.ErrorCount > 0)
      {
        return ValidationProblem(ModelState);
      }

      _chatValidator.ValidateGroupAdmin(user.Id, chatId, ModelState);

      _messageRepository.DeleteAllInChat(chatId);
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
      _userValidator.ValidateUserToken(out User? user, Request.Headers, ModelState);
      _userValidator.ValidateUserExists(out User? secondParty, chatData.SecondPartyId, ModelState);

      if (user is null || secondParty is null || ModelState.ErrorCount > 0)
      {
        return ValidationProblem(ModelState);
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
      _userValidator.ValidateUserToken(out User? user, Request.Headers, ModelState);

      if (user is null || ModelState.ErrorCount > 0)
      {
        return ValidationProblem(ModelState);
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
      _userValidator.ValidateUserToken(out User? user, Request.Headers, ModelState);

      _chatValidator.ValidateChatExists(out Chat? chat, chatId, ModelState);

      if (user is null || chat is null || ModelState.ErrorCount > 0)
      {
        return ValidationProblem(ModelState);
      }

      _chatValidator.ValidateGroupMember(out _, chatId, user.Id, ModelState);

      return ModelState.ErrorCount > 0
        ? ValidationProblem(ModelState)
        : (ActionResult<IEnumerable<ChatMembershipReadDto>>)Ok(_mapper.Map<IEnumerable<ChatMembershipReadDto>>(_chatRepository.GetMembershipsByChat(chatId)));
    }

    // GET /api/chat/mine
    [HttpGet("mine")]
    public ActionResult<IEnumerable<ChatReadDto>> GetMyChats()
    {
      _userValidator.ValidateUserToken(out User? user, Request.Headers, ModelState);

      return user is null || ModelState.ErrorCount > 0
        ? ValidationProblem(ModelState)
        : (ActionResult<IEnumerable<ChatReadDto>>)Ok(_mapper.Map<IEnumerable<ChatMembershipReadDto>>(_chatRepository.GetMembershipsByUser(user.Id)));
    }

    // POST /api/chat/{chatId}/members/{userId}
    [HttpPost("{chatId}/members/{userId}")]
    public ActionResult<ChatReadDto> AddUserToChat(int chatId, int userId)
    {
      _userValidator.ValidateUserToken(out User? user, Request.Headers, ModelState);
      _chatValidator.ValidateChatExists(out Chat? chat, chatId, ModelState);
      if (user is null || chat is null || ModelState.ErrorCount > 0)
      {
        return ValidationProblem(ModelState);
      }

      _chatValidator.ValidateGroupAdmin(chatId, user.Id, ModelState);
      _userValidator.ValidateUserExists(out User? userToAdd, userId, ModelState);
      if (ModelState.ErrorCount > 0 || userToAdd is null)
      {
        return ValidationProblem(ModelState);
      }

      var newMembership = new ChatMembership { ChatId = chat.Id, Chat = chat, UserId = userToAdd.Id, User = userToAdd, IsAdmin = false };

      _chatValidator.ValidateGroupNotMember(chatId, userToAdd.Id, ModelState);
      if (ModelState.ErrorCount > 0)
      {
        return ValidationProblem(ModelState);
      }

      _chatRepository.AddUser(newMembership);
      return !_chatRepository.SaveChanges()
        ? StatusCode(500, new { error = "A problem happened while handling your request." })
        : (ActionResult<ChatReadDto>)NoContent();
    }

    // DELETE /api/chat/{chatId}/members/{userId}
    [HttpDelete("{chatId}/members/{userId}")]
    public ActionResult<ChatReadDto> RemoveUserFromChat(int chatId, int userId)
    {
      _userValidator.ValidateUserToken(out User? user, Request.Headers, ModelState);
      _chatValidator.ValidateChatExists(out Chat? chat, chatId, ModelState);
      if (user is null || chat is null || ModelState.ErrorCount > 0)
      {
        return ValidationProblem(ModelState);
      }

      _chatValidator.ValidateGroupAdmin(chatId, user.Id, ModelState);
      _userValidator.ValidateUserExists(out User? userToRemove, userId, ModelState);
      if (ModelState.ErrorCount > 0 || userToRemove is null)
      {
        return ValidationProblem(ModelState);
      }

      _chatValidator.ValidateGroupMember(out ChatMembership? existingMembership, chatId, userToRemove.Id, ModelState);
      if (ModelState.ErrorCount > 0 || existingMembership is null)
      {
        return ValidationProblem(ModelState);
      }

      _messageRepository.DeleteAllInChatByUserId(chatId, userId);
      _chatRepository.RemoveUser(existingMembership);
      return !_chatRepository.SaveChanges()
        ? StatusCode(500, new { error = "A problem happened while handling your request." })
        : (ActionResult<ChatReadDto>)NoContent();
    }

    // POST /api/chat/{chatId}/members/{userId}/admin
    [HttpPost("{chatId}/members/{userId}/admin")]
    public ActionResult<ChatReadDto> SetUserAdmin(int chatId, int userId, [FromBody] ChatMembershipSetAdminDto adminMembership)
    {
      _userValidator.ValidateUserToken(out User? user, Request.Headers, ModelState);
      _chatValidator.ValidateChatExists(out Chat? chat, chatId, ModelState);
      if (user is null || chat is null || ModelState.ErrorCount > 0)
      {
        return ValidationProblem(ModelState);
      }

      _chatValidator.ValidateGroupAdmin(chatId, user.Id, ModelState);
      _userValidator.ValidateUserExists(out _, userId, ModelState);
      _chatValidator.ValidateGroupMember(out ChatMembership? membership, chatId, userId, ModelState);
      if (membership is null || ModelState.ErrorCount > 0)
      {
        return ValidationProblem(ModelState);
      }

      membership.IsAdmin = adminMembership.IsAdmin;
      _chatRepository.SetAdmin(membership);

      return !_chatRepository.SaveChanges()
        ? StatusCode(500, new { error = "A problem happened while handling your request." })
        : (ActionResult<ChatReadDto>)NoContent();
    }
  }
}
