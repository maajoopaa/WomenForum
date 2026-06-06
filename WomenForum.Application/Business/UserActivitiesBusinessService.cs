using AutoMapper;
using Templates.Business;
using Templates.Models;
using WomenForum.Business.Interfaces;
using WomenForum.Domain.Enums;
using WomenForum.Domain.Models;
using WomenForum.Exceptions;
using WomenForum.Models;
using WomenForum.Repository;

namespace WomenForum.Business;

public class UserActivitiesBusinessService : BaseBusinessService, IUserActivitiesBusinessService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<UserActivitiesBusinessService> _logger;

    public UserActivitiesBusinessService(
        IHttpContextAccessor httpContextAccessor,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<UserActivitiesBusinessService> logger) : base(httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PagedResult<UserActivityDto>> GetUserActivitiesByIdAsync(Guid userId, PaginationParameters paginationParameters, CancellationToken cancellationToken)
    {
        var currentUser = await _unitOfWork.UsersRepository.GetByIdAsync(UserId,cancellationToken);

        if (currentUser?.Role != Role.Administrator)
        {
            throw new NoPermissionException("У вас недостаточно прав для этого действия.");
        }
        
        var pagedEntities = await _unitOfWork.UserActivitiesRepository.GetPagedAsync(
            x => x.UserId == userId,
            paginationParameters.PageNumber, 
            paginationParameters.PageSize, 
            cancellationToken);

        return new PagedResult<UserActivityDto>(
            _mapper.Map<List<UserActivityDto>>(pagedEntities.Items, opts => 
                opts.Items["CurrentUserId"] = UserId),
            pagedEntities.TotalCount,
            pagedEntities.PageNumber,
            pagedEntities.PageSize
        );
    }

    public async Task LogActivityAsync(ActivityType type, string description, Guid? targetId, CancellationToken cancellationToken)
    {
        var activity = new UserActivity
        {
            UserId = UserId,
            Type = type,
            Description = description,
            TargetId = targetId
        };

        await _unitOfWork.UserActivitiesRepository.AddAsync(activity, cancellationToken);
        _logger.LogInformation($"Activity logged: {type}");
    }
}
