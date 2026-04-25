using WomenForum.Models;

namespace WomenForum.Helpers.Interfaces;

public interface IPermissionsService
{
    Task<List<PermissionTypes>> GetUserPermissionsAsync(string resourceType, Guid userId, Guid resourceId, CancellationToken cancellationToken);
}