using AutoMapper;
using Templates.Business;
using WomenForum.Business.Interfaces;
using WomenForum.Domain.Models;
using WomenForum.Exceptions;
using WomenForum.Helpers.Interfaces;
using WomenForum.Models;
using WomenForum.Models.Requests;
using WomenForum.Repository;
using WomenForum.Domain.Enums;

namespace WomenForum.Business;

public class CommentsBusinessService : BaseBusinessService, ICommentsBusinessService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CommentsBusinessService> _logger;
    private readonly IPermissionsService _permissionsService;
    private readonly IUserActivitiesBusinessService _userActivitiesBusinessService;
    private readonly INotificationsBusinessService _notificationsBusinessService;

    public CommentsBusinessService(
        IHttpContextAccessor httpContextAccessor,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<CommentsBusinessService> logger,
        IPermissionsService permissionsService,
        IUserActivitiesBusinessService userActivitiesBusinessService,
        INotificationsBusinessService notificationsBusinessService) : base(httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
        _permissionsService = permissionsService;
        _userActivitiesBusinessService = userActivitiesBusinessService;
        _notificationsBusinessService = notificationsBusinessService;
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
        
        await _userActivitiesBusinessService.LogActivityAsync(ActivityType.Comment, "Оставлен комментарий", comment.Id, cancellationToken);
        await _notificationsBusinessService.SendNotificationAsync(post.AuthorUserId, NotificationType.CommentAdded, "Новый комментарий", NotificationSource.Comment, comment.Id, UserId, cancellationToken);

        _logger.LogInformation("Comment successfully created");
        
        return _mapper.Map<CommentDto>(comment, opts => 
            opts.Items["CurrentUserId"] = UserId);
    }

    public async Task UpdateCommentAsync(Guid commentId, UpdateCommentRequest request, CancellationToken cancellationToken)
    {
        var permissions =
            await _permissionsService.GetUserPermissionsAsync("comments", UserId, commentId, cancellationToken);

        if (!permissions.Contains(PermissionTypes.Write))
        {
            throw new NoPermissionException("У вас недостаточно прав для этого действия.");
        }
        
        var entity = await  _unitOfWork.CommentsRepository.GetByIdAsync(commentId, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException($"Комментария {commentId} не найдено.");
        }

        entity.Content = request.Content;
        
        await _unitOfWork.CommentsRepository.UpdateAsync(entity, cancellationToken);
        
        await _userActivitiesBusinessService.LogActivityAsync(ActivityType.Comment, "Обновлен комментарий", entity.Id, cancellationToken);
        
        _logger.LogInformation("Comment successfully updated");
    }

    public async Task DeleteCommentAsync(Guid commentId, CancellationToken cancellationToken)
    {
        var permissions =
            await _permissionsService.GetUserPermissionsAsync("comments", UserId, commentId, cancellationToken);

        if (!permissions.Contains(PermissionTypes.Delete))
        {
            throw new NoPermissionException("У вас недостаточно прав для этого действия.");
        }
        
        var entity = await  _unitOfWork.CommentsRepository.GetByIdAsync(commentId, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException($"Комментария {commentId} не найдено.");
        }

        await _unitOfWork.CommentsRepository.DeleteAsync(entity, cancellationToken);
        
        await _userActivitiesBusinessService.LogActivityAsync(ActivityType.Comment, "Удален комментарий", entity.Id, cancellationToken);
        
        _logger.LogInformation("Comment successfully deleted");
    }
}