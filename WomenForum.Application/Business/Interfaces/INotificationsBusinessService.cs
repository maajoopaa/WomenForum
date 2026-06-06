using Templates.Business.Interfaces;
using Templates.Models;
using WomenForum.Domain.Enums;
using WomenForum.Models;
using WomenForum.Models.Requests;

namespace WomenForum.Business.Interfaces;

public interface INotificationsBusinessService : IBaseBusinessService
{
    Task<PagedResult<NotificationDto>> GetMyNotificationsAsync(PaginationParameters paginationParameters, CancellationToken cancellationToken);
    
    Task SendNotificationAsync(Guid receiverId, NotificationType type, string message, NotificationSource source, Guid? targetId, Guid? triggeredById, CancellationToken cancellationToken);
    
    Task SendGlobalNotificationAsync(CreateGlobalNotificationRequest request, CancellationToken cancellationToken);

    Task MarkAsReadAsync(Guid notificationId, CancellationToken cancellationToken);
}
