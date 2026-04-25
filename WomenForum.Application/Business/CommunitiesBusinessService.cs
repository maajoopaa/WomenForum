using AutoMapper;
using Templates.Business;
using WomenForum.Business.Interfaces;
using WomenForum.Domain.Enums;
using WomenForum.Domain.Models;
using WomenForum.Exceptions;
using WomenForum.Models;
using WomenForum.Models.Requests;
using WomenForum.Repository;

namespace WomenForum.Business;

public class CommunitiesBusinessService : BaseBusinessService, ICommunitiesBusinessService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CommunitiesBusinessService> _logger;

    public CommunitiesBusinessService(
        IHttpContextAccessor httpContextAccessor,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<CommunitiesBusinessService> logger) : base(httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<CommunityDto> AddCommunityAsync(CreateCommunityRequest request, CancellationToken cancellationToken)
    {
        var category = await _unitOfWork.CategoriesRepository.GetByIdAsync(request.CategoryId,cancellationToken);

        if (category == null)
        {
            throw new NotFoundException($"Категории {request.CategoryId} не найдено.");
        }
        
        var community = _mapper.Map<Community>(request);

        community.CreatedById = UserId;
        
        await _unitOfWork.CommunitiesRepository.AddAsync(community, cancellationToken);
        
        _logger.LogInformation("Community successfully added");
        
        return _mapper.Map<CommunityDto>(community);
    }

    public async Task UpdateCommunityAsync(Guid communityId, UpdateCommunityRequest request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.CommunitiesRepository.GetByIdAsync(communityId, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException($"Сообщество {communityId} не найдено.");
        }
        
        var category = await _unitOfWork.CategoriesRepository.GetByIdAsync(request.CategoryId, cancellationToken);

        if (category == null)
        {
            throw new NotFoundException($"Категории {request.CategoryId} не найдено.");
        }
        
        entity.Title =  request.Title;
        entity.Description = request.Description;
        entity.CategoryId = request.CategoryId;
        entity.Avatar = request.Avatar;
        entity.ChangedAt = DateTime.UtcNow;
        
        await _unitOfWork.CommunitiesRepository.UpdateAsync(entity, cancellationToken);
        
        _logger.LogInformation("Community successfully updated");
    }

    public async Task DeleteCommunityAsync(Guid communityId, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.CommunitiesRepository.GetByIdAsync(communityId, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException($"Сообщество {communityId} не найдено.");
        }
        
        entity.DeletedAt =  DateTime.UtcNow;

        await _unitOfWork.CommunitiesRepository.UpdateAsync(entity, cancellationToken);
        
        _logger.LogInformation("Community successfully deleted");
    }

    public async Task<List<CommunityDto>> GetAllCommunitiesAsync(CancellationToken cancellationToken)
    {
        var entities = await _unitOfWork.CommunitiesRepository.GetAsync(null, cancellationToken);
        
        return _mapper.Map<List<CommunityDto>>(entities);
    }

    public async Task<List<CommunityDto>> GetCommunitiesByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var entities = await _unitOfWork.CommunitiesRepository.GetAsync(x => x.CreatedById == userId,cancellationToken);
        
        return _mapper.Map<List<CommunityDto>>(entities);
    }

    public async Task<List<CommunityDto>> GetCommunitiesBySearchQueryAsync(string searchQuery, CancellationToken cancellationToken)
    {
        searchQuery = searchQuery.ToLower();
        
        var entities = await _unitOfWork.CommunitiesRepository.GetAsync(x => 
                x.Title.Contains(searchQuery) || x.Description.Contains(searchQuery), cancellationToken);
        
        return _mapper.Map<List<CommunityDto>>(entities);
    }

    public async Task<List<CommunityDto>> GetPopularCommunitiesAsync(CancellationToken cancellationToken)
    {
        var entities = await _unitOfWork.CommunitiesRepository.GetAsync(null,cancellationToken);
        
        entities = entities.OrderByDescending(x => x.Members.Count + x.Posts.Count).ToList();

        return _mapper.Map<List<CommunityDto>>(entities);
    }

    public async Task ChangeCommunityVisibilityAsync(Guid communityId, VisibilityType visibility, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.CommunitiesRepository.GetByIdAsync(communityId, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException($"Сообщество {communityId} не найдено.");
        }
        
        entity.Visibility = visibility;
        
        await _unitOfWork.CommunitiesRepository.UpdateAsync(entity, cancellationToken);
        
        _logger.LogInformation("Community successfully changed");
    }
}