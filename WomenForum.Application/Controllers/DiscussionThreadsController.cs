using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public async Task<ActionResult<List<DiscussionThreadDto>>> GetAllAsync([FromQuery] string searchQuery,
        CancellationToken cancellationToken)
    {
        var result =
            await _discussionThreadsBusinessService.GetDiscussionThreadsBySearchQueryAsync(searchQuery,
                cancellationToken);

        return Ok(result);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<DiscussionThreadDto>> AddAsync([FromBody] CreateDiscussionThreadRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _discussionThreadsBusinessService.AddDiscussionThreadAsync(request, cancellationToken);

        return Ok(result);
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

        return Ok(result);
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
    public async Task<ActionResult> DeleteMessagesAsync(List<Guid> messageIds, CancellationToken cancellationToken)
    {
        await _messagesBusinessService.DeleteMessagesAsync(messageIds,cancellationToken);

        return NoContent();
    }
    
    [HttpGet("{threadId:guid}/messages")]
    public async Task<ActionResult<List<MessageDto>>> GetMessagesAsync(Guid threadId, CancellationToken cancellationToken)
    {
        var result = await _messagesBusinessService.GetMessagesByDiscussionThreadIdAsync(threadId, cancellationToken);

        return Ok(result);
    }
    
    [Authorize]
    [HttpGet("{threadId:guid}/messages/{messageId:guid}/replies")]
    public async Task<ActionResult<List<MessageDto>>> GetMessageRepliesAsync(Guid messageId, CancellationToken cancellationToken)
    {
        var result = await _messagesBusinessService.GetRepliesAsync(messageId, cancellationToken);

        return Ok(result);
    }
}