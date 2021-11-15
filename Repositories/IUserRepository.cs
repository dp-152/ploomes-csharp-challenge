using PloomesCsharpChallenge.Models;

namespace PloomesCsharpChallenge.Repositories
{
  public interface IUserRepository
  {
    bool SaveChanges();
    User Register(User userData);
    User GetByName(string username);
    User GetById(int id);
  }
}
