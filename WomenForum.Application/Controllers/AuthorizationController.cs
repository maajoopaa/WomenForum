using Microsoft.AspNetCore.Mvc;
using WomenForum.Business.Interfaces;
using WomenForum.Models;
using WomenForum.Models.Requests;

namespace WomenForum.Controllers;

[ApiController]
[Route("authorization")]
[Produces("application/json")]
public class AuthorizationController : ControllerBase
{
    private readonly IAuthorizationBusinessService _service;

    public AuthorizationController(IAuthorizationBusinessService service)
    {
        _service = service;
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthorizationResponse>> LoginAsync([FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _service.LoginAsync(request, cancellationToken);

        return Ok(response);
    }
    
    [HttpPost("register")]
    public async Task<ActionResult<AuthorizationResponse>> RegisterAsync([FromBody] CreateUserRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _service.RegisterAsync(request, cancellationToken);

        return Ok(response);
    }
}