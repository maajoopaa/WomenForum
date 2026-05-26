using Templates.Business.Interfaces;
using WomenForum.Models;
using WomenForum.Models.Requests;

namespace WomenForum.Business.Interfaces;

public interface IUserSettingsBusinessService : IBaseBusinessService
{
    public Task UpdateUserSettingsAsync(UpdateUserSettingsRequest request, CancellationToken cancellationToken);
}