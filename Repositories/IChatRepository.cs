using PloomesCsharpChallenge.Models;

namespace PloomesCsharpChallenge.Repositories
{
  public interface IChatRepository
  {
    IEnumerable<Chat> GetAll();
    Chat? GetById(int chatId);
    IEnumerable<ChatMembership> GetMembershipsByChat(int chatId);
    IEnumerable<ChatMembership> GetMembershipsByUser(int userId);
    ChatMembership? GetSingleMembership(ChatMembership memberData);
    Chat Create(Chat chatData);
    void Delete(Chat chatData);
    void AddUser(ChatMembership memberData);
    void RemoveUser(ChatMembership memberData);
    void SetAdmin(ChatMembership memberData);
    bool SaveChanges();
  }
}
