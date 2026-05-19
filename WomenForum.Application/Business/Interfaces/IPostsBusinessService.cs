using Templates.Business.Interfaces;
using Templates.Models;
using WomenForum.Models;
using WomenForum.Models.Requests;

namespace WomenForum.Business.Interfaces;

public interface IPostsBusinessService : IBaseBusinessService
{
    public Task<PostDto> AddPostAsync(CreatePostRequest request, CancellationToken cancellationToken);
    public Task UpdatePostAsync(Guid postId, UpdatePostRequest request, CancellationToken cancellationToken);
    public Task DeletePostAsync(Guid postId, CancellationToken cancellationToken);
    public Task<PagedResult<PostDto>> GetPostsByCommunityIdAsync(Guid communityId, PaginationParameters paginationParameters, CancellationToken cancellationToken);
    public Task<PagedResult<PostDto>> GetPostsByUserIdAsync(Guid userId, PaginationParameters paginationParameters, CancellationToken cancellationToken);
    public Task<PagedResult<PostDto>> GetPopularPostsAsync(PaginationParameters paginationParameters, CancellationToken cancellationToken);
    public Task<PagedResult<PostDto>> GetRecentPostsAsync(PaginationParameters paginationParameters, CancellationToken cancellationToken);
    public Task<PagedResult<PostDto>> GetPostsBySearchQueryAsync(string? query, PaginationParameters paginationParameters, CancellationToken cancellationToken);
}