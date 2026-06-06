using AutoMapper;
using Templates.Business;
using Templates.Models;
using WomenForum.Business.Interfaces;
using WomenForum.Domain.Models;
using WomenForum.Exceptions;
using WomenForum.Helpers.Interfaces;
using WomenForum.Models;
using WomenForum.Models.Requests;
using WomenForum.Repository;
using WomenForum.Domain.Enums;

namespace WomenForum.Business;

public class PostsBusinessService : BaseBusinessService, IPostsBusinessService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<PostsBusinessService> _logger;
    private readonly IPermissionsService _permissionsService;
    private readonly IUserActivitiesBusinessService _userActivitiesBusinessService;

    public PostsBusinessService(
        IHttpContextAccessor httpContextAccessor,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<PostsBusinessService> logger,
        IPermissionsService permissionsService,
        IUserActivitiesBusinessService userActivitiesBusinessService) : base(httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
        _permissionsService = permissionsService;
        _userActivitiesBusinessService = userActivitiesBusinessService;
    }

    public async Task<PostDto> AddPostAsync(CreatePostRequest request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Post>(request);
        entity.AuthorUserId = UserId;
        
        if (entity.CommunityId != null)
        {
            var community = await _unitOfWork.CommunitiesRepository.GetByIdAsync(entity.CommunityId.Value, cancellationToken);
            
            if (community == null)
            {
                throw new NotFoundException($"Сообщество {entity.CommunityId} не найдено.");
            }
        }
        
        await _unitOfWork.PostsRepository.AddAsync(entity, cancellationToken);
        
        await _userActivitiesBusinessService.LogActivityAsync(ActivityType.CreatePost, "Создан пост", entity.Id, cancellationToken);
        
        _logger.LogInformation("Post created");
        
        return _mapper.Map<PostDto>(entity, opts => 
            opts.Items["CurrentUserId"] = UserId);
    }

    public async Task UpdatePostAsync(Guid postId, UpdatePostRequest request, CancellationToken cancellationToken)
    {
        var permissions =
            await _permissionsService.GetUserPermissionsAsync("posts", UserId, postId, cancellationToken);

        if (!permissions.Contains(PermissionTypes.Write))
        {
            throw new NoPermissionException("У вас недостаточно прав для этого действия.");
        }
        
        var entity = await _unitOfWork.PostsRepository.GetByIdAsync(postId, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException($"Пост {postId} не найден.");
        }
        
        entity.Title = request.Title;
        entity.HtmlContent = request.HtmlContent;
        entity.ChangedAt = DateTime.UtcNow;
        
        await _unitOfWork.PostsRepository.UpdateAsync(entity, cancellationToken);
        
        _logger.LogInformation("Post updated");
    }

    public async Task DeletePostAsync(Guid postId, CancellationToken cancellationToken)
    {
        var permissions =
            await _permissionsService.GetUserPermissionsAsync("posts", UserId, postId, cancellationToken);

        if (!permissions.Contains(PermissionTypes.Delete))
        {
            throw new NoPermissionException("У вас недостаточно прав для этого действия.");
        }
        
        var entity = await _unitOfWork.PostsRepository.GetByIdAsync(postId, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException($"Пост {postId} не найден.");
        }
        
        entity.DeletedAt = DateTime.UtcNow;
        
        await _unitOfWork.PostsRepository.UpdateAsync(entity, cancellationToken);
        
        await _userActivitiesBusinessService.LogActivityAsync(ActivityType.DeletePost, "Пост удален", entity.Id, cancellationToken);
        
        _logger.LogInformation("Post deleted");
    }

    public async Task<PagedResult<PostDto>> GetPostsByCommunityIdAsync(Guid communityId, PaginationParameters paginationParameters, CancellationToken cancellationToken)
    {
        var permissions =
            await _permissionsService.GetUserPermissionsAsync("communities", UserId, communityId, cancellationToken);

        if (!permissions.Contains(PermissionTypes.Read))
        {
            throw new NoPermissionException("У вас недостаточно прав для этого действия.");
        }
        
        var pagedEntities = await _unitOfWork.PostsRepository.GetPagedAsync(x =>
            x.CommunityId == communityId && x.DeletedAt == null, paginationParameters.PageNumber, paginationParameters.PageSize, cancellationToken);

        return new PagedResult<PostDto>(
            _mapper.Map<List<PostDto>>(pagedEntities.Items, opts => 
                opts.Items["CurrentUserId"] = UserId),
            pagedEntities.TotalCount,
            pagedEntities.PageNumber,
            pagedEntities.PageSize
        );
    }

    public async Task<PagedResult<PostDto>> GetPostsByUserIdAsync(Guid userId, PaginationParameters paginationParameters, CancellationToken cancellationToken)
    {
        var permissions =
            await _permissionsService.GetUserPermissionsAsync("users", UserId, userId, cancellationToken);

        if (!permissions.Contains(PermissionTypes.Read))
        {
            throw new NoPermissionException("У вас недостаточно прав для этого действия.");
        }
        
        var pagedEntities = await _unitOfWork.PostsRepository.GetPagedAsync(x =>
            x.AuthorUserId == userId && x.DeletedAt == null, paginationParameters.PageNumber, paginationParameters.PageSize, cancellationToken);

        return new PagedResult<PostDto>(
            _mapper.Map<List<PostDto>>(pagedEntities.Items, opts => 
                opts.Items["CurrentUserId"] = UserId),
            pagedEntities.TotalCount,
            pagedEntities.PageNumber,
            pagedEntities.PageSize
        );
    }

    public async Task<PagedResult<PostDto>> GetPopularPostsAsync(PaginationParameters paginationParameters, CancellationToken cancellationToken)
    {
        var entities = await _unitOfWork.PostsRepository.GetAsync(x => x.DeletedAt == null, cancellationToken);
        
        var sorted = entities.OrderByDescending(x => x.Likes.Count + x.Comments.Count).ToList();
        var totalCount = sorted.Count;
        var paged = sorted
            .Skip((paginationParameters.PageNumber - 1) * paginationParameters.PageSize)
            .Take(paginationParameters.PageSize)
            .ToList();

        return new PagedResult<PostDto>(
            _mapper.Map<List<PostDto>>(paged, opts => 
                opts.Items["CurrentUserId"] = UserId),
            totalCount,
            paginationParameters.PageNumber,
            paginationParameters.PageSize
        );
    }

    public async Task<PagedResult<PostDto>> GetRecentPostsAsync(PaginationParameters paginationParameters, CancellationToken cancellationToken)
    {
        var entities = await _unitOfWork.PostsRepository.GetAsync(x => x.DeletedAt == null, cancellationToken);
        
        var sorted = entities.OrderByDescending(x => x.CreatedAt).ToList();
        var totalCount = sorted.Count;
        var paged = sorted
            .Skip((paginationParameters.PageNumber - 1) * paginationParameters.PageSize)
            .Take(paginationParameters.PageSize)
            .ToList();

        return new PagedResult<PostDto>(
            _mapper.Map<List<PostDto>>(paged, opts => 
                                                              opts.Items["CurrentUserId"] = UserId),
            totalCount,
            paginationParameters.PageNumber,
            paginationParameters.PageSize
        );
    }

    public async Task<PagedResult<PostDto>> GetPostsBySearchQueryAsync(string? query, PaginationParameters paginationParameters, CancellationToken cancellationToken)
    {
        var searchQuery = query?.ToLower();
        var pagedEntities = await _unitOfWork.PostsRepository.GetPagedAsync(x =>
            (string.IsNullOrEmpty(searchQuery) || x.Title.ToLower().Contains(searchQuery)) && x.DeletedAt == null, paginationParameters.PageNumber, paginationParameters.PageSize, cancellationToken);

        return new PagedResult<PostDto>(
            _mapper.Map<List<PostDto>>(pagedEntities.Items, opts => 
                opts.Items["CurrentUserId"] = UserId),
            pagedEntities.TotalCount,
            pagedEntities.PageNumber,
            pagedEntities.PageSize
        );
    }
}