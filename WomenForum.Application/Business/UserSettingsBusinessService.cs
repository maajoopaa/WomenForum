using Templates.Business;
using WomenForum.Business.Interfaces;
using WomenForum.Exceptions;
using WomenForum.Helpers.Interfaces;
using WomenForum.Models;
using WomenForum.Models.Requests;
using WomenForum.Repository;

namespace WomenForum.Business;

public class UserSettingsBusinessService : BaseBusinessService, IUserSettingsBusinessService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UserSettingsBusinessService> _logger;
    private readonly IPermissionsService _permissionsService;

    public UserSettingsBusinessService(
        IHttpContextAccessor httpContextAccessor,
        IUnitOfWork unitOfWork,
        ILogger<UserSettingsBusinessService> logger,
        IPermissionsService permissionsService) : base(httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _permissionsService = permissionsService;
    }

    public async Task UpdateUserSettingsAsync(Guid userId, UpdateUserSettingsRequest request, CancellationToken cancellationToken)
    {
        var permissions =
            await _permissionsService.GetUserPermissionsAsync("users", UserId, userId, cancellationToken);

        if (!permissions.Contains(PermissionTypes.Write))
        {
            throw new NoPermissionException("У вас недостаточно прав для этого действия.");
        }
        
        var user = await _unitOfWork.UsersRepository.GetByIdAsync(userId, cancellationToken);

        if (user == null)
        {
            throw new NotFoundException($"Пользователь {userId} не найден.");
        }

        var settings = user.UserSettings;

        settings.Theme = request.Theme;
        
        await _unitOfWork.UserSettingsRepository.UpdateAsync(settings, cancellationToken);
        
        _logger.LogInformation("User settings updated");
    }
}