using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Templates.Models;
using WomenForum.Business.Interfaces;
using WomenForum.Domain.Enums;
using WomenForum.Models;
using WomenForum.Models.Requests;

namespace WomenForum.Controllers;

[ApiController]
[Route("communities")]
[Produces("application/json")]
public class CommunitiesController : ControllerBase
{
    private readonly ICommunitiesBusinessService _communitiesBusinessService;
    private readonly ICommunityJoinRequestsBusinessService _communityJoinRequestsBusinessService;
    private readonly ICommunityMembersBusinessService _communityMembersBusinessService;
    private readonly IPostsBusinessService _postsBusinessService;

    public CommunitiesController(
        ICommunitiesBusinessService communitiesBusinessService, 
        ICommunityJoinRequestsBusinessService communityJoinRequestsBusinessService,
        ICommunityMembersBusinessService communityMembersBusinessService,
        IPostsBusinessService postsBusinessService)
    {
        _communitiesBusinessService = communitiesBusinessService;
        _communityJoinRequestsBusinessService = communityJoinRequestsBusinessService;
        _communityMembersBusinessService = communityMembersBusinessService;
        _postsBusinessService = postsBusinessService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<CommunityDto>>> GetAllAsync([FromQuery] string? searchQuery, [FromQuery] PaginationParameters paginationParameters, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(searchQuery))
        {
            var allResult = await _communitiesBusinessService.GetAllCommunitiesAsync(paginationParameters, cancellationToken);
            return Ok(allResult);
        }
        var result = await _communitiesBusinessService.GetCommunitiesBySearchQueryAsync(searchQuery, paginationParameters, cancellationToken);
        
        return Ok(result);
    }
    
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<CommunityDto>> AddAsync([FromBody] CreateCommunityRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _communitiesBusinessService.AddCommunityAsync(request, cancellationToken);

        return Created(string.Empty, result);
    }

    [Authorize]
    [HttpPut("{communityId:guid}")]
    public async Task<ActionResult> UpdateAsync(Guid communityId, [FromBody] UpdateCommunityRequest request,
        CancellationToken cancellationToken)
    {
        await _communitiesBusinessService.UpdateCommunityAsync(communityId, request, cancellationToken);
        
        return NoContent();
    }
    
    [HttpGet("{communityId:guid}/posts")]
    public async Task<ActionResult<PagedResult<PostDto>>> GetPostsAsync(Guid communityId, [FromQuery] PaginationParameters paginationParameters, CancellationToken cancellationToken)
    {
        var result = await _postsBusinessService.GetPostsByCommunityIdAsync(communityId, paginationParameters, cancellationToken);
        
        return Ok(result);
    }

    [Authorize]
    [HttpDelete("{communityId:guid}")]
    public async Task<ActionResult> DeleteAsync(Guid communityId, CancellationToken cancellationToken)
    {
        await _communitiesBusinessService.DeleteCommunityAsync(communityId, cancellationToken);
        
        return NoContent();
    }

    [Authorize]
    [HttpPatch("{communityId:guid}")]
    public async Task<ActionResult> ChangeVisibilityStatus(Guid communityId, [FromQuery] VisibilityType visibility,
        CancellationToken cancellationToken)
    {
        await _communitiesBusinessService.ChangeCommunityVisibilityAsync(communityId, visibility, cancellationToken);
        
        return  NoContent();
    }

    [Authorize]
    [HttpPost("{communityId:guid}/members")]
    public async Task<ActionResult> AddMemberAsync(Guid communityId, CancellationToken cancellationToken)
    {
        await _communityJoinRequestsBusinessService.JoinCommunityAsync(communityId, cancellationToken);

        return Created();
    }
    
    [HttpGet("{communityId:guid}/members")]
    public async Task<ActionResult<PagedResult<CommunityMemberDto>>> GetMembersAsync(Guid communityId, [FromQuery] PaginationParameters paginationParameters, CancellationToken cancellationToken)
    {
        var result = await _communityMembersBusinessService.GetCommunityMembersByCommunityIdAsync(communityId, paginationParameters, cancellationToken);

        return Ok(result);
    }

    [Authorize]
    [HttpPatch("{communityId:guid}/members/{memberId:guid}/role")]
    public async Task<ActionResult> ChangeMemberRoleAsync(Guid memberId, [FromQuery] CommunityRole role, CancellationToken cancellationToken)
    {
        await _communityMembersBusinessService.ChangeCommunityMemberRoleAsync(memberId,role, cancellationToken);

        return NoContent();
    }

    [Authorize]
    [HttpDelete("{communityId:guid}/members")]
    public async Task<ActionResult> DeleteMembersAsync([FromQuery] List<Guid> memberIds, CancellationToken cancellationToken)
    {
        await _communityMembersBusinessService.DeleteCommunityMembersAsync(memberIds, cancellationToken);

        return NoContent();
    }

    [Authorize]
    [HttpPatch("{communityId:guid}/members/{memberId:guid}/ban")]
    public async Task<ActionResult> ChangeMemberBanStatusAsync(Guid memberId, [FromQuery] bool isBanned, CancellationToken cancellationToken)
    {
        await _communityMembersBusinessService.ChangeBanStatusCommunityMemberAsync(memberId,isBanned, cancellationToken);

        return NoContent();
    }

    [Authorize]
    [HttpGet("{communityId:guid}/join-requests")]
    public async Task<ActionResult<PagedResult<CommunityJoinRequestDto>>> GetJoinRequests(Guid communityId,
        [FromQuery] PaginationParameters paginationParameters,
        CancellationToken cancellationToken)
    {
        var result =
            await _communityJoinRequestsBusinessService.GetJoinRequestsByCommunityIdAsync(communityId,
                paginationParameters,
                cancellationToken);

        return Ok(result);
    }

    [Authorize]
    [HttpPatch("{communityId:guid}/join-requests/{requestId:guid}")]
    public async Task<ActionResult> ChangeJoinRequestStatusAsync(Guid requestId, [FromQuery] JoinRequestStatus status,
        [FromQuery] string message,
        CancellationToken cancellationToken)
    {
        await _communityJoinRequestsBusinessService.ChangeJoinRequestStatusAsync(requestId, status, message, cancellationToken);
        
        return NoContent();
    }
}