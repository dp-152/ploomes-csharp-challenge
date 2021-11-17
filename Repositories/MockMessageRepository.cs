using PloomesCsharpChallenge.Models;

namespace PloomesCsharpChallenge.Repositories
{
  public class MockMessageRepository : IMessageRepository
  {
    private readonly List<Message> _messages = new ()
    {
      new Message { Id = 0, ChatId = 0, SenderId = 0, MessageBody = "Hello, Clarence! From John Smith@johnsmith1247 x Lonstrould83" },
      new Message { Id = 1, ChatId = 0, SenderId = 2, MessageBody = "Hello, John! From Clarence Park@johnsmith1247 x Lonstrould83" },
      new Message { Id = 2, ChatId = 2, SenderId = 4, MessageBody = "Hello, world! From Melisssa Blevins@Chat2" },
      new Message { Id = 3, ChatId = 3, SenderId = 2, MessageBody = "Hello, Charles! From Clarence Park@Lonstrould83 x nessittere" },
      new Message { Id = 4, ChatId = 1, SenderId = 1, MessageBody = "Hello, world! From Jane Doe@Chat1" },
      new Message { Id = 5, ChatId = 2, SenderId = 1, MessageBody = "Hello, Melissa & world! From John Smith@Chat2" },
      new Message { Id = 6, ChatId = 1, SenderId = 3, MessageBody = "Hello, Jane & world! From Guy Tang@Chat1" },
      new Message { Id = 7, ChatId = 1, SenderId = 5, MessageBody = "Hello, Jane, Guy & world! From Charles Brandenburg@Chat1" },
      new Message { Id = 8, ChatId = 2, SenderId = 3, MessageBody = "Hello, Melissa, John & world! From Guy Tang@Chat2" },
      new Message { Id = 9, ChatId = 0, SenderId = 2, MessageBody = "How are you doing today, Mr. John Smith? Clarence Park@johnsmith1247 x Lonstrould83" },
      new Message { Id = 10, ChatId = 3, SenderId = 5, MessageBody = "Hello, Clarence! From Charles Brandenburg@Lonstrould83 x nessittere" },
      new Message { Id = 11, ChatId = 0, SenderId = 0, MessageBody = "I am doing very well! John Smith@johnsmith1247 x Lonstrould83" },
      new Message { Id = 12, ChatId = 0, SenderId = 2, MessageBody = "I am very glad to hear it! Clarence Park@johnsmith1247 x Lonstrould83" },
      new Message { Id = 13, ChatId = 1, SenderId = 4, MessageBody = "Hello, Jane, Guy, Charles & world! From Melissa Blevins@Chat1" },
      new Message { Id = 14, ChatId = 3, SenderId = 2, MessageBody = "How are you doing today, Mr. Charles Brandenburg? Clarence Park@Lonstrould83 x nessittere" },
      new Message { Id = 15, ChatId = 0, SenderId = 0, MessageBody = "How are you doing today, Mrs. Clarence Park? John Smith@ohnsmith1247 x Lonstrould8" },
      new Message { Id = 16, ChatId = 2, SenderId = 4, MessageBody = "Nice weather today, isn't it? Melissa Blevins@Chat2" },
      new Message { Id = 17, ChatId = 0, SenderId = 0, MessageBody = "I hope you are doing very well! John Smith@johnsmith1247 x Lonstrould83" },
      new Message { Id = 18, ChatId = 1, SenderId = 2, MessageBody = "Hello, Jane, Guy, Charles, Melissa & world! From Clarence Park@Chat1" },
      new Message { Id = 19, ChatId = 0, SenderId = 2, MessageBody = "I am, indeed, doing very well! Clarence Park@johnsmith1247 x Lonstrould83" },
    };

    private int _nextId = 19;

    public Message Create(Message msgData)
    {
      msgData.Id = _nextId++;
      _messages.Add(msgData);
      return msgData;
    }

    public void Delete(Message msgData)
    {
      _messages.Remove(msgData);
    }

    public IEnumerable<Message> GetAllByChatId(int chatId)
    {
      return _messages.Where(el => el.ChatId == chatId);
    }

    public Message? GetById(int id)
    {
      return _messages.FirstOrDefault(el => el.Id == id);
    }

    public bool SaveChanges()
    {
      return true;
    }

    public void Update(Message msgData)
    {
      int index = _messages.FindLastIndex(el => el.Id == msgData.Id);
      if (index > 0)
      {
        _messages[index] = msgData;
      }
    }
  }
}
