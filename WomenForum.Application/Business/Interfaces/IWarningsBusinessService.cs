using Templates.Business.Interfaces;
using Templates.Models;
using WomenForum.Models;
using WomenForum.Models.Requests;

namespace WomenForum.Business.Interfaces;

public interface IWarningsBusinessService : IBaseBusinessService
{
    public Task<WarningDto> AddWarningAsync(CreateWarningRequest request, CancellationToken cancellationToken);
    public Task DeleteWarningAsync(Guid warningId, CancellationToken cancellationToken);
    public Task<PagedResult<WarningDto>> GetWarningsByUserIdAsync(Guid userId, PaginationParameters paginationParameters, CancellationToken cancellationToken);
    public Task ChangeWarningReadStatus(List<Guid> warningIds, CancellationToken cancellationToken);
}