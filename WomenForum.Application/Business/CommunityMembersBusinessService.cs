using AutoMapper;
using Templates.Business;
using WomenForum.Business.Interfaces;
using WomenForum.Domain.Enums;
using WomenForum.Exceptions;
using WomenForum.Models;
using WomenForum.Repository;

namespace WomenForum.Business;

public class CommunityMembersBusinessService : BaseBusinessService, ICommunityMembersBusinessService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public CommunityMembersBusinessService(
        IHttpContextAccessor httpContextAccessor,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger logger) : base(httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task ChangeBanStatusCommunityMemberAsync(Guid memberId, bool isBanned, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.CommunityMembersRepository.GetByIdAsync(memberId,cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException($"Подписчик {memberId} не найден.");
        }
        
        entity.IsBanned = isBanned;

        await _unitOfWork.CommunityMembersRepository.UpdateAsync(entity, cancellationToken);
        
        _logger.LogInformation("Community member successfully changed.");
    }

    public async Task DeleteCommunityMembersAsync(List<Guid> memberIds, CancellationToken cancellationToken)
    {
        var entities = await _unitOfWork.CommunityMembersRepository.GetAsync(x =>
            memberIds.Contains(x.Id), cancellationToken);
        
        await _unitOfWork.CommunityMembersRepository.DeleteRangeAsync(entities, cancellationToken);
        
        _logger.LogInformation("Community members successfully deleted.");
    }

    public async Task ChangeCommunityMemberRoleAsync(Guid memberId, CommunityRole role, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.CommunityMembersRepository.GetByIdAsync(memberId,cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException($"Подписчик {memberId} не найден.");
        }
        
        entity.Role = role;
        
        await _unitOfWork.CommunityMembersRepository.UpdateAsync(entity, cancellationToken);
        
        _logger.LogInformation("Community member successfully changed.");
    }

    public async Task<List<CommunityMemberDto>> GetCommunityMembersByCommunityIdAsync(Guid communityId, CancellationToken cancellationToken)
    {
        var entities =
            await _unitOfWork.CommunityMembersRepository.GetAsync(x =>
                x.CommunityId == communityId, cancellationToken);
        
        return _mapper.Map<List<CommunityMemberDto>>(entities);
    }
}