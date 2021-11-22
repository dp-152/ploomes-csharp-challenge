using AutoMapper;

using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

using PloomesCsharpChallenge.Dto;
using PloomesCsharpChallenge.Models;
using PloomesCsharpChallenge.Repositories;
using PloomesCsharpChallenge.Util;

namespace PloomesCsharpChallenge.Controllers
{
  [ApiController]
  [Route("api/message")]
  public class MessageEndpointsController : ControllerBase
  {
    private readonly IMessageRepository _messageRepository;
    private readonly IUserRepository _userRepository;
    private readonly IChatRepository _chatRepository;
    private readonly IMapper _mapper;
    private readonly ChatValidator _chatValidator;
    private readonly UserValidator _userValidator;
    private readonly MessageValidator _messageValidator;

    public MessageEndpointsController(
      IMessageRepository messageRepository,
      IUserRepository userRepository,
      IChatRepository chatRepository,
      IMapper mapper,
      ChatValidator chatValidator,
      UserValidator userValidator,
      MessageValidator messageValidator)
    {
      _messageRepository = messageRepository;
      _userRepository = userRepository;
      _chatRepository = chatRepository;
      _mapper = mapper;
      _chatValidator = chatValidator;
      _userValidator = userValidator;
      _messageValidator = messageValidator;
    }

    // POST /api/message/send/{chatId}
    [HttpPost("send/{chatId}")]
    public ActionResult<MessageReadDto> Send(int chatId, [FromBody] MessageCreateDto msgData)
    {
      _userValidator.ValidateUserToken(out User? user, Request.Headers, ModelState);
      _chatValidator.ValidateChatExists(out Chat? chat, chatId, ModelState);
      if (user is null || chat is null || ModelState.ErrorCount > 0)
      {
        return ValidationProblem(ModelState);
      }

      _chatValidator.ValidateGroupMember(out _, chatId, user.Id, ModelState);

      var message = _mapper.Map<Message>(msgData);
      message.SenderId = user.Id;
      message.Sender = user;
      message.ChatId = chatId;
      message.Chat = chat;
      message.Created = DateTime.Now;
      message.LastChanged = DateTime.Now;

      var returnedMessage = _messageRepository.Create(message);
      return !_messageRepository.SaveChanges()
        ? StatusCode(500, new { error = "A problem happened while handling your request." })
        : (ActionResult<MessageReadDto>)CreatedAtRoute(
        nameof(GetById),
        new { returnedMessage.Id },
        _mapper.Map<MessageReadDto>(returnedMessage));
    }

    // GET /api/message/{id}
    [HttpGet("{id}")]
    public ActionResult<MessageReadDto> GetById(int id)
    {
      _userValidator.ValidateUserToken(out _, Request.Headers, ModelState);
      _messageValidator.ValidateMessageExists(out Message? message, id, ModelState);
      if (message is null || ModelState.ErrorCount > 0)
      {
        return ValidationProblem(ModelState);
      }

      _chatValidator.ValidateGroupMember(out _, message.ChatId, message.SenderId, ModelState);
      return ModelState.ErrorCount > 0
        ? ValidationProblem(ModelState)
        : (ActionResult<MessageReadDto>)Ok(_mapper.Map<MessageReadDto>(message));
    }

    // GET /api/message/chat/{chatId}
    [HttpGet("chat/{chatId}")]
    public ActionResult<IEnumerable<MessageReadDto>> GetByChatId(int chatId)
    {
      _userValidator.ValidateUserToken(out User? user, Request.Headers, ModelState);
      _chatValidator.ValidateChatExists(out _, chatId, ModelState);
      if (user is null || ModelState.ErrorCount > 0)
      {
        return ValidationProblem(ModelState);
      }

      _chatValidator.ValidateGroupMember(out _, chatId, user.Id, ModelState);
      if (ModelState.ErrorCount > 0)
      {
        return ValidationProblem(ModelState);
      }

      var messages = _messageRepository.GetAllByChatId(chatId);
      return Ok(_mapper.Map<IEnumerable<MessageReadDto>>(messages));
    }

    // PATCH /api/message/{id}
    [HttpPatch("{id}")]
    public ActionResult Edit(int id, [FromBody] JsonPatchDocument<MessageCreateDto> patchDocument)
    {
      _userValidator.ValidateUserToken(out User? user, Request.Headers, ModelState);
      _messageValidator.ValidateMessageExists(out Message? message, id, ModelState);
      if (user is null || message is null || ModelState.ErrorCount > 0)
      {
        return ValidationProblem(ModelState);
      }

      _messageValidator.ValidateIsMessageSender(message, user.Id, ModelState);
      if (ModelState.ErrorCount > 0)
      {
        return ValidationProblem(ModelState);
      }

      var messageToPatch = _mapper.Map<MessageCreateDto>(message);
      patchDocument.ApplyTo(messageToPatch, ModelState);
      if (!TryValidateModel(messageToPatch))
      {
        return ValidationProblem(ModelState);
      }

      _mapper.Map(messageToPatch, message);
      message.LastChanged = DateTime.Now;

      _messageRepository.Update(message);
      return !_messageRepository.SaveChanges()
        ? StatusCode(500, new { error = "A problem happened while handling your request." })
        : NoContent();
    }

    // DELETE /api/message/{id}
    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
      _userValidator.ValidateUserToken(out User? user, Request.Headers, ModelState);
      _messageValidator.ValidateMessageExists(out Message? message, id, ModelState);
      if (user is null || message is null || ModelState.ErrorCount > 0)
      {
        return ValidationProblem(ModelState);
      }

      _chatValidator.ValidateGroupMember(out _, message.ChatId, user.Id, ModelState);
      _chatValidator.ValidateChatExists(out Chat? chat, message.ChatId, ModelState);
      if (chat is null || ModelState.ErrorCount > 0)
      {
        return ValidationProblem(ModelState);
      }

      if (chat.Type == "private")
      {
        _messageValidator.ValidateIsMessageSender(message, user.Id, ModelState);
      }
      else if (chat.Type == "group")
      {
        _chatValidator.ValidateGroupAdmin(message.ChatId, user.Id, ModelState);
      }

      if (ModelState.ErrorCount > 0)
      {
        return ValidationProblem(ModelState);
      }

      _messageRepository.Delete(message);
      return !_messageRepository.SaveChanges()
        ? StatusCode(500, new { error = "A problem happened while handling your request." })
        : NoContent();
    }
  }
}
