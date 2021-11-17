using PloomesCsharpChallenge.Contexts;
using PloomesCsharpChallenge.Models;

namespace PloomesCsharpChallenge.Repositories
{
  public class SqlMessageRepository : IMessageRepository
  {
    private readonly MainContext _context;

    public SqlMessageRepository(MainContext context)
    {
      _context = context;
    }

    public Message Create(Message msgData)
    {
      var created = _context.Messages.Add(msgData);
      return created.Entity;
    }

    public void Delete(Message msgData)
    {
      _context.Messages.Remove(msgData);
    }

    public void DeleteAllInChat(int chatId)
    {
      _context.Messages.RemoveRange(_context.Messages.Where(m => m.ChatId == chatId));
    }

    public void DeleteAllInChatByUserId(int chatId, int userId)
    {
      _context.Messages.RemoveRange(_context.Messages.Where(m => m.ChatId == chatId && m.SenderId == userId));
    }

    public IEnumerable<Message> GetAllByChatId(int chatId)
    {
      return _context.Messages.Where(el => el.ChatId == chatId);
    }

    public Message? GetById(int id)
    {
      return _context.Messages.FirstOrDefault(el => el.Id == id);
    }

    public bool SaveChanges()
    {
      return _context.SaveChanges() >= 0;
    }

    public void Update(Message msgData)
    { }
  }
}
