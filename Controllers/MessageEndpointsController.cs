﻿using AutoMapper;

using Microsoft.AspNetCore.Mvc;

using PloomesCsharpChallenge.Dto;
using PloomesCsharpChallenge.Models;
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

    // POST /api/net3/message/send/{chatId}
    [HttpPost("/api/net3/message/send/{chatId}")]
    public ActionResult<MessageReadDto> Send(int chatId, [FromBody] MessageCreateDto msgData)
    {
      ValidateToken(out User? user);
      if (user is null)
      {
        return Unauthorized(new { error = "Must provide a valid user token to send a message" });
      }

      var chat = _chatRepository.GetById(chatId);
      if (chat is null)
      {
        return NotFound(new { error = "Chat not found" });
      }

      var isMember = _chatRepository.GetSingleMembership(new ChatMembership { ChatId = chatId, UserId = user.Id });
      if (isMember is null)
      {
        return Unauthorized(new { error = "User is not a member of this chat" });
      }

      var message = _mapper.Map<Message>(msgData);
      message.SenderId = user.Id;
      message.ChatId = chatId;
      message.Created = DateTime.Now;
      message.LastChanged = DateTime.Now;

      var returnedMessage = _messageRepository.Create(message);

      return CreatedAtRoute(
        nameof(GetById),
        new { returnedMessage.Id },
        _mapper.Map<MessageReadDto>(returnedMessage));
    }

    // GET /api/net3/message/{id}
    [HttpGet("/api/net3/message/{id}")]
    public ActionResult<MessageReadDto> GetById(int id)
    {
      ValidateToken(out User? user);
      if (user is null)
      {
        return Unauthorized(new { error = "Must provide a valid user token to read messages" });
      }

      var message = _messageRepository.GetById(id);
      if (message is null)
      {
        return NotFound(new { error = "Message not found" });
      }

      var chat = _chatRepository.GetById(message.ChatId);
      if (chat is null)
      {
        return NotFound(new { error = "Chat not found" });
      }

      var membership = _chatRepository.GetSingleMembership(new ChatMembership { ChatId = chat.Id, UserId = user.Id });
      if (membership is null)
      {
        return Unauthorized(new { error = "User is not a member of this chat" });
      }

      return Ok(_mapper.Map<MessageReadDto>(message));
    }

    // GET /api/net3/message/chat/{chatId}
    [HttpGet("/api/net3/message/chat/{chatId}")]
    public ActionResult<IEnumerable<MessageReadDto>> GetByChatId(int chatId)
    {
      ValidateToken(out User? user);
      if (user is null)
      {
        return Unauthorized(new { error = "Must provide a valid user token to read messages" });
      }

      var chat = _chatRepository.GetById(chatId);
      if (chat is null)
      {
        return NotFound(new { error = "Chat not found" });
      }

      var membership = _chatRepository.GetSingleMembership(new ChatMembership { ChatId = chat.Id, UserId = user.Id });
      if (membership is null)
      {
        return Unauthorized(new { error = "User is not a member of this chat" });
      }

      var messages = _messageRepository.GetAllByChatId(chatId);
      return Ok(_mapper.Map<IEnumerable<MessageReadDto>>(messages));
    }

    // PATCH /api/net3/message/{id}
    [HttpPatch("/api/net3/message/{id}")]
    public ActionResult Edit(int id, [FromBody] MessageCreateDto patchDocument)
    {
      throw new NotImplementedException();
    }

    // DELETE /api/net3/message/{id}
    [HttpDelete("/api/net3/message/{id}")]
    public ActionResult Delete(int id)
    {
      ValidateToken(out User? user);
      if (user is null)
      {
        return Unauthorized(new { error = "Must provide a valid user token to delete messages" });
      }

      var message = _messageRepository.GetById(id);
      if (message is null)
      {
        return NotFound(new { error = "Message not found" });
      }

      var chat = _chatRepository.GetById(message.ChatId);
      if (chat is null)
      {
        return NotFound(new { error = "Chat not found" });
      }

      var membership = _chatRepository.GetSingleMembership(new ChatMembership { ChatId = chat.Id, UserId = user.Id });
      if (membership is null)
      {
        return Unauthorized(new { error = "User is not a member of this chat" });
      }

      _messageRepository.Delete(message);
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
