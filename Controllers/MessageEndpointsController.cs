using AutoMapper;

using Microsoft.AspNetCore.Mvc;

using PloomesCsharpChallenge.Dto;
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
      throw new NotImplementedException();
    }

    // GET /api/net3/message/{id}
    [HttpGet("/api/net3/message/{id}")]
    public ActionResult<MessageReadDto> GetById(int id)
    {
      throw new NotImplementedException();
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
      throw new NotImplementedException();
    }
  }
}
