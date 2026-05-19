using Templates.Business.Interfaces;
using Templates.Models;
using WomenForum.Domain.Enums;
using WomenForum.Models;

namespace WomenForum.Business.Interfaces;

public interface ICommunityJoinRequestsBusinessService : IBaseBusinessService
{
    public Task JoinCommunityAsync(Guid communityId, CancellationToken cancellationToken);
    public Task<PagedResult<CommunityJoinRequestDto>> GetJoinRequestsByCommunityIdAsync(Guid communityId, PaginationParameters paginationParameters, CancellationToken cancellationToken);
    public Task ChangeJoinRequestStatusAsync(Guid requestId, JoinRequestStatus status, string? message, CancellationToken cancellationToken);
}