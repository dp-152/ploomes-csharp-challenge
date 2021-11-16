using PloomesCsharpChallenge.Models;

namespace PloomesCsharpChallenge.Repositories
{
  public interface IChatRepository
  {
    IEnumerable<Chat> GetAll();
    IEnumerable<ChatMembership> GetMemberships(int chatId);
    Chat Create(Chat chatData);
    void Delete(int chatId);
    void AddUser(ChatMembership memberData);
    void RemoveUser(ChatMembership memberData);
    void SetAdmin(ChatMembership memberData);
    bool SaveChanges();
  }
}
