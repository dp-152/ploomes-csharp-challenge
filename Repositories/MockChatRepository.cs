using PloomesCsharpChallenge.Models;

namespace PloomesCsharpChallenge.Repositories
{
  public class MockChatRepository : IChatRepository
  {
    private readonly List<Chat> _chats = new ()
    {
      new Chat { Id = 0, Title = "johnsmith1247 x Lonstrould83", Type = "private" },
      new Chat { Id = 1, Title = "Chat1", Type = "group" },
      new Chat { Id = 2, Title = "Chat2", Type = "group" },
      new Chat { Id = 3, Title = "Lonstrould83 x nessittere", Type = "private" },
    };

    private readonly List<ChatMembership> _chatMemberships = new ()
    {
      new ChatMembership { Id = 0, ChatId = 0, UserId = 0, IsAdmin = false },
      new ChatMembership { Id = 1, ChatId = 0, UserId = 2, IsAdmin = false },
      new ChatMembership { Id = 2, ChatId = 1, UserId = 0, IsAdmin = false },
      new ChatMembership { Id = 3, ChatId = 1, UserId = 1, IsAdmin = true },
      new ChatMembership { Id = 4, ChatId = 1, UserId = 2, IsAdmin = false },
      new ChatMembership { Id = 5, ChatId = 1, UserId = 3, IsAdmin = false },
      new ChatMembership { Id = 6, ChatId = 1, UserId = 4, IsAdmin = false },
      new ChatMembership { Id = 7, ChatId = 1, UserId = 5, IsAdmin = true },
      new ChatMembership { Id = 8, ChatId = 2, UserId = 1, IsAdmin = false },
      new ChatMembership { Id = 9, ChatId = 2, UserId = 3, IsAdmin = true },
      new ChatMembership { Id = 10, ChatId = 2, UserId = 4, IsAdmin = false },
      new ChatMembership { Id = 11, ChatId = 3, UserId = 2, IsAdmin = false },
      new ChatMembership { Id = 12, ChatId = 3, UserId = 5, IsAdmin = false },
    };

    private int _nextChatId = 4;
    private int _nextMembershipId = 13;

    public void AddUser(ChatMembership memberData)
    {
      memberData.Id = _nextMembershipId++;
      _chatMemberships.Add(memberData);
    }

    public Chat Create(Chat chatData)
    {
      chatData.Id = _nextChatId++;
      _chats.Add(chatData);
      return chatData;
    }

    public void Delete(int chatId)
    {
      _chats.RemoveAll(el => el.Id == chatId);
      _chatMemberships.RemoveAll(el => el.ChatId == chatId);
    }

    public IEnumerable<Chat> GetAll()
    {
      return new List<Chat>(_chats);
    }

    public Chat? GetById(int chatId)
    {
      return _chats.FirstOrDefault(el => el.Id == chatId);
    }

    public IEnumerable<ChatMembership> GetMemberships(int chatId)
    {
      return new List<ChatMembership>(_chatMemberships);
    }

    public ChatMembership? GetSingleMembership(ChatMembership memberData)
    {
      return _chatMemberships.FirstOrDefault(el =>
      el.ChatId == memberData.ChatId && el.UserId == memberData.UserId);
    }

    public void RemoveUser(ChatMembership memberData)
    {
      _chatMemberships.Remove(memberData);
    }

    public bool SaveChanges()
    {
      return true;
    }

    public void SetAdmin(ChatMembership memberData)
    {
      int index = _chatMemberships.FindLastIndex(el =>
      el.ChatId == memberData.ChatId && el.UserId == memberData.UserId);
      if (index > 0)
      {
        _chatMemberships[index] = memberData;
      }
    }
  }
}
