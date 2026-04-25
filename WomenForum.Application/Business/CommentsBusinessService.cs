using AutoMapper;
using Templates.Business;
using WomenForum.Business.Interfaces;
using WomenForum.Domain.Models;
using WomenForum.Exceptions;
using WomenForum.Models;
using WomenForum.Models.Requests;
using WomenForum.Repository;

namespace WomenForum.Business;

public class CommentsBusinessService : BaseBusinessService, ICommentsBusinessService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CommentsBusinessService> _logger;

    public CommentsBusinessService(
        IHttpContextAccessor httpContextAccessor,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<CommentsBusinessService> logger) : base(httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<CommentDto> AddCommentAsync(Guid postId, CreateCommentRequest request, CancellationToken cancellationToken)
    {
        var post = await _unitOfWork.PostsRepository.GetByIdAsync(postId, cancellationToken);

        if (post == null)
        {
            throw new NotFoundException($"Поста {postId} не найдено.");
        }
        
        var comment = _mapper.Map<Comment>(request);
        
        comment.PostId = postId;
        comment.CreatedById = UserId;
        
        await _unitOfWork.CommentsRepository.AddAsync(comment, cancellationToken);
        
        _logger.LogInformation("Comment successfully created");
        
        return _mapper.Map<CommentDto>(comment);
    }

    public async Task UpdateCommentAsync(Guid commentId, UpdateCommentRequest request, CancellationToken cancellationToken)
    {
        var entity = await  _unitOfWork.CommentsRepository.GetByIdAsync(commentId, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException($"Комментария {commentId} не найдено.");
        }

        entity.Content = request.Content;
        
        await _unitOfWork.CommentsRepository.UpdateAsync(entity, cancellationToken);
        
        _logger.LogInformation("Comment successfully updated");
    }

    public async Task DeleteCommentAsync(Guid commentId, CancellationToken cancellationToken)
    {
        var entity = await  _unitOfWork.CommentsRepository.GetByIdAsync(commentId, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException($"Комментария {commentId} не найдено.");
        }

        await _unitOfWork.CommentsRepository.DeleteAsync(entity, cancellationToken);
        
        _logger.LogInformation("Comment successfully deleted");
    }
}