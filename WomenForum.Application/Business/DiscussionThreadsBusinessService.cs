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

namespace WomenForum.Business;

public class DiscussionThreadsBusinessService : BaseBusinessService, IDiscussionThreadsBusinessService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<DiscussionThreadsBusinessService> _logger;
    private readonly IPermissionsService _permissionsService;

    public DiscussionThreadsBusinessService(
        IHttpContextAccessor httpContextAccessor,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<DiscussionThreadsBusinessService> logger,
        IPermissionsService permissionsService) : base(httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
        _permissionsService = permissionsService;
    }

    public async Task<DiscussionThreadDto> AddDiscussionThreadAsync(CreateDiscussionThreadRequest request,
        CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<DiscussionThread>(request);
        entity.CreatedById = UserId;

        await _unitOfWork.DiscussionThreadsRepository.AddAsync(entity, cancellationToken);

        _logger.LogInformation("DiscussionThread added");

        return _mapper.Map<DiscussionThreadDto>(entity);
    }

    public async Task UpdateDiscussionThreadAsync(Guid discussionThreadId, UpdateDiscussionThreadRequest request,
        CancellationToken cancellationToken)
    {
        var permissions =
            await _permissionsService.GetUserPermissionsAsync("threads", UserId, discussionThreadId, cancellationToken);

        if (!permissions.Contains(PermissionTypes.Write))
        {
            throw new NoPermissionException("У вас недостаточно прав для этого действия.");
        }
        
        var entity = await _unitOfWork.DiscussionThreadsRepository.GetByIdAsync(discussionThreadId, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException($"Тема для обсуждения {discussionThreadId} не найдена.");
        }

        entity.ChangedAt = DateTime.UtcNow;
        entity.Title = request.Title;
        entity.Description = request.Description;

        await _unitOfWork.DiscussionThreadsRepository.UpdateAsync(entity, cancellationToken);

        _logger.LogInformation("DiscussionThread updated");
    }

    public async Task DeleteDiscussionThreadAsync(Guid threadId, CancellationToken cancellationToken)
    {
        var permissions =
            await _permissionsService.GetUserPermissionsAsync("threads", UserId, threadId, cancellationToken);

        if (!permissions.Contains(PermissionTypes.Delete))
        {
            throw new NoPermissionException("У вас недостаточно прав для этого действия.");
        }
        
        var entity = await _unitOfWork.DiscussionThreadsRepository.GetByIdAsync(threadId, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException($"Тема для обсуждения {threadId} не найдена.");
        }

        entity.DeletedAt = DateTime.UtcNow;

        await _unitOfWork.DiscussionThreadsRepository.UpdateAsync(entity, cancellationToken);

        _logger.LogInformation("DiscussionThread deleted");
    }

    public async Task<PagedResult<DiscussionThreadDto>> GetAllDiscussionThreadsAsync(PaginationParameters paginationParameters, CancellationToken cancellationToken)
    {
        var pagedEntities = await _unitOfWork.DiscussionThreadsRepository.GetPagedAsync(x => x.DeletedAt == null, paginationParameters.PageNumber, paginationParameters.PageSize, cancellationToken);
        
        return new PagedResult<DiscussionThreadDto>(
            _mapper.Map<List<DiscussionThreadDto>>(pagedEntities.Items),
            pagedEntities.TotalCount,
            pagedEntities.PageNumber,
            pagedEntities.PageSize
        );
    }

    public async Task<PagedResult<DiscussionThreadDto>> GetDiscussionThreadsBySearchQueryAsync(string? searchQuery,
        PaginationParameters paginationParameters, CancellationToken cancellationToken)
    {
        var query = searchQuery?.ToLower();
        var pagedEntities = await _unitOfWork.DiscussionThreadsRepository.GetPagedAsync(
            x => string.IsNullOrEmpty(query) || x.Title.ToLower().Contains(query) || x.Description.ToLower().Contains(query),
            paginationParameters.PageNumber,
            paginationParameters.PageSize,
            cancellationToken
        );
        
        return new PagedResult<DiscussionThreadDto>(
            _mapper.Map<List<DiscussionThreadDto>>(pagedEntities.Items),
            pagedEntities.TotalCount,
            pagedEntities.PageNumber,
            pagedEntities.PageSize
        );
    }

    public async Task<PagedResult<DiscussionThreadDto>> GetPopularDiscussionThreadsAsync(PaginationParameters paginationParameters, CancellationToken cancellationToken)
    {
        var entities = await _unitOfWork.DiscussionThreadsRepository.GetAsync(null, cancellationToken);
        
        var sorted = entities.OrderByDescending(x => x.Messages.Count).ToList();
        var totalCount = sorted.Count;
        var paged = sorted
            .Skip((paginationParameters.PageNumber - 1) * paginationParameters.PageSize)
            .Take(paginationParameters.PageSize)
            .ToList();
        
        return new PagedResult<DiscussionThreadDto>(
            _mapper.Map<List<DiscussionThreadDto>>(paged),
            totalCount,
            paginationParameters.PageNumber,
            paginationParameters.PageSize
        );
    }

    public async Task<PagedResult<DiscussionThreadDto>> GetRecentDiscussionThreadsAsync(PaginationParameters paginationParameters, CancellationToken cancellationToken)
    {
        var entities = await _unitOfWork.DiscussionThreadsRepository.GetAsync(null, cancellationToken);
        
        var sorted = entities.OrderByDescending(x => x.CreatedAt).ToList();
        var totalCount = sorted.Count;
        var paged = sorted
            .Skip((paginationParameters.PageNumber - 1) * paginationParameters.PageSize)
            .Take(paginationParameters.PageSize)
            .ToList();
        
        return new PagedResult<DiscussionThreadDto>(
            _mapper.Map<List<DiscussionThreadDto>>(paged),
            totalCount,
            paginationParameters.PageNumber,
            paginationParameters.PageSize
        );
    }
}