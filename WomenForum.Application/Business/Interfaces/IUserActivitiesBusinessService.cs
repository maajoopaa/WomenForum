using Templates.Business.Interfaces;
using Templates.Models;
using WomenForum.Domain.Enums;
using WomenForum.Models;

namespace WomenForum.Business.Interfaces;

public interface IUserActivitiesBusinessService : IBaseBusinessService
{
    Task<PagedResult<UserActivityDto>> GetUserActivitiesByIdAsync(Guid userId, PaginationParameters paginationParameters, CancellationToken cancellationToken);
    
    Task LogActivityAsync(ActivityType type, string description, Guid? targetId, CancellationToken cancellationToken);
}
