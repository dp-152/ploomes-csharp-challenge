using PloomesCsharpChallenge.Models;

namespace PloomesCsharpChallenge.Repositories
{
  public interface IMessageRepository
  {
    bool SaveChanges();
    Message? GetById(int id);
    IEnumerable<Message> GetAllByChatId(int chatId);
    Message Create(Message msgData);
    void Update(Message msgData);
    void Delete(Message msgData);
    void DeleteAllInChat(int chatId);
    void DeleteAllInChatByUserId(int chatId, int userId);
  }
}
