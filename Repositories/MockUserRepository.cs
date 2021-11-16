using PloomesCsharpChallenge.Models;

namespace PloomesCsharpChallenge.Repositories
{
  public class MockUserRepository : IUserRepository
  {
    private readonly List<User> _users = new ()
    {
      new User { Id = 0, FirstName = "John", LastName = "Smith", Username = "johnsmith1247", AuthToken = "992d6f253f421a7d7f14244d0f98bb472bfcc1e1d3b3eb18e73ee34ba2f5eb13" },
      new User { Id = 1, FirstName = "Jane", LastName = "Doe", Username = "MrsJaneDoe", AuthToken = "3ad23d34076e9c61cf56f114e9f483405dde352f20dd570a86ea7639fa83ca37" },
      new User { Id = 2, FirstName = "Clarence", LastName = "Park", Username = "Lonstrould83", AuthToken = "71a643b01ba734a6338cb4cf92ed23908a5c84ed77e6c80325f2f36406b719d2" },
      new User { Id = 3, FirstName = "Guy", LastName = "Tang", Username = "kneve1970", AuthToken = "147e28e9ab7c021514f886fb8a9a118e30fdb0f07987b388e282b65b7ea375e7" },
      new User { Id = 4, FirstName = "Melissa", LastName = "Blevins", Username = "glactionD", AuthToken = "2f0ebbdbcdfc436780679e036709b2e570dc614ffdac12a3099178e3edae8096" },
      new User { Id = 5, FirstName = "Charles", LastName = "Brandenburg", Username = "nessittere", AuthToken = "362b76c708d25364b7fe6bc3192e75060e3b03d7db0f7c28ad1758f810dc8f00" },
    };

    public User? GetById(int id)
    {
      return _users.FirstOrDefault(el => el.Id == id);
    }

    public User? GetByName(string username)
    {
      return _users.FirstOrDefault(el => el.Username == username);
    }

    public User? GetByToken(string token)
    {
      return _users.FirstOrDefault(el => el.AuthToken == token);
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
