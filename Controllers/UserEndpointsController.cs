using System.Security.Cryptography;
using System.Text;

using AutoMapper;

using Microsoft.AspNetCore.Mvc;

using PloomesCsharpChallenge.Dto;
using PloomesCsharpChallenge.Models;
using PloomesCsharpChallenge.Repositories;
using PloomesCsharpChallenge.Util;

namespace PloomesCsharpChallenge.Controllers
{
  [ApiController]
  [Route("api/user")]
  public class UserEndpointsController : ControllerBase
  {
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;
    private readonly UserValidator _userValidator;

    public UserEndpointsController(IUserRepository repository, IMapper mapper, UserValidator userValidator)
    {
      _repository = repository;
      _mapper = mapper;
      _userValidator = userValidator;
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
      _userValidator.ValidateUsernameNotInUse(userData.Username, ModelState);
      _userValidator.ValidateUsernameIsSafe(userData.Username, ModelState);
      if (ModelState.ErrorCount > 0)
      {
        return ValidationProblem(ModelState);
      }

      var user = _mapper.Map<User>(userData);
      user.AuthToken = GenRandomToken();
      var returnedUser = _repository.Register(user);
      return !_repository.SaveChanges()
        ? StatusCode(500, new { error = "A problem happened while handling your request." })
        : (ActionResult<UserReadDto>)CreatedAtRoute(
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
