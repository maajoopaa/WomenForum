using AutoMapper;
using Templates.Business;
using Templates.Models;
using WomenForum.Business.Interfaces;
using WomenForum.Domain.Models;
using WomenForum.Exceptions;
using WomenForum.Models;
using WomenForum.Models.Requests;
using WomenForum.Repository;

namespace WomenForum.Business;

public class WarningsBusinessService : BaseBusinessService, IWarningsBusinessService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public WarningsBusinessService(
        IHttpContextAccessor httpContextAccessor,
        IUnitOfWork unitOfWork,
        IMapper mapper) : base(httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<WarningDto> AddWarningAsync(CreateWarningRequest request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Warning>(request);
        
        var user = await _unitOfWork.UsersRepository.GetByIdAsync(request.UserId,cancellationToken);

        if (user == null)
        {
            throw new NotFoundException("Пользователь не найдена.");
        }
        
        await _unitOfWork.WarningsRepository.AddAsync(entity, cancellationToken);
        
        return _mapper.Map<WarningDto>(entity, opts => 
            opts.Items["CurrentUserId"] = UserId);
    }

    public async Task DeleteWarningAsync(Guid warningId, CancellationToken cancellationToken)
    {
        var warning = await _unitOfWork.WarningsRepository.GetByIdAsync(warningId, cancellationToken);

        if (warning == null)
        {
            throw new NotFoundException("Предупреждение не найдено.");
        }
        
        warning.DeletedAt = DateTime.UtcNow;

        await _unitOfWork.WarningsRepository.UpdateAsync(warning, cancellationToken);
    }

    public async Task<PagedResult<WarningDto>> GetWarningsByUserIdAsync(Guid userId, PaginationParameters paginationParameters, CancellationToken cancellationToken)
    {
        var entities = await _unitOfWork.WarningsRepository.GetAsync(x => x.DeletedAt == null && x.UserId == userId, cancellationToken);
        
        var totalCount = entities.Count;
        var paged = entities
            .Skip((paginationParameters.PageNumber - 1) * paginationParameters.PageSize)
            .Take(paginationParameters.PageSize)
            .ToList();

        return new PagedResult<WarningDto>(
            _mapper.Map<List<WarningDto>>(paged, opts => 
                opts.Items["CurrentUserId"] = UserId),
            totalCount,
            paginationParameters.PageNumber,
            paginationParameters.PageSize
        );
    }

    public async Task ChangeWarningReadStatus(List<Guid> warningIds, CancellationToken cancellationToken)
    {
        var warnings = new List<Warning>();
        
        foreach (var warningId in warningIds)
        {
            var warning = await _unitOfWork.WarningsRepository.GetByIdAsync(warningId, cancellationToken);
            
            if(warning == null) 
            {
                throw new NotFoundException($"Предупреждение {warningId} не найдено.");
            }
            
            warning.IsRead = true;

            warnings.Add(warning);
        }

        await _unitOfWork.WarningsRepository.UpdateRangeAsync(warnings, cancellationToken);
    }
}