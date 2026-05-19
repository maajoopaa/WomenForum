using Templates.Business.Interfaces;
using Templates.Models;
using WomenForum.Models;
using WomenForum.Models.Requests;

namespace WomenForum.Business.Interfaces;

public interface IMessagesBusinessService : IBaseBusinessService
{
    public Task<MessageDto> AddMessageAsync(Guid threadId, CreateMessageRequest request, CancellationToken cancellationToken);
    public Task UpdateMessageAsync(Guid messageId, UpdateMessageRequest request, CancellationToken cancellationToken);
    public Task DeleteMessagesAsync(List<Guid> messageIds, CancellationToken cancellationToken);
    public Task<PagedResult<MessageDto>> GetMessagesByDiscussionThreadIdAsync(Guid discussionThreadId, PaginationParameters paginationParameters, CancellationToken cancellationToken);
    public Task<PagedResult<MessageDto>> GetRepliesAsync(Guid messageId, PaginationParameters paginationParameters, CancellationToken cancellationToken);
}