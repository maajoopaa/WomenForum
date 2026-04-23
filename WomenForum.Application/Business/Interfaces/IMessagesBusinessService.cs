using Templates.Business.Interfaces;
using WomenForum.Models;
using WomenForum.Models.Requests;

namespace WomenForum.Business.Interfaces;

public interface IMessagesBusinessService : IBaseBusinessService
{
    public Task<MessageDto> AddMessageAsync(Guid threadId, CreateMessageRequest request, CancellationToken cancellationToken);
    public Task UpdateMessageAsync(Guid messageId, UpdateMessageRequest request, CancellationToken cancellationToken);
    public Task DeleteMessagesAsync(List<Guid> messageIds, CancellationToken cancellationToken);
    public Task<List<MessageDto>> GetMessagesByDiscussionThreadIdAsync(Guid discussionThreadId,CancellationToken cancellationToken);
    public Task<List<MessageDto>> GetRepliesAsync(Guid messageId, CancellationToken cancellationToken);
}