using Templates.Business;
using WomenForum.Business.Interfaces;
using WomenForum.Exceptions;
using WomenForum.Models.Requests;
using WomenForum.Repository;

namespace WomenForum.Business;

public class UserSettingsBusinessService : BaseBusinessService, IUserSettingsBusinessService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UserSettingsBusinessService> _logger;

    public UserSettingsBusinessService(
        IHttpContextAccessor httpContextAccessor,
        IUnitOfWork unitOfWork,
        ILogger<UserSettingsBusinessService> logger) : base(httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task UpdateUserSettingsAsync(Guid userId, UpdateUserSettingsRequest request, CancellationToken cancellationToken)
    {
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