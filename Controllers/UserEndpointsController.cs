using AutoMapper;

using Microsoft.AspNetCore.Mvc;

using PloomesCsharpChallenge.Dto;
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
      throw new NotImplementedException();
    }

    // GET /api/net3/user/{username}
    [HttpGet("username/{username}")]
    public ActionResult<UserReadDto> GetByUsername(string username)
    {
      throw new NotImplementedException();
    }

    // POST /api/net3/user
    [HttpPost]
    public ActionResult<UserReadDto> Register(UserCreateDto userCreateDto)
    {
      throw new NotImplementedException();
    }
  }
}
