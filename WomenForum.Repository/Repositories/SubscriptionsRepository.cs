using Microsoft.EntityFrameworkCore;
using Templates.Repositories;
using WomenForum.Database;
using WomenForum.Domain.Models;
using WomenForum.Repository.Repositories.Interfaces;

namespace WomenForum.Repository.Repositories;

public class SubscriptionsRepository(WomenForumDbContext context)
    : BaseRepository<Subscription, WomenForumDbContext>(context), ISubscriptionsRepository
{
    public async Task<Subscription?> GetBySubscriberAndTargetIds(Guid subscriberId, Guid targetId, CancellationToken cancellationToken)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.SubscriberId == subscriberId && x.TargetUserId == targetId,
            cancellationToken);
    }
}