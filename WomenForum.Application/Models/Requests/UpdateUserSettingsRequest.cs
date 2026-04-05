using WomenForum.Domain.Enums;

namespace WomenForum.Models.Requests;

public class UpdateUserSettingsRequest
{
    public Theme Theme { get; set; }
}