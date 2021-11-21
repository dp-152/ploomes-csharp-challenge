using AutoMapper;

using Microsoft.AspNetCore.Mvc.ModelBinding;

using PloomesCsharpChallenge.Models;
using PloomesCsharpChallenge.Repositories;

namespace PloomesCsharpChallenge.Util
{
  public class ChatValidator
  {
    private readonly IChatRepository _chatRepository;

    public ChatValidator(
      IChatRepository chatRepository)
    {
      _chatRepository = chatRepository;
    }

    public void ValidateGroupMember(out ChatMembership? membership, int chatId, int userId, ModelStateDictionary modelState)
    {
      membership = _chatRepository.GetSingleMembership(new ChatMembership { ChatId = chatId, UserId = userId });
      if (membership is null)
      {
        modelState.AddModelError("ChatMember", $"User #{userId} is not a member of chat #{chatId}");
      }
    }

    public void ValidateGroupNotMember(int chatId, int userId, ModelStateDictionary modelState)
    {
      ValidateGroupMember(out ChatMembership? membership, chatId, userId, modelState);
      modelState.Remove("ChatMember");

      if (membership is not null)
      {
        modelState.AddModelError("ChatMember", $"User #{userId} is already a member of chat #{chatId}");
      }
    }

    public void ValidateGroupAdmin(int chatId, int userId, ModelStateDictionary modelState)
    {
      ValidateGroupMember(out ChatMembership? membership, chatId, userId, modelState);
      if (membership is not null && !membership.IsAdmin)
      {
        modelState.AddModelError("ChatAdmin", $"User #{userId} is not an admin of chat #{chatId}");
      }
    }

    public void ValidateChatExists(out Chat? chat, int chatId, ModelStateDictionary modelState)
    {
      chat = _chatRepository.GetById(chatId);
      if (chat is null)
      {
        modelState.AddModelError("Chat", $"No such chat with ID #{chatId}");
      }
    }

    public void ValidateInputData()
    { }
  }
}
