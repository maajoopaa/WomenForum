using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WomenForum.Business.Interfaces;
using WomenForum.Models;
using WomenForum.Models.Requests;

namespace WomenForum.Controllers;

[ApiController]
[Route("posts")]
[Produces("application/json")]
public class PostsController : ControllerBase
{
    private readonly IPostsBusinessService _postsBusinessService;
    private readonly ICommentsBusinessService _commentsBusinessService;
    private readonly ILikesBusinessService _likesBusinessService;

    public PostsController(
        IPostsBusinessService postsBusinessService,
        ICommentsBusinessService commentsBusinessService,
        ILikesBusinessService likesBusinessService)
    {
        _postsBusinessService = postsBusinessService;
        _commentsBusinessService = commentsBusinessService;
        _likesBusinessService = likesBusinessService;
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<PostDto>> AddAsync([FromBody] CreatePostRequest request, CancellationToken cancellationToken)
    {
        var result = await _postsBusinessService.AddPostAsync(request, cancellationToken);

        return Ok(result);
    }

    [Authorize]
    [HttpPut("{postId:guid}")]
    public async Task<ActionResult> UpdateAsync(Guid postId, [FromBody] UpdatePostRequest request,
        CancellationToken cancellationToken)
    {
        await _postsBusinessService.UpdatePostAsync(postId, request, cancellationToken);

        return NoContent();
    }

    [Authorize]
    [HttpDelete("{postId:guid}")]
    public async Task<ActionResult> DeleteAsync(Guid postId, CancellationToken cancellationToken)
    {
        await _postsBusinessService.DeletePostAsync(postId, cancellationToken);

        return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<List<PostDto>>> GetPostsAsync([FromQuery] string searchQuery, CancellationToken cancellationToken)
    {
        var result = await _postsBusinessService.GetPostsBySearchQueryAsync(searchQuery, cancellationToken);

        return Ok(result);
    }
    
    [Authorize]
    [HttpPost("{postId:guid}/comments")]
    public async Task<ActionResult<CommentDto>> AddCommentAsync(Guid postId, [FromBody] CreateCommentRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _commentsBusinessService.AddCommentAsync(postId,request, cancellationToken);

        return Ok(result);
    }
    
    [Authorize]
    [HttpPut("{postId:guid}/comments/{commentId:guid}")]
    public async Task<ActionResult> UpdateCommentAsync(Guid commentId, [FromBody] UpdateCommentRequest request,
        CancellationToken cancellationToken)
    {
        await _commentsBusinessService.UpdateCommentAsync(commentId,request, cancellationToken);

        return NoContent();
    }
    
    [Authorize]
    [HttpDelete("{postId:guid}/comments/{commentId:guid}")]
    public async Task<ActionResult> DeleteCommentAsync(Guid commentId, CancellationToken cancellationToken)
    {
        await _commentsBusinessService.DeleteCommentAsync(commentId, cancellationToken);

        return NoContent();
    }

    [Authorize]
    [HttpPut("{postId:guid}/likes")]
    public async Task<ActionResult> ChangeLikeStatusAsync(Guid postId, CancellationToken cancellationToken)
    {
        await _likesBusinessService.ChangeLikeStatusAsync(postId, cancellationToken);
        
        return NoContent();
    }
}