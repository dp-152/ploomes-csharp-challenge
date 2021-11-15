using PloomesCsharpChallenge.Models;

namespace PloomesCsharpChallenge.Repositories
{
  public interface IChatRepository
  {
    IEnumerable<Message> GetMessages(int chatId);
    IEnumerable<Chat> GetAll();
    Chat CreatePrivate(Chat chatData);
    Chat CreateGroup(Chat chatData);
    void Delete(int chatId);
    void AddUser(ChatMembership memberData);
    void RemoveUser(ChatMembership memberData);
    void SetAdmin(ChatMembership memberData);
    bool SaveChanges();
  }
}
