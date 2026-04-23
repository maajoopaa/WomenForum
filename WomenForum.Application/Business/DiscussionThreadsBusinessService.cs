using AutoMapper;
using Templates.Business;
using WomenForum.Business.Interfaces;
using WomenForum.Domain.Models;
using WomenForum.Exceptions;
using WomenForum.Models;
using WomenForum.Models.Requests;
using WomenForum.Repository;

namespace WomenForum.Business;

public class DiscussionThreadsBusinessService : BaseBusinessService, IDiscussionThreadsBusinessService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public DiscussionThreadsBusinessService(
        IHttpContextAccessor httpContextAccessor,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger logger) : base(httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
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
        var entity = await _unitOfWork.DiscussionThreadsRepository.GetByIdAsync(threadId, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException($"Тема для обсуждения {threadId} не найдена.");
        }

        entity.DeletedAt = DateTime.UtcNow;

        await _unitOfWork.DiscussionThreadsRepository.UpdateAsync(entity, cancellationToken);

        _logger.LogInformation("DiscussionThread deleted");
    }

    public async Task<List<DiscussionThreadDto>> GetAllDiscussionThreadsAsync(CancellationToken cancellationToken)
    {
        var entities = await _unitOfWork.DiscussionThreadsRepository.GetAsync(null, cancellationToken);
        
        return _mapper.Map<List<DiscussionThreadDto>>(entities);
    }

    public async Task<List<DiscussionThreadDto>> GetDiscussionThreadsBySearchQueryAsync(string searchQuery,
        CancellationToken cancellationToken)
    {
        searchQuery = searchQuery.ToLower();
        
        var entities = await _unitOfWork.DiscussionThreadsRepository.GetAsync(x => 
            x.Title.Contains(searchQuery) || x.Description.Contains(searchQuery),cancellationToken);
        
        return _mapper.Map<List<DiscussionThreadDto>>(entities);
    }

    public async Task<List<DiscussionThreadDto>> GetPopularDiscussionThreadsAsync(CancellationToken cancellationToken)
    {
        var entities = await _unitOfWork.DiscussionThreadsRepository.GetAsync(null, cancellationToken);
        
        entities = entities.OrderByDescending(x => x.Messages.Count).ToList();
        
        return _mapper.Map<List<DiscussionThreadDto>>(entities);
    }

    public async Task<List<DiscussionThreadDto>> GetRecentDiscussionThreadsAsync(CancellationToken cancellationToken)
    {
        var entities = await _unitOfWork.DiscussionThreadsRepository.GetAsync(null, cancellationToken);
        
        entities = entities.OrderByDescending(x => x.CreatedAt).ToList();
        
        return _mapper.Map<List<DiscussionThreadDto>>(entities);
    }
}