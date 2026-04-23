using Templates.Business.Interfaces;
using WomenForum.Domain.Enums;
using WomenForum.Models;
using WomenForum.Models.Requests;

namespace WomenForum.Business.Interfaces;

public interface IReportsBusinessService : IBaseBusinessService
{
    public Task<ReportDto> AddReportAsync(CreateReportRequest request, CancellationToken cancellationToken);
    public Task ChangeReportStatusAsync(Guid reportId, ReportStatus status, CancellationToken cancellationToken);
    public Task<List<ReportDto>> GetAllReportsAsync(CancellationToken cancellationToken);
}