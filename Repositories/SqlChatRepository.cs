using PloomesCsharpChallenge.Contexts;
using PloomesCsharpChallenge.Models;

namespace PloomesCsharpChallenge.Repositories
{
  public class SqlChatRepository : IChatRepository
  {
    private readonly MainContext _context;

    public SqlChatRepository(MainContext context)
    {
      _context = context;
    }

    public void AddUser(ChatMembership memberData)
    {
      _context.ChatMemberships.Add(memberData);
    }

    public Chat Create(Chat chatData)
    {
      var created = _context.Chats.Add(chatData);
      return created.Entity;
    }

    public void Delete(Chat chatData)
    {
      _context.Chats.Remove(chatData);
    }

    public IEnumerable<Chat> GetAll()
    {
      return _context.Chats.ToList();
    }

    public Chat? GetById(int chatId)
    {
      return _context.Chats.FirstOrDefault(el => el.Id == chatId);
    }

    public IEnumerable<ChatMembership> GetMembershipsByChat(int chatId)
    {
      return _context.ChatMemberships.Where(el => el.ChatId == chatId);
    }

    public IEnumerable<ChatMembership> GetMembershipsByUser(int userId)
    {
      return _context.ChatMemberships.Where(el => el.UserId == userId);
    }

    public ChatMembership? GetSingleMembership(ChatMembership memberData)
    {
      return _context.ChatMemberships.FirstOrDefault(el =>
      el.ChatId == memberData.ChatId && el.UserId == memberData.UserId);
    }

    public void RemoveUser(ChatMembership memberData)
    {
      _context.ChatMemberships.Remove(memberData);
    }

    public bool SaveChanges()
    {
      return _context.SaveChanges() >= 0;
    }

    public void SetAdmin(ChatMembership memberData)
    {
      var membership = _context.ChatMemberships.FirstOrDefault(el =>
      el.ChatId == memberData.ChatId && el.UserId == memberData.UserId);
      if (membership is null)
      {
        return;
      }

      membership.IsAdmin = memberData.IsAdmin;
    }
  }
}
