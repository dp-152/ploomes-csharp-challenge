using PloomesCsharpChallenge.Models;

namespace PloomesCsharpChallenge.Repositories
{
  public class MockUserRepository : IUserRepository
  {
    private readonly List<User> _users = new ()
    {
      new User { Id = 0, FirstName = "John", LastName = "Smith", Username = "johnsmith1247" },
      new User { Id = 1, FirstName = "Jane", LastName = "Doe", Username = "MrsJaneDoe" },
      new User { Id = 2, FirstName = "Clarence", LastName = "Park", Username = "Lonstrould83" },
      new User { Id = 3, FirstName = "Guy", LastName = "Tang", Username = "kneve1970" },
      new User { Id = 4, FirstName = "Melissa", LastName = "Blevins", Username = "glactionD" },
      new User { Id = 5, FirstName = "Charles", LastName = "Brandenburg", Username = "nessittere" },
    };

    public User? GetById(int id)
    {
      return _users.FirstOrDefault(el => el.Id == id);
    }

    public User? GetByName(string username)
    {
      return _users.FirstOrDefault(el => el.Username == username);
    }

    public User Register(User userData)
    {
      _users.Add(userData);
      return userData;
    }

    public bool SaveChanges()
    {
      return true;
    }
  }
}
