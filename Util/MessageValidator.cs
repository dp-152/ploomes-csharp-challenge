using Microsoft.AspNetCore.Mvc.ModelBinding;

using PloomesCsharpChallenge.Models;
using PloomesCsharpChallenge.Repositories;

namespace PloomesCsharpChallenge.Util
{
  public class MessageValidator
  {
    private readonly IMessageRepository _messageRepository;
    public MessageValidator(IMessageRepository messageRepository)
    {
      _messageRepository = messageRepository;
    }

    public void ValidateMessageExists(out Message? message, int messageId, ModelStateDictionary modelState)
    {
      message = _messageRepository.GetById(messageId);
      if (message is null)
      {
        modelState.AddModelError("Message", $"No such message with ID {messageId}");
      }
    }

    public void ValidateIsMessageSender(Message message, int userId, ModelStateDictionary modelState)
    {
      if (!(message.SenderId != userId))
      {
        modelState.AddModelError("MessageSender", $"User #{userId} is not the sender of message #{message.Id}");
      }
    }
  }
}
