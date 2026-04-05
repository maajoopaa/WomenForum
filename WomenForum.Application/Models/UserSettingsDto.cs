using Templates.Models;
using WomenForum.Domain.Enums;
using WomenForum.Domain.Models;

namespace WomenForum.Models;

public class UserSettingsDto : BaseDtoWithId
{
    public Theme Theme { get; set; }
}