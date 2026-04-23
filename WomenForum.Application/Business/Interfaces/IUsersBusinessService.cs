using Templates.Business.Interfaces;
using WomenForum.Domain.Enums;
using WomenForum.Models;
using WomenForum.Models.Requests;

namespace WomenForum.Business.Interfaces;

public interface IUsersBusinessService : IBaseBusinessService
{
    public Task UpdateUserAsync(Guid userId, UpdateUserRequest request, CancellationToken cancellationToken);
    public Task UpdateUserVisibilityAsync(Guid userId, VisibilityType visibility, CancellationToken cancellationToken);
    public Task DeleteUsersAsync(List<Guid> userIds, CancellationToken cancellationToken);
    public Task ChangeSubscriptionStatusAsync(Guid subscriberId, Guid targetId, CancellationToken cancellationToken);
    public Task<List<SubscriptionDto>> GetSubscriptionsByUserIdAsync(Guid userId, CancellationToken cancellationToken);
    public Task<List<SubscriptionDto>> GetSubscribersByUserIdAsync(Guid userId, CancellationToken cancellationToken);
    public Task<List<UserDto>> GetAllUsersAsync(CancellationToken cancellationToken);
}