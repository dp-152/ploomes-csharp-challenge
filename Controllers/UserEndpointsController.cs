using AutoMapper;

using Microsoft.AspNetCore.Mvc;

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
  }
}
