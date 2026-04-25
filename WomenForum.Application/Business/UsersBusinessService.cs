using AutoMapper;
using Templates.Business;
using WomenForum.Business.Interfaces;
using WomenForum.Domain.Enums;
using WomenForum.Domain.Models;
using WomenForum.Exceptions;
using WomenForum.Models;
using WomenForum.Models.Requests;
using WomenForum.Repository;

namespace WomenForum.Business;

public class UsersBusinessService : BaseBusinessService, IUsersBusinessService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<UsersBusinessService> _logger;

    public UsersBusinessService(
        IHttpContextAccessor httpContextAccessor,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<UsersBusinessService> logger) : base(httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task UpdateUserAsync(Guid userId, UpdateUserRequest request, CancellationToken cancellationToken)
    {
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

    public async Task UpdateUserVisibilityAsync(Guid userId, VisibilityType visibility, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.UsersRepository.GetByIdAsync(userId, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException($"Пользователь {userId} не найден.");
        }
        
        entity.Visibility = visibility;
        
        await _unitOfWork.UsersRepository.UpdateAsync(entity, cancellationToken);
        
        _logger.LogInformation("User successfully updated {@User}.", entity);
    }

    public async Task DeleteUsersAsync(List<Guid> userIds, CancellationToken cancellationToken)
    {
        var entities = await _unitOfWork.UsersRepository.GetAsync(x =>
            userIds.Contains(x.Id), cancellationToken);

        foreach (var entity in entities)
        {
            entity.DeletedAt = DateTime.UtcNow;
        }

        await _unitOfWork.UsersRepository.UpdateRangeAsync(entities, cancellationToken);
        
        _logger.LogInformation("Users deleted");
    }

    public async Task ChangeSubscriptionStatusAsync(Guid subscriberId, Guid targetId, CancellationToken cancellationToken)
    {
        var existingSubscription = await _unitOfWork.SubscriptionsRepository.GetBySubscriberAndTargetIds(subscriberId, targetId, cancellationToken);

        if (existingSubscription == null)
        {
            var subscription = new Subscription
            {
                SubscriberId = subscriberId,
                TargetUserId = targetId
            };

            await _unitOfWork.SubscriptionsRepository.AddAsync(subscription, cancellationToken);

            _logger.LogInformation("Subscription successfully created {@Subscription}.", subscription);
            
            return;
        }

        await _unitOfWork.SubscriptionsRepository.DeleteAsync(existingSubscription, cancellationToken);
        
        _logger.LogInformation("Subscription successfully deleted {@Subscription}.", existingSubscription);
    }

    public async Task<List<SubscriptionDto>> GetSubscriptionsByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var entities = await _unitOfWork.SubscriptionsRepository.GetAsync(x =>
            x.SubscriberId == userId, cancellationToken);

        return _mapper.Map<List<SubscriptionDto>>(entities);
    }

    public async Task<List<SubscriptionDto>> GetSubscribersByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var entities = await _unitOfWork.SubscriptionsRepository.GetAsync(x =>
            x.TargetUserId == userId, cancellationToken);

        return _mapper.Map<List<SubscriptionDto>>(entities);
    }

    public async Task<List<UserDto>> GetAllUsersAsync(CancellationToken cancellationToken)
    {
        var entities = await _unitOfWork.UsersRepository.GetAsync(null, cancellationToken);

        return _mapper.Map<List<UserDto>>(entities);
    }
}