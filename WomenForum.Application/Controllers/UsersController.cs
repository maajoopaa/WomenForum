using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Templates.Models;
using WomenForum.Business.Interfaces;
using WomenForum.Domain.Enums;
using WomenForum.Models;
using WomenForum.Models.Requests;

namespace WomenForum.Controllers;

[ApiController]
[Route("users")]
[Produces("application/json")]
public class UsersController : ControllerBase
{
    private readonly IUsersBusinessService _usersBusinessService;
    private readonly ICommunitiesBusinessService _communitiesBusinessService;
    private readonly IPostsBusinessService _postsBusinessService;
    private readonly IUserSettingsBusinessService _userSettingsBusinessService;
    private readonly IUserActivitiesBusinessService _userActivitiesBusinessService;

    public UsersController(
        IUsersBusinessService usersBusinessService,
        ICommunitiesBusinessService communitiesBusinessService,
        IPostsBusinessService postsBusinessService,
        IUserSettingsBusinessService userSettingsBusinessService,
        IUserActivitiesBusinessService userActivitiesBusinessService)
    {
        _usersBusinessService = usersBusinessService;
        _communitiesBusinessService = communitiesBusinessService;
        _postsBusinessService = postsBusinessService;
        _userSettingsBusinessService = userSettingsBusinessService;
        _userActivitiesBusinessService = userActivitiesBusinessService;
    }
    
    [Authorize]
    [HttpPut("{userId:guid}")]
    public async Task<ActionResult> UpdateAsync(
        Guid userId,
        [FromBody] UpdateUserRequest request,
        CancellationToken cancellationToken)
    {
        await _usersBusinessService.UpdateUserAsync(userId, request, cancellationToken);

        return NoContent();
    }

    [Authorize]
    [HttpPatch("visibility")]
    public async Task<ActionResult> UpdateVisibilityAsync(
        [FromQuery] VisibilityType visibility,
        CancellationToken cancellationToken)
    {
        await _usersBusinessService.UpdateUserVisibilityAsync(visibility, cancellationToken);

        return NoContent();
    }

    [Authorize]
    [HttpDelete]
    public async Task<ActionResult> DeleteAsync(
        [FromQuery] List<Guid> userIds,
        CancellationToken cancellationToken)
    {
        await _usersBusinessService.DeleteUsersAsync(userIds, cancellationToken);

        return NoContent();
    }
    
    [HttpGet("{userId:guid}/communities")]
    public async Task<ActionResult<PagedResult<CommunityDto>>> GetCommunitiesAsync(Guid userId, [FromQuery] PaginationParameters paginationParameters, CancellationToken cancellationToken)
    {
        var result = await _communitiesBusinessService.GetCommunitiesByUserIdAsync(userId, paginationParameters, cancellationToken);

        return Ok(result);
    }
    
    [HttpGet("{userId:guid}/posts")]
    public async Task<ActionResult<PagedResult<PostDto>>> GetPostsAsync(Guid userId, [FromQuery] PaginationParameters paginationParameters, CancellationToken cancellationToken)
    {
        var result = await _postsBusinessService.GetPostsByUserIdAsync(userId, paginationParameters, cancellationToken);

        return Ok(result);
    }
    
    [Authorize]
    [HttpPut("settings")]
    public async Task<ActionResult> UpdateSettingsAsync([FromBody] UpdateUserSettingsRequest request,
        CancellationToken cancellationToken)
    {
        await _userSettingsBusinessService.UpdateUserSettingsAsync(request, cancellationToken);

        return NoContent();
    }

    [Authorize]
    [HttpPut("subscriptions/{targetId:guid}")]
    public async Task<ActionResult> ChangeSubscriptionStatusAsync(
        Guid targetId,
        CancellationToken cancellationToken)
    {
        await _usersBusinessService.ChangeSubscriptionStatusAsync(targetId, cancellationToken);

        return NoContent();
    }

    [HttpGet("{userId:guid}/subscriptions")]
    public async Task<ActionResult<PagedResult<SubscriptionDto>>> GetSubscriptionsAsync(
        Guid userId,
        [FromQuery] PaginationParameters paginationParameters,
        CancellationToken cancellationToken)
    {
        var result = await _usersBusinessService.GetSubscriptionsByUserIdAsync(userId, paginationParameters, cancellationToken);

        return Ok(result);
    }

    [HttpGet("{userId:guid}/subscribers")]
    public async Task<ActionResult<PagedResult<SubscriptionDto>>> GetSubscribersAsync(
        Guid userId,
        [FromQuery] PaginationParameters paginationParameters,
        CancellationToken cancellationToken)
    {
        var result = await _usersBusinessService.GetSubscribersByUserIdAsync(userId, paginationParameters, cancellationToken);

        return Ok(result);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<PagedResult<UserDto>>> GetAllAsync([FromQuery] PaginationParameters paginationParameters, CancellationToken cancellationToken)
    {
        var result = await _usersBusinessService.GetAllUsersAsync(paginationParameters, cancellationToken);

        return Ok(result);
    }
    
    [HttpGet("{userId:guid}")]
    public async Task<ActionResult<UserDto>> GetByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var result = await _usersBusinessService.GetUserByIdAsync(userId, cancellationToken);

        return Ok(result);
    }

    [HttpPatch("{userId:guid}/ban")]
    public async Task<ActionResult> ChangeUserBanStatus(Guid userId, [FromQuery] bool isBanned,
        CancellationToken cancellationToken)
    {
        await _usersBusinessService.ChangeUserBanStatus(userId, isBanned, cancellationToken);

        return NoContent();
    }
    
    [Authorize]
    [HttpGet("{userId:guid}/activities")]
    public async Task<ActionResult<PagedResult<UserActivityDto>>> GetActivitiesByIdAsync(Guid userId, [FromQuery] PaginationParameters paginationParameters, CancellationToken cancellationToken)
    {
        var result = await _userActivitiesBusinessService.GetUserActivitiesByIdAsync(userId, paginationParameters, cancellationToken);
        return Ok(result);
    }

    [Authorize]
    [HttpPatch("{userId:guid}/role")]
    public async Task<ActionResult> ChangeUserRoleAsync(Guid userId, [FromQuery] Role role,
        CancellationToken cancellationToken)
    {
        await _usersBusinessService.ChangeUserRole(userId, role, cancellationToken);

        return NoContent();
    }
}