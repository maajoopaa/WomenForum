using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Templates.Models;
using WomenForum.Business.Interfaces;
using WomenForum.Models;
using WomenForum.Models.Requests;
using WomenForum.Repository.Repositories.Interfaces;

namespace WomenForum.Controllers;

[ApiController]
[Route("warnings")]
[Produces("application/json")]
public class WarningsController : ControllerBase
{
    private readonly IWarningsBusinessService _warningsBusinessService;

    public WarningsController(IWarningsBusinessService warningsBusinessService)
    {
        _warningsBusinessService = warningsBusinessService;
    }
    
    [Authorize]
    [HttpGet("{userId:guid}")]
    public async Task<ActionResult<PagedResult<WarningDto>>> GetWarningsByUserIdAsync(Guid userId, [FromQuery] PaginationParameters paginationParameters, CancellationToken cancellationToken)
    {
        var result = await _warningsBusinessService.GetWarningsByUserIdAsync(userId, paginationParameters, cancellationToken);

        return Ok(result);
    }

    [Authorize]
    [HttpDelete("{warningId:guid}")]
    public async Task<ActionResult> DeleteAsync(Guid warningId, CancellationToken cancellationToken)
    {
        await _warningsBusinessService.DeleteWarningAsync(warningId, cancellationToken);

        return NoContent();
    }
    
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<CommentDto>> AddWarningAsync([FromBody] CreateWarningRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _warningsBusinessService.AddWarningAsync(request, cancellationToken);

        return Created(string.Empty, result);
    }

    [Authorize]
    [HttpPatch("read")]
    public async Task<ActionResult> ChangeWarningReadStatus([FromQuery]List<Guid> warningIds, CancellationToken cancellationToken)
    {
        await _warningsBusinessService.ChangeWarningReadStatus(warningIds, cancellationToken);

        return NoContent();
    }
}