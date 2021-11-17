using PloomesCsharpChallenge.Contexts;
using PloomesCsharpChallenge.Models;

namespace PloomesCsharpChallenge.Repositories
{
  public class SqlUserRepository : IUserRepository
  {
    private readonly MainContext _context;

    public SqlUserRepository(MainContext context)
    {
      _context = context;
    }

    public User? GetById(int id)
    {
      return _context.Users.FirstOrDefault(el => el.Id == id);
    }

    public User? GetByName(string username)
    {
      return _context.Users.FirstOrDefault(el => el.Username == username);
    }

    public User? GetByToken(string token)
    {
      return _context.Users.FirstOrDefault(el => el.AuthToken == token);
    }

    public User Register(User userData)
    {
      var created = _context.Users.Add(userData);
      return created.Entity;
    }

    public bool SaveChanges()
    {
      return _context.SaveChanges() >= 0;
    }
  }
}
