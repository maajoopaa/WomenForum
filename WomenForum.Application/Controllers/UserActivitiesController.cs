using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Templates.Models;
using WomenForum.Business.Interfaces;
using WomenForum.Models;

namespace WomenForum.Controllers;

[ApiController]
[Route("activities")]
[Produces("application/json")]
public class UserActivitiesController : ControllerBase
{
    private readonly IUserActivitiesBusinessService _userActivitiesBusinessService;

    public UserActivitiesController(IUserActivitiesBusinessService userActivitiesBusinessService)
    {
        _userActivitiesBusinessService = userActivitiesBusinessService;
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<PagedResult<UserActivityDto>>> GetActivitiesAsync([FromQuery] PaginationParameters paginationParameters, CancellationToken cancellationToken)
    {
        var result = await _userActivitiesBusinessService.GetMyActivitiesAsync(paginationParameters, cancellationToken);
        return Ok(result);
    }
}
