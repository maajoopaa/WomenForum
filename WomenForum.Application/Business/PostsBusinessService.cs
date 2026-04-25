using AutoMapper;
using Templates.Business;
using WomenForum.Business.Interfaces;
using WomenForum.Domain.Models;
using WomenForum.Exceptions;
using WomenForum.Helpers.Interfaces;
using WomenForum.Models;
using WomenForum.Models.Requests;
using WomenForum.Repository;

namespace WomenForum.Business;

public class PostsBusinessService : BaseBusinessService, IPostsBusinessService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<PostsBusinessService> _logger;
    private readonly IPermissionsService _permissionsService;

    public PostsBusinessService(
        IHttpContextAccessor httpContextAccessor,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<PostsBusinessService> logger,
        IPermissionsService permissionsService) : base(httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
        _permissionsService = permissionsService;
    }

    public async Task<PostDto> AddPostAsync(CreatePostRequest request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Post>(request);

        if (entity.CommunityId == null)
        {
            entity.AuthorUserId = UserId;

            await _unitOfWork.PostsRepository.AddAsync(entity, cancellationToken);
            
            _logger.LogInformation("Post created");
            
            return _mapper.Map<PostDto>(entity);
        }
        
        var community = await _unitOfWork.CommunitiesRepository.GetByIdAsync(entity.CommunityId.Value, cancellationToken);

        if (community == null)
        {
            throw new NotFoundException($"Сообщество {entity.CommunityId} не найдено.");
        }
        
        await _unitOfWork.PostsRepository.AddAsync(entity, cancellationToken);
        
        _logger.LogInformation("Post created");
        
        return _mapper.Map<PostDto>(entity);
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
        
        _logger.LogInformation("Post deleted");
    }

    public async Task<List<PostDto>> GetPostsByCommunityIdAsync(Guid communityId, CancellationToken cancellationToken)
    {
        var permissions =
            await _permissionsService.GetUserPermissionsAsync("communities", UserId, communityId, cancellationToken);

        if (!permissions.Contains(PermissionTypes.Read))
        {
            throw new NoPermissionException("У вас недостаточно прав для этого действия.");
        }
        
        var entities = await _unitOfWork.PostsRepository.GetAsync(x =>
            x.CommunityId == communityId, cancellationToken);

        return _mapper.Map<List<PostDto>>(entities);
    }

    public async Task<List<PostDto>> GetPostsByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var permissions =
            await _permissionsService.GetUserPermissionsAsync("users", UserId, userId, cancellationToken);

        if (!permissions.Contains(PermissionTypes.Read))
        {
            throw new NoPermissionException("У вас недостаточно прав для этого действия.");
        }
        
        var entities = await _unitOfWork.PostsRepository.GetAsync(x =>
            x.AuthorUserId == userId, cancellationToken);

        return _mapper.Map<List<PostDto>>(entities);
    }

    public async Task<List<PostDto>> GetPopularPostsAsync(CancellationToken cancellationToken)
    {
        var entities = await _unitOfWork.PostsRepository.GetAsync(null, cancellationToken);
        
        entities = entities.OrderByDescending(x => x.Likes.Count + x.Comments.Count).ToList();

        return _mapper.Map<List<PostDto>>(entities);
    }

    public async Task<List<PostDto>> GetRecentPostsAsync(CancellationToken cancellationToken)
    {
        var entities = await _unitOfWork.PostsRepository.GetAsync(null, cancellationToken);
        
        entities = entities.OrderByDescending(x => x.CreatedAt).ToList();

        return _mapper.Map<List<PostDto>>(entities);
    }

    public async Task<List<PostDto>> GetPostsBySearchQueryAsync(string query, CancellationToken cancellationToken)
    {
        query = query.ToLower();
        
        var entities = await _unitOfWork.PostsRepository.GetAsync(x =>
            x.Title.Contains(query), cancellationToken);

        return _mapper.Map<List<PostDto>>(entities);
    }
}