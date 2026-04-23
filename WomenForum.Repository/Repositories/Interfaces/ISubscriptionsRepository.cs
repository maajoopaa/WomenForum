using Templates.Repositories.Interfaces;
using WomenForum.Domain.Models;

namespace WomenForum.Repository.Repositories.Interfaces;

public interface ISubscriptionsRepository :  IBaseRepository<Subscription>
{
    public Task<Subscription?> GetBySubscriberAndTargetIds(Guid subscriberId, Guid targetId, CancellationToken cancellationToken);
}