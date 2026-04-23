using Templates.Business.Interfaces;
using WomenForum.Models;

namespace WomenForum.Business.Interfaces;

public interface ILikesBusinessService : IBaseBusinessService
{
    public Task ChangeLikeStatusAsync(Guid postId, CancellationToken cancellationToken);
}