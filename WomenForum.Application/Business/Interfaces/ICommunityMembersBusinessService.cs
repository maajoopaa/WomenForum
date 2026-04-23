using Templates.Business.Interfaces;
using WomenForum.Domain.Enums;
using WomenForum.Models;

namespace WomenForum.Business.Interfaces;

public interface ICommunityMembersBusinessService : IBaseBusinessService
{
    public Task ChangeBanStatusCommunityMemberAsync(Guid memberId, bool isBanned, CancellationToken cancellationToken);
    public Task DeleteCommunityMembersAsync(List<Guid> memberIds, CancellationToken cancellationToken);
    public Task ChangeCommunityMemberRoleAsync(Guid memberId, CommunityRole role, CancellationToken cancellationToken);
    public Task<List<CommunityMemberDto>> GetCommunityMembersByCommunityIdAsync(Guid communityId, CancellationToken cancellationToken);
}