using Templates.Business.Interfaces;
using Templates.Models;
using WomenForum.Domain.Enums;
using WomenForum.Models;
using WomenForum.Models.Requests;

namespace WomenForum.Business.Interfaces;

public interface ICommunitiesBusinessService : IBaseBusinessService
{
    public Task<CommunityDto> AddCommunityAsync(CreateCommunityRequest request, CancellationToken cancellationToken);
    public Task UpdateCommunityAsync(Guid communityId, UpdateCommunityRequest request, CancellationToken cancellationToken);
    public Task DeleteCommunityAsync(Guid communityId, CancellationToken cancellationToken);
    public Task<PagedResult<CommunityDto>> GetAllCommunitiesAsync(PaginationParameters paginationParameters, CancellationToken cancellationToken);
    public Task<PagedResult<CommunityDto>> GetCommunitiesByUserIdAsync(Guid userId, PaginationParameters paginationParameters, CancellationToken cancellationToken);
    public Task<PagedResult<CommunityDto>> GetCommunitiesBySearchQueryAsync(string? searchQuery, PaginationParameters paginationParameters, CancellationToken cancellationToken);
    public Task<PagedResult<CommunityDto>> GetPopularCommunitiesAsync(PaginationParameters paginationParameters, CancellationToken cancellationToken);
    public Task ChangeCommunityVisibilityAsync(Guid communityId, VisibilityType visibility, CancellationToken cancellationToken);
}