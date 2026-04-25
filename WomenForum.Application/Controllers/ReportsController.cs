using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WomenForum.Business.Interfaces;
using WomenForum.Domain.Enums;
using WomenForum.Models;
using WomenForum.Models.Requests;

namespace WomenForum.Controllers;

[ApiController]
[Route("reports")]
[Produces("application/json")]
public class ReportsController : ControllerBase
{
    private readonly IReportsBusinessService _reportsBusinessService;

    public ReportsController(IReportsBusinessService reportsBusinessService)
    {
        _reportsBusinessService = reportsBusinessService;
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<ReportDto>> AddAsync([FromBody] CreateReportRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _reportsBusinessService.AddReportAsync(request, cancellationToken);

        return Ok(result);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<ReportDto>>> GetAllAsync(CancellationToken cancellationToken)
    {
        var result = await _reportsBusinessService.GetAllReportsAsync(cancellationToken);

        return Ok(result);
    }

    [Authorize]
    [HttpPatch("{reportId:guid}")]
    public async Task<ActionResult> ChangeReportStatusAsync(Guid reportId, [FromQuery] ReportStatus reportStatus,
        CancellationToken cancellationToken)
    {
        await _reportsBusinessService.ChangeReportStatusAsync(reportId, reportStatus, cancellationToken);

        return NoContent();
    }
}