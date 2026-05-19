using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Templates.Models;
using WomenForum.Business.Interfaces;
using WomenForum.Models;
using WomenForum.Models.Requests;

namespace WomenForum.Controllers;

[ApiController]
[Route("threads")]
[Produces("application/json")]
public class DiscussionThreadsController : ControllerBase
{
    private readonly IDiscussionThreadsBusinessService _discussionThreadsBusinessService;
    private readonly IMessagesBusinessService _messagesBusinessService;

    public DiscussionThreadsController(
        IDiscussionThreadsBusinessService  discussionThreadsBusinessService,
        IMessagesBusinessService messagesBusinessService)
    {
        _discussionThreadsBusinessService = discussionThreadsBusinessService;
        _messagesBusinessService = messagesBusinessService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<DiscussionThreadDto>>> GetAllAsync(
        [FromQuery] string? searchQuery,
        [FromQuery] PaginationParameters paginationParameters,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(searchQuery))
        {
            var allResult = await _discussionThreadsBusinessService.GetAllDiscussionThreadsAsync(paginationParameters, cancellationToken);
            return Ok(allResult);
        }
        var result =
            await _discussionThreadsBusinessService.GetDiscussionThreadsBySearchQueryAsync(searchQuery,
                paginationParameters,
                cancellationToken);

        return Ok(result);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<DiscussionThreadDto>> AddAsync([FromBody] CreateDiscussionThreadRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _discussionThreadsBusinessService.AddDiscussionThreadAsync(request, cancellationToken);

        return Created(string.Empty, result);
    }

    [Authorize]
    [HttpPut("{threadId:guid}")]
    public async Task<ActionResult> UpdateAsync(Guid threadId, [FromBody] UpdateDiscussionThreadRequest request,
        CancellationToken cancellationToken)
    {
        await _discussionThreadsBusinessService.UpdateDiscussionThreadAsync(threadId, request, cancellationToken);

        return NoContent();
    }

    [Authorize]
    [HttpDelete("{threadId:guid}")]
    public async Task<ActionResult> DeleteAsync(Guid threadId, CancellationToken cancellationToken)
    {
        await _discussionThreadsBusinessService.DeleteDiscussionThreadAsync(threadId, cancellationToken);

        return NoContent();
    }
    
    [Authorize]
    [HttpPost("{threadId:guid}/messages")]
    public async Task<ActionResult<MessageDto>> AddMessageAsync(Guid threadId, [FromBody] CreateMessageRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _messagesBusinessService.AddMessageAsync(threadId, request, cancellationToken);

        return Created(string.Empty, result);
    }
    
    [Authorize]
    [HttpPut("{threadId:guid}/messages/{messageId:guid}")]
    public async Task<ActionResult> UpdateMessageAsync(Guid messageId, [FromBody] UpdateMessageRequest request,
        CancellationToken cancellationToken)
    {
        await _messagesBusinessService.UpdateMessageAsync(messageId, request, cancellationToken);

        return NoContent();
    }
    
    [Authorize]
    [HttpDelete("{threadId:guid}/messages")]
    public async Task<ActionResult> DeleteMessagesAsync([FromQuery] List<Guid> messageIds, CancellationToken cancellationToken)
    {
        await _messagesBusinessService.DeleteMessagesAsync(messageIds,cancellationToken);

        return NoContent();
    }
    
    [HttpGet("{threadId:guid}/messages")]
    public async Task<ActionResult<PagedResult<MessageDto>>> GetMessagesAsync(Guid threadId, [FromQuery] PaginationParameters paginationParameters, CancellationToken cancellationToken)
    {
        var result = await _messagesBusinessService.GetMessagesByDiscussionThreadIdAsync(threadId, paginationParameters, cancellationToken);

        return Ok(result);
    }
    
    [Authorize]
    [HttpGet("{threadId:guid}/messages/{messageId:guid}/replies")]
    public async Task<ActionResult<PagedResult<MessageDto>>> GetMessageRepliesAsync(Guid messageId, [FromQuery] PaginationParameters paginationParameters, CancellationToken cancellationToken)
    {
        var result = await _messagesBusinessService.GetRepliesAsync(messageId, paginationParameters, cancellationToken);

        return Ok(result);
    }
}