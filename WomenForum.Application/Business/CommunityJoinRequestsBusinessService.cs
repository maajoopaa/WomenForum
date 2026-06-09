using AutoMapper;
using Templates.Business;
using Templates.Models;
using WomenForum.Business.Interfaces;
using WomenForum.Domain.Enums;
using WomenForum.Domain.Models;
using WomenForum.Exceptions;
using WomenForum.Helpers.Interfaces;
using WomenForum.Models;
using WomenForum.Repository;

namespace WomenForum.Business;

public class CommunityJoinRequestsBusinessService : BaseBusinessService, ICommunityJoinRequestsBusinessService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CommunityJoinRequestsBusinessService> _logger;
    private readonly IPermissionsService _permissionsService;
    private readonly IUserActivitiesBusinessService _userActivitiesBusinessService;
    private readonly INotificationsBusinessService _notificationsBusinessService;

    public CommunityJoinRequestsBusinessService(
        IHttpContextAccessor httpContextAccessor,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<CommunityJoinRequestsBusinessService> logger,
        IPermissionsService permissionsService,
        IUserActivitiesBusinessService userActivitiesBusinessService,
        INotificationsBusinessService notificationsBusinessService) : base(httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
        _permissionsService = permissionsService;
        _userActivitiesBusinessService = userActivitiesBusinessService;
        _notificationsBusinessService = notificationsBusinessService;
    }

    public async Task JoinCommunityAsync(Guid communityId, CancellationToken cancellationToken)
    {
        var community = await _unitOfWork.CommunitiesRepository.GetByIdAsync(communityId, cancellationToken);

        if (community == null)
        {
            throw new NotFoundException($"Сообщество {communityId} не найдено.");
        }

        if (community.Visibility == VisibilityType.Public)
        {
            var communityMember = new CommunityMember
            {
                UserId = UserId,
                CommunityId = communityId,
            };
            
            await _unitOfWork.CommunityMembersRepository.AddAsync(communityMember, cancellationToken);
            
            await _userActivitiesBusinessService.LogActivityAsync(ActivityType.JoinCommunity, "Вступление в сообщество", communityId, cancellationToken);
            
            _logger.LogInformation("Community member successfully joined.");

            return;
        }

        var communityJoinRequest = new CommunityJoinRequest
        {
            CommunityId = communityId,
            UserId = UserId,
        };
        
        var existingJoinRequest = await _unitOfWork.CommunityJoinRequestsRepository
            .FirstOrDefaultAsync(x => x.CommunityId == communityId && x.UserId == UserId, cancellationToken);

        if (existingJoinRequest != null)
        {
            return;
        }

        await _unitOfWork.CommunityJoinRequestsRepository.AddAsync(communityJoinRequest, cancellationToken);
        
        await _userActivitiesBusinessService.LogActivityAsync(ActivityType.SendJoinRequest, "Запрос на вступление в сообщество", communityId, cancellationToken);
        
        _logger.LogInformation("Community join request successfully sent.");
    }

    public async Task<PagedResult<CommunityJoinRequestDto>> GetJoinRequestsByCommunityIdAsync(Guid communityId, PaginationParameters paginationParameters, CancellationToken cancellationToken)
    {
        var permissions =
            await _permissionsService.GetUserPermissionsAsync("communities", UserId, communityId, cancellationToken);

        if (!permissions.Contains(PermissionTypes.Write))
        {
            throw new NoPermissionException("У вас недостаточно прав для этого действия.");
        }
        
        var pagedEntities = await _unitOfWork.CommunityJoinRequestsRepository.GetPagedAsync(x =>
            x.CommunityId == communityId, paginationParameters.PageNumber, paginationParameters.PageSize, cancellationToken);
        
        return new PagedResult<CommunityJoinRequestDto>(
            _mapper.Map<List<CommunityJoinRequestDto>>(pagedEntities.Items, opts => 
                opts.Items["CurrentUserId"] = UserId),
            pagedEntities.TotalCount,
            pagedEntities.PageNumber,
            pagedEntities.PageSize
        );
    }

    public async Task ChangeJoinRequestStatusAsync(Guid requestId, JoinRequestStatus status, string? message,
        CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.CommunityJoinRequestsRepository.GetByIdAsync(requestId, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException($"Запрос {requestId} не найден.");
        }
        
        var permissions =
            await _permissionsService.GetUserPermissionsAsync("communities", UserId, entity.CommunityId, cancellationToken);

        if (!permissions.Contains(PermissionTypes.Write))
        {
            throw new NoPermissionException("У вас недостаточно прав для этого действия.");
        }
        
        entity.Status = status;
        entity.ReviewedById = UserId;
        entity.ReviewedAt = DateTime.UtcNow;
        
        await _unitOfWork.CommunityJoinRequestsRepository.UpdateAsync(entity, cancellationToken);

        if (status == JoinRequestStatus.Approved)
        {
            var member = new CommunityMember
            {
                UserId = entity.UserId,
                CommunityId = entity.CommunityId
            };
            
            await _unitOfWork.CommunityMembersRepository.AddAsync(member,cancellationToken);
        }
        
        await _notificationsBusinessService.SendNotificationAsync(entity.UserId, NotificationType.JoinRequestStatusChanged, $"Статус вашего запроса: {status}", NotificationSource.Community, entity.CommunityId, UserId, cancellationToken);
        
        _logger.LogInformation("Community join request successfully changed.");
    }
}