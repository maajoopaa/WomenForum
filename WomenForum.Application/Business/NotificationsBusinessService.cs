using AutoMapper;
using Templates.Business;
using Templates.Models;
using WomenForum.Business.Interfaces;
using WomenForum.Domain.Enums;
using WomenForum.Domain.Models;
using WomenForum.Exceptions;
using WomenForum.Models;
using WomenForum.Models.Requests;
using WomenForum.Repository;

namespace WomenForum.Business;

public class NotificationsBusinessService : BaseBusinessService, INotificationsBusinessService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<NotificationsBusinessService> _logger;

    public NotificationsBusinessService(
        IHttpContextAccessor httpContextAccessor,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<NotificationsBusinessService> logger) : base(httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PagedResult<NotificationDto>> GetMyNotificationsAsync(PaginationParameters paginationParameters, CancellationToken cancellationToken)
    {
        var pagedEntities = await _unitOfWork.NotificationsRepository.GetPagedAsync(
            x => x.ReceiverId == UserId, 
            paginationParameters.PageNumber, 
            paginationParameters.PageSize, 
            cancellationToken);

        // Map and order them (could also order in repo if GetPagedAsync supports it, but typical default is implicit)
        var orderedItems = pagedEntities.Items.OrderByDescending(x => x.CreatedAt).ToList();

        return new PagedResult<NotificationDto>(
            _mapper.Map<List<NotificationDto>>(orderedItems, opts => 
                opts.Items["CurrentUserId"] = UserId),
            pagedEntities.TotalCount,
            pagedEntities.PageNumber,
            pagedEntities.PageSize
        );
    }

    public async Task SendNotificationAsync(Guid receiverId, NotificationType type, string message, NotificationSource source, Guid? targetId, Guid? triggeredById, CancellationToken cancellationToken)
    {
        if (receiverId == triggeredById)
        {
            // Do not notify if the user triggered the action themselves (e.g., liked own post)
            return;
        }

        var notification = new Notification
        {
            ReceiverId = receiverId,
            Type = type,
            Message = message,
            Source = source,
            TargetId = targetId,
            TriggeredById = triggeredById
        };

        await _unitOfWork.NotificationsRepository.AddAsync(notification, cancellationToken);
        _logger.LogInformation($"Notification sent to user {receiverId}");
    }

    public async Task SendGlobalNotificationAsync(CreateGlobalNotificationRequest request, CancellationToken cancellationToken)
    {
        var currentUser = await _unitOfWork.UsersRepository.GetByIdAsync(UserId,cancellationToken);

        if (currentUser?.Role != Role.Administrator)
        {
            throw new NoPermissionException("У вас недостаточно прав для этого действия.");
        }
        
        var allUsers = await _unitOfWork.UsersRepository.GetAsync(x => x.DeletedAt == null, cancellationToken);
        
        var notifications = allUsers.Select(user => new Notification
        {
            ReceiverId = user.Id,
            Type = request.Type,
            Message = request.Message,
            TriggeredById = UserId
        }).ToList();

        foreach (var notification in notifications)
        {
            await _unitOfWork.NotificationsRepository.AddAsync(notification, cancellationToken);
        }
        
        _logger.LogInformation($"Global notification sent to {notifications.Count} users");
    }

    public async Task MarkAsReadAsync(Guid notificationId, CancellationToken cancellationToken)
    {
        var notification = await _unitOfWork.NotificationsRepository.GetByIdAsync(notificationId, cancellationToken);

        if (notification == null)
        {
            throw new NotFoundException($"Уведомление {notificationId} не найдено.");
        }

        if (notification.ReceiverId != UserId)
        {
            throw new NoPermissionException("У вас недостаточно прав для этого действия.");
        }

        if (notification.ReadAt == null)
        {
            notification.ReadAt = DateTime.UtcNow;
            await _unitOfWork.NotificationsRepository.UpdateAsync(notification, cancellationToken);
        }
    }
}
