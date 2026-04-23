using Templates.Business.Interfaces;
using WomenForum.Domain.Enums;
using WomenForum.Models;

namespace WomenForum.Business.Interfaces;

public interface ICommunityJoinRequestsBusinessService : IBaseBusinessService
{
    public Task JoinCommunityAsync(Guid communityId, CancellationToken cancellationToken);
    public Task<List<CommunityJoinRequestDto>> GetJoinRequestsByCommunityIdAsync(Guid communityId, CancellationToken cancellationToken);
    public Task ChangeJoinRequestStatusAsync(Guid requestId, JoinRequestStatus status, string? message, CancellationToken cancellationToken);
}