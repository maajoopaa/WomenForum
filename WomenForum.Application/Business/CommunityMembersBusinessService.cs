using AutoMapper;
using Templates.Business;
using Templates.Models;
using WomenForum.Business.Interfaces;
using WomenForum.Domain.Enums;
using WomenForum.Exceptions;
using WomenForum.Helpers.Interfaces;
using WomenForum.Models;
using WomenForum.Repository;

namespace WomenForum.Business;

public class CommunityMembersBusinessService : BaseBusinessService, ICommunityMembersBusinessService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CommunityMembersBusinessService> _logger;
    private readonly IPermissionsService _permissionsService;

    public CommunityMembersBusinessService(
        IHttpContextAccessor httpContextAccessor,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<CommunityMembersBusinessService> logger,
        IPermissionsService permissionsService) : base(httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
        _permissionsService = permissionsService;
    }

    public async Task ChangeBanStatusCommunityMemberAsync(Guid memberId, bool isBanned, CancellationToken cancellationToken)
    {
        var permissions =
            await _permissionsService.GetUserPermissionsAsync("community-members", UserId, memberId, cancellationToken);

        if (!permissions.Contains(PermissionTypes.Write))
        {
            throw new NoPermissionException("У вас недостаточно прав для этого действия.");
        }
        
        var entity = await _unitOfWork.CommunityMembersRepository.GetByIdAsync(memberId,cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException($"Подписчик {memberId} не найден.");
        }
        
        entity.IsBanned = isBanned;

        await _unitOfWork.CommunityMembersRepository.UpdateAsync(entity, cancellationToken);
        
        _logger.LogInformation("Community member successfully changed.");
    }

    public async Task DeleteCommunityMembersAsync(Guid communityId, List<Guid> memberIds, CancellationToken cancellationToken)
    {
        var entities = await _unitOfWork.CommunityMembersRepository.GetAsync(x =>
            memberIds.Contains(x.UserId) && x.CommunityId == communityId, cancellationToken);

        foreach (var member in entities)
        {
            var permissions =
                await _permissionsService.GetUserPermissionsAsync("community-members", UserId, member.Id, cancellationToken);

            if (!permissions.Contains(PermissionTypes.Delete) && UserId != member.UserId)
            {
                throw new NoPermissionException("У вас недостаточно прав для этого действия.");
            }
        }
        
        await _unitOfWork.CommunityMembersRepository.DeleteRangeAsync(entities, cancellationToken);
        
        _logger.LogInformation("Community members successfully deleted.");
    }

    public async Task ChangeCommunityMemberRoleAsync(Guid memberId, CommunityRole role, CancellationToken cancellationToken)
    {
        var permissions =
            await _permissionsService.GetUserPermissionsAsync("community-members", UserId, memberId, cancellationToken);

        if (!permissions.Contains(PermissionTypes.Write))
        {
            throw new NoPermissionException("У вас недостаточно прав для этого действия.");
        }
        
        var entity = await _unitOfWork.CommunityMembersRepository.GetByIdAsync(memberId,cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException($"Подписчик {memberId} не найден.");
        }
        
        entity.Role = role;
        
        await _unitOfWork.CommunityMembersRepository.UpdateAsync(entity, cancellationToken);
        
        _logger.LogInformation("Community member successfully changed.");
    }

    public async Task<PagedResult<CommunityMemberDto>> GetCommunityMembersByCommunityIdAsync(Guid communityId, PaginationParameters paginationParameters, CancellationToken cancellationToken)
    {
        var permissions =
            await _permissionsService.GetUserPermissionsAsync("communities", UserId, communityId, cancellationToken);

        if (!permissions.Contains(PermissionTypes.Read))
        {
            throw new NoPermissionException("У вас недостаточно прав для этого действия.");
        }
        
        var pagedEntities =
            await _unitOfWork.CommunityMembersRepository.GetPagedAsync(x =>
                x.CommunityId == communityId, paginationParameters.PageNumber, paginationParameters.PageSize, cancellationToken);
        
        return new PagedResult<CommunityMemberDto>(
            _mapper.Map<List<CommunityMemberDto>>(pagedEntities.Items, opts => 
                opts.Items["CurrentUserId"] = UserId),
            pagedEntities.TotalCount,
            pagedEntities.PageNumber,
            pagedEntities.PageSize
        );
    }
}