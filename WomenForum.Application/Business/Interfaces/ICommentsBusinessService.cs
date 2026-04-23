using Templates.Business.Interfaces;
using WomenForum.Models;
using WomenForum.Models.Requests;

namespace WomenForum.Business.Interfaces;

public interface ICommentsBusinessService : IBaseBusinessService
{
    public Task<CommentDto> AddCommentAsync(Guid postId, CreateCommentRequest request, CancellationToken cancellationToken);
    public Task UpdateCommentAsync(Guid commentId, UpdateCommentRequest request, CancellationToken cancellationToken);
    public Task DeleteCommentAsync(Guid commentId, CancellationToken cancellationToken);
}