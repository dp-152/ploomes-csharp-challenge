using AutoMapper;

using Microsoft.AspNetCore.Mvc;

using PloomesCsharpChallenge.Dto;
using PloomesCsharpChallenge.Models;
using PloomesCsharpChallenge.Repositories;

namespace PloomesCsharpChallenge.Controllers
{
  [ApiController]
  [Route("/api/net3/user")]
  public class UserEndpointsController : ControllerBase
  {
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;

    public UserEndpointsController(IUserRepository repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    // GET /api/net3/user/{id}
    [HttpGet("{id}")]
    public ActionResult<UserReadDto> GetById(int id)
    {
      var user = _repository.GetById(id);

      return user is null ? NotFound() : (ActionResult<UserReadDto>)Ok(_mapper.Map<UserReadDto>(user));
    }

    // GET /api/net3/user/{username}
    [HttpGet("username/{username}")]
    public ActionResult<UserReadDto> GetByUsername(string username)
    {
      var user = _repository.GetByName(username);
      return user is null ? NotFound() : (ActionResult<UserReadDto>)Ok(_mapper.Map<UserReadDto>(user));
    }

    // POST /api/net3/user
    [HttpPost]
    public ActionResult<UserReadDto> Register(UserCreateDto userData)
    {
      var existingUser = _repository.GetByName(userData.Username);

      if (existingUser is not null)
      {
        ModelState.AddModelError("Username", "Username already in use");
        return ValidationProblem(new ValidationProblemDetails(ModelState));
      }

      var user = _mapper.Map<User>(userData);
      return Ok(_mapper.Map<UserMeDto>(_repository.Register(user)));
    }
  }
}
