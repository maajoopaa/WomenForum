using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Templates.Models;
using WomenForum.Business.Interfaces;
using WomenForum.Models;
using WomenForum.Models.Requests;

namespace WomenForum.Controllers;

[ApiController]
[Route("notifications")]
[Produces("application/json")]
public class NotificationsController : ControllerBase
{
    private readonly INotificationsBusinessService _notificationsBusinessService;

    public NotificationsController(INotificationsBusinessService notificationsBusinessService)
    {
        _notificationsBusinessService = notificationsBusinessService;
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<PagedResult<NotificationDto>>> GetNotificationsAsync([FromQuery] PaginationParameters paginationParameters, CancellationToken cancellationToken)
    {
        var result = await _notificationsBusinessService.GetMyNotificationsAsync(paginationParameters, cancellationToken);
        return Ok(result);
    }

    [Authorize]
    [HttpPut("{notificationId:guid}/read")]
    public async Task<ActionResult> MarkAsReadAsync(Guid notificationId, CancellationToken cancellationToken)
    {
        await _notificationsBusinessService.MarkAsReadAsync(notificationId, cancellationToken);
        return NoContent();
    }

    [Authorize]
    [HttpPost("global")]
    public async Task<ActionResult> SendGlobalNotificationAsync([FromBody] CreateGlobalNotificationRequest request, CancellationToken cancellationToken)
    {
        await _notificationsBusinessService.SendGlobalNotificationAsync(request, cancellationToken);
        return Ok();
    }
}
