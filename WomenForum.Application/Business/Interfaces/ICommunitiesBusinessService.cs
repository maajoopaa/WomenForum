using Templates.Business.Interfaces;
using WomenForum.Domain.Enums;
using WomenForum.Models;
using WomenForum.Models.Requests;

namespace WomenForum.Business.Interfaces;

public interface ICommunitiesBusinessService : IBaseBusinessService
{
    public Task<CommunityDto> AddCommunityAsync(CreateCommunityRequest request, CancellationToken cancellationToken);
    public Task UpdateCommunityAsync(Guid communityId, UpdateCommunityRequest request, CancellationToken cancellationToken);
    public Task DeleteCommunityAsync(Guid communityId, CancellationToken cancellationToken);
    public Task<List<CommunityDto>> GetAllCommunitiesAsync(CancellationToken cancellationToken);
    public Task<List<CommunityDto>> GetCommunitiesByUserIdAsync(Guid userId, CancellationToken cancellationToken);
    public Task<List<CommunityDto>> GetCommunitiesBySearchQueryAsync(string searchQuery, CancellationToken cancellationToken);
    public Task<List<CommunityDto>> GetPopularCommunitiesAsync(CancellationToken cancellationToken);
    public Task ChangeCommunityVisibilityAsync(Guid communityId, VisibilityType visibility, CancellationToken cancellationToken);
}