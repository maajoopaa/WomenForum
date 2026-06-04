using AutoMapper;
using Templates.Business;
using Templates.Models;
using WomenForum.Business.Interfaces;
using WomenForum.Domain.Enums;
using WomenForum.Domain.Models;
using WomenForum.Exceptions;
using WomenForum.Helpers.Interfaces;
using WomenForum.Models;
using WomenForum.Models.Requests;
using WomenForum.Repository;

namespace WomenForum.Business;

public class UsersBusinessService : BaseBusinessService, IUsersBusinessService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<UsersBusinessService> _logger;
    private readonly IPermissionsService _permissionsService;

    public UsersBusinessService(
        IHttpContextAccessor httpContextAccessor,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<UsersBusinessService> logger,
        IPermissionsService permissionsService) : base(httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
        _permissionsService = permissionsService;
    }

    public async Task UpdateUserAsync(Guid userId, UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var permissions =
            await _permissionsService.GetUserPermissionsAsync("users", UserId, userId, cancellationToken);

        if (!permissions.Contains(PermissionTypes.Write))
        {
            throw new NoPermissionException("У вас недостаточно прав для этого действия.");
        }
        
        var entity = await _unitOfWork.UsersRepository.GetByIdAsync(userId, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException($"Пользователь {userId} не найден.");
        }
        
        entity.FirstName = request.FirstName;
        entity.LastName = request.LastName;
        entity.Email = request.Email;
        entity.Avatar = request.Avatar;
        entity.BirthDate = request.BirthDate;
        entity.Username = request.Username;
        
        await _unitOfWork.UsersRepository.UpdateAsync(entity, cancellationToken);
        
        _logger.LogInformation("User successfully updated {@User}.", entity);
    }

    public async Task UpdateUserVisibilityAsync(VisibilityType visibility, CancellationToken cancellationToken)
    {
        var permissions =
            await _permissionsService.GetUserPermissionsAsync("users", UserId, UserId, cancellationToken);

        if (!permissions.Contains(PermissionTypes.Write))
        {
            throw new NoPermissionException("У вас недостаточно прав для этого действия.");
        }
        
        var entity = await _unitOfWork.UsersRepository.GetByIdAsync(UserId, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException($"Пользователь {UserId} не найден.");
        }
        
        entity.Visibility = visibility;
        
        await _unitOfWork.UsersRepository.UpdateAsync(entity, cancellationToken);
        
    }

    public async Task DeleteUsersAsync(List<Guid> userIds, CancellationToken cancellationToken)
    {
        var entities = await _unitOfWork.UsersRepository.GetAsync(x =>
            userIds.Contains(x.Id), cancellationToken);

        foreach (var entity in entities)
        {
            var permissions =
                await _permissionsService.GetUserPermissionsAsync("users", UserId, entity.Id, cancellationToken);

            if (!permissions.Contains(PermissionTypes.Delete))
            {
                throw new NoPermissionException("У вас недостаточно прав для этого действия.");
            }
            
            entity.DeletedAt = DateTime.UtcNow;
        }

        await _unitOfWork.UsersRepository.UpdateRangeAsync(entities, cancellationToken);
        
        _logger.LogInformation("Users deleted");
    }

    public async Task ChangeSubscriptionStatusAsync(Guid targetId, CancellationToken cancellationToken)
    {
        var existingSubscription = await _unitOfWork.SubscriptionsRepository.GetBySubscriberAndTargetIds(UserId, targetId, cancellationToken);

        if (existingSubscription == null)
        {
            var subscription = new Subscription
            {
                SubscriberId = UserId,
                TargetUserId = targetId
            };

            await _unitOfWork.SubscriptionsRepository.AddAsync(subscription, cancellationToken);

            _logger.LogInformation("Subscription successfully created {@Subscription}.", subscription);
            
            return;
        }

        await _unitOfWork.SubscriptionsRepository.DeleteAsync(existingSubscription, cancellationToken);
        
        _logger.LogInformation("Subscription successfully deleted {@Subscription}.", existingSubscription);
    }

    public async Task<PagedResult<SubscriptionDto>> GetSubscriptionsByUserIdAsync(Guid userId, PaginationParameters paginationParameters, CancellationToken cancellationToken)
    {
        var permissions =
            await _permissionsService.GetUserPermissionsAsync("users", UserId, userId, cancellationToken);

        if (!permissions.Contains(PermissionTypes.Read))
        {
            throw new NoPermissionException("У вас недостаточно прав для этого действия.");
        }
        
        var pagedEntities = await _unitOfWork.SubscriptionsRepository.GetPagedAsync(x =>
            x.SubscriberId == userId, paginationParameters.PageNumber, paginationParameters.PageSize, cancellationToken);

        return new PagedResult<SubscriptionDto>(
            _mapper.Map<List<SubscriptionDto>>(pagedEntities.Items, opts => 
                opts.Items["CurrentUserId"] = UserId),
            pagedEntities.TotalCount,
            pagedEntities.PageNumber,
            pagedEntities.PageSize
        );
    }

    public async Task<PagedResult<SubscriptionDto>> GetSubscribersByUserIdAsync(Guid userId, PaginationParameters paginationParameters, CancellationToken cancellationToken)
    {
        var permissions =
            await _permissionsService.GetUserPermissionsAsync("users", UserId, userId, cancellationToken);

        if (!permissions.Contains(PermissionTypes.Read))
        {
            throw new NoPermissionException("У вас недостаточно прав для этого действия.");
        }
        
        var pagedEntities = await _unitOfWork.SubscriptionsRepository.GetPagedAsync(x =>
            x.TargetUserId == userId, paginationParameters.PageNumber, paginationParameters.PageSize, cancellationToken);

        return new PagedResult<SubscriptionDto>(
            _mapper.Map<List<SubscriptionDto>>(pagedEntities.Items, opts => 
                opts.Items["CurrentUserId"] = UserId),
            pagedEntities.TotalCount,
            pagedEntities.PageNumber,
            pagedEntities.PageSize
        );
    }

    public async Task<PagedResult<UserDto>> GetAllUsersAsync(PaginationParameters paginationParameters, CancellationToken cancellationToken)
    {
        var pagedEntities = await _unitOfWork.UsersRepository.GetPagedAsync(x => x.DeletedAt == null, paginationParameters.PageNumber, paginationParameters.PageSize, cancellationToken);

        return new PagedResult<UserDto>(
            _mapper.Map<List<UserDto>>(pagedEntities.Items, opts => 
                opts.Items["CurrentUserId"] = UserId),
            pagedEntities.TotalCount,
            pagedEntities.PageNumber,
            pagedEntities.PageSize
        );
    }

    public async Task<UserDto> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.UsersRepository.GetByIdAsync(userId, cancellationToken);
        
        return _mapper.Map<UserDto>(user, opts => 
            opts.Items["CurrentUserId"] = UserId);
    }
}