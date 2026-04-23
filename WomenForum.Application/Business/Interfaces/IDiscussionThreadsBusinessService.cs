using Templates.Business.Interfaces;
using WomenForum.Models;
using WomenForum.Models.Requests;

namespace WomenForum.Business.Interfaces;

public interface IDiscussionThreadsBusinessService : IBaseBusinessService
{
    public Task<DiscussionThreadDto> AddDiscussionThreadAsync(CreateDiscussionThreadRequest request, CancellationToken cancellationToken);
    public Task UpdateDiscussionThreadAsync(Guid discussionThreadId, UpdateDiscussionThreadRequest request, CancellationToken cancellationToken);
    public Task DeleteDiscussionThreadAsync(Guid threadId, CancellationToken cancellationToken);
    public Task<List<DiscussionThreadDto>> GetAllDiscussionThreadsAsync(CancellationToken cancellationToken);
    public Task<List<DiscussionThreadDto>> GetDiscussionThreadsBySearchQueryAsync(string searchQuery, CancellationToken cancellationToken);
    public Task<List<DiscussionThreadDto>> GetPopularDiscussionThreadsAsync(CancellationToken cancellationToken);
    public Task<List<DiscussionThreadDto>> GetRecentDiscussionThreadsAsync(CancellationToken cancellationToken);
}