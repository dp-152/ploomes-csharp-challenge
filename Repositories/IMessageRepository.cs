using PloomesCsharpChallenge.Models;

namespace PloomesCsharpChallenge.Repositories
{
  public interface IMessageRepository
  {
    bool SaveChanges();
    Message? GetById(int id);
    Message Create(Message msgData);
    void Update(Message msgData);
    void Delete(Message msgData);
  }
}
