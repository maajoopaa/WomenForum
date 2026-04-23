using Templates.Business.Interfaces;
using WomenForum.Models;
using WomenForum.Models.Requests;

namespace WomenForum.Business.Interfaces;

public interface IPostsBusinessService : IBaseBusinessService
{
    public Task<PostDto> AddPostAsync(CreatePostRequest request, CancellationToken cancellationToken);
    public Task UpdatePostAsync(Guid postId, UpdatePostRequest request, CancellationToken cancellationToken);
    public Task DeletePostAsync(Guid postId, CancellationToken cancellationToken);
    public Task<List<PostDto>> GetPostsByCommunityIdAsync(Guid communityId, CancellationToken cancellationToken);
    public Task<List<PostDto>> GetPostsByUserIdAsync(Guid userId, CancellationToken cancellationToken);
    public Task<List<PostDto>> GetPopularPostsAsync(CancellationToken cancellationToken);
    public Task<List<PostDto>> GetRecentPostsAsync(CancellationToken cancellationToken);
    public Task<List<PostDto>> GetPostsBySearchQueryAsync(string query, CancellationToken cancellationToken);
}