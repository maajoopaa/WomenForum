using AutoMapper;
using Templates.Business;
using WomenForum.Business.Interfaces;
using WomenForum.Domain.Enums;
using WomenForum.Domain.Models;
using WomenForum.Exceptions;
using WomenForum.Models;
using WomenForum.Repository;

namespace WomenForum.Business;

public class CommunityJoinRequestsBusinessService : BaseBusinessService, ICommunityJoinRequestsBusinessService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CommunityJoinRequestsBusinessService> _logger;

    public CommunityJoinRequestsBusinessService(
        IHttpContextAccessor httpContextAccessor,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<CommunityJoinRequestsBusinessService> logger) : base(httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task JoinCommunityAsync(Guid communityId, CancellationToken cancellationToken)
    {
        var community = await _unitOfWork.CommunitiesRepository.GetByIdAsync(communityId, cancellationToken);

        if (community == null)
        {
            throw new NotFoundException($"Сообщество {communityId} не найдено.");
        }

        if (community.Visibility == VisibilityType.Public)
        {
            var communityMember = new CommunityMember
            {
                UserId = UserId,
                CommunityId = communityId,
            };
            
            await _unitOfWork.CommunityMembersRepository.AddAsync(communityMember, cancellationToken);
            
            _logger.LogInformation("Community member successfully joined.");

            return;
        }

        var communityJoinRequest = new CommunityJoinRequest
        {
            CommunityId = communityId,
            UserId = UserId,
        };

        await _unitOfWork.CommunityJoinRequestsRepository.AddAsync(communityJoinRequest, cancellationToken);
        
        _logger.LogInformation("Community join request successfully sent.");
    }

    public async Task<List<CommunityJoinRequestDto>> GetJoinRequestsByCommunityIdAsync(Guid communityId, CancellationToken cancellationToken)
    {
        var entities = await _unitOfWork.CommunityJoinRequestsRepository.GetAsync(x =>
            x.CommunityId == communityId, cancellationToken);
        
        return _mapper.Map<List<CommunityJoinRequestDto>>(entities);
    }

    public async Task ChangeJoinRequestStatusAsync(Guid requestId, JoinRequestStatus status, string? message,
        CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.CommunityJoinRequestsRepository.GetByIdAsync(requestId, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException($"Запрос {requestId} не найден.");
        }
        
        entity.Status = status;
        
        await _unitOfWork.CommunityJoinRequestsRepository.UpdateAsync(entity, cancellationToken);
        
        _logger.LogInformation("Community join request successfully changed.");
    }
}