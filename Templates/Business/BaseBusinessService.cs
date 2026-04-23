using Templates.Business.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Templates.Business;

public class BaseBusinessService : IBaseBusinessService
{
    public BaseBusinessService(IHttpContextAccessor httpContextAccessor)
    {
        UserId = GetCurrentUserId(httpContextAccessor);
    }

    public Guid UserId { get; set; }

    public Guid GetCurrentUserId(IHttpContextAccessor httpContextAccessor)
    {
        var claim = httpContextAccessor.HttpContext?.User?.FindFirst("id")?.Value;
        return Guid.TryParse(claim, out var id) ? id : Guid.Empty;
    }
}