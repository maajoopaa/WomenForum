using Templates.Business.Interfaces;
using Templates.Models;
using WomenForum.Models;
using WomenForum.Models.Requests;

namespace WomenForum.Business.Interfaces;

public interface IDiscussionThreadsBusinessService : IBaseBusinessService
{
    public Task<DiscussionThreadDto> AddDiscussionThreadAsync(CreateDiscussionThreadRequest request, CancellationToken cancellationToken);
    public Task UpdateDiscussionThreadAsync(Guid discussionThreadId, UpdateDiscussionThreadRequest request, CancellationToken cancellationToken);
    public Task DeleteDiscussionThreadAsync(Guid threadId, CancellationToken cancellationToken);
    public Task<PagedResult<DiscussionThreadDto>> GetAllDiscussionThreadsAsync(PaginationParameters paginationParameters, CancellationToken cancellationToken);
    public Task<PagedResult<DiscussionThreadDto>> GetDiscussionThreadsBySearchQueryAsync(string? searchQuery, PaginationParameters paginationParameters, CancellationToken cancellationToken);
    public Task<PagedResult<DiscussionThreadDto>> GetPopularDiscussionThreadsAsync(PaginationParameters paginationParameters, CancellationToken cancellationToken);
    public Task<PagedResult<DiscussionThreadDto>> GetRecentDiscussionThreadsAsync(PaginationParameters paginationParameters, CancellationToken cancellationToken);
}