using Templates.Business.Interfaces;
using Templates.Models;
using WomenForum.Domain.Enums;
using WomenForum.Models;
using WomenForum.Models.Requests;

namespace WomenForum.Business.Interfaces;

public interface IUsersBusinessService : IBaseBusinessService
{
    public Task UpdateUserAsync(Guid userId, UpdateUserRequest request, CancellationToken cancellationToken);
    public Task UpdateUserVisibilityAsync(VisibilityType visibility, CancellationToken cancellationToken);
    public Task DeleteUsersAsync(List<Guid> userIds, CancellationToken cancellationToken);
    public Task ChangeSubscriptionStatusAsync(Guid targetId, CancellationToken cancellationToken);
    public Task<PagedResult<SubscriptionDto>> GetSubscriptionsByUserIdAsync(Guid userId, PaginationParameters paginationParameters, CancellationToken cancellationToken);
    public Task<PagedResult<SubscriptionDto>> GetSubscribersByUserIdAsync(Guid userId, PaginationParameters paginationParameters, CancellationToken cancellationToken);
    public Task<PagedResult<UserDto>> GetAllUsersAsync(PaginationParameters paginationParameters, CancellationToken cancellationToken);
}