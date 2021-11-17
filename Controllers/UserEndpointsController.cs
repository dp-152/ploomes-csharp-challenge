using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

using AutoMapper;

using Microsoft.AspNetCore.Mvc;

using PloomesCsharpChallenge.Dto;
using PloomesCsharpChallenge.Models;
using PloomesCsharpChallenge.Repositories;

namespace PloomesCsharpChallenge.Controllers
{
  [ApiController]
  [Route("api/user")]
  public class UserEndpointsController : ControllerBase
  {
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;

    public UserEndpointsController(IUserRepository repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    // GET /api/user/{id}
    [HttpGet("{id}", Name = "GetById")]
    public ActionResult<UserReadDto> GetById(int id)
    {
      var user = _repository.GetById(id);

      return user is null ? NotFound() : (ActionResult<UserReadDto>)Ok(_mapper.Map<UserReadDto>(user));
    }

    // GET /api/user/usename/{username}
    [HttpGet("username/{username}")]
    public ActionResult<UserReadDto> GetByUsername(string username)
    {
      var user = _repository.GetByName(username);
      return user is null ? NotFound() : (ActionResult<UserReadDto>)Ok(_mapper.Map<UserReadDto>(user));
    }

    // POST /api/user
    [HttpPost]
    public ActionResult<UserReadDto> Register([FromBody] UserCreateDto userData)
    {
      var existingUser = _repository.GetByName(userData.Username);

      if (existingUser is not null)
      {
        ModelState.AddModelError("Username", "Username already in use");
        return ValidationProblem(new ValidationProblemDetails(ModelState));
      }

      var user = _mapper.Map<User>(userData);
      if (Regex.Match(user.Username, @"^.*[\ \?\&\^\$\#\@\!\(\)\+\-\,\:\;\<\>\’\\\'\-_\*]+.*$").Success)
      {
        ModelState.AddModelError("Username", "Username contains invalid characters");
        return ValidationProblem(new ValidationProblemDetails(ModelState));
      }

      user.AuthToken = GenRandomToken();
      var returnedUser = _repository.Register(user);

      if (!_repository.SaveChanges())
      {
        return StatusCode(500, new { error = "A problem happened while handling your request." });
      }

      return CreatedAtRoute(
        nameof(GetById),
        new { returnedUser.Id },
        _mapper.Map<UserMeDto>(user));
    }

    internal string GenRandomToken()
    {
      using SHA256 hashCreator = SHA256.Create();
      using RandomNumberGenerator rngCsp = RandomNumberGenerator.Create();

      byte[] randomBytes = new byte[64];
      rngCsp.GetBytes(randomBytes);
      var hashValue = hashCreator.ComputeHash(randomBytes);
      StringBuilder hexHashValue = new (hashValue.Length * 2);
      foreach (byte b in hashValue)
      {
        hexHashValue.Append($"{b:x2}");
      }

      return hexHashValue.ToString();
    }
  }
}
