using System.ComponentModel.DataAnnotations;
using WomenForum.Domain.Enums;

namespace WomenForum.Models.Requests;

public class CreateGlobalNotificationRequest
{
    [Required]
    public string Message { get; set; } = null!;

    [Required]
    public NotificationType Type { get; set; }
}
