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

public class ReportsBusinessService : BaseBusinessService, IReportsBusinessService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<ReportsBusinessService> _logger;

    public ReportsBusinessService(
        IHttpContextAccessor httpContextAccessor,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<ReportsBusinessService> logger) : base(httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ReportDto> AddReportAsync(CreateReportRequest request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Report>(request);

        if (!request.UserId.HasValue &&
            !request.CommunityId.HasValue &&
            !request.PostId.HasValue &&
            !request.ThreadId.HasValue)
        {
            throw new NotFoundException("Ни одна из целей жалобы не найдена.");
        }
        
        if (request.UserId.HasValue)
        {
            var user = await _unitOfWork.UsersRepository.GetByIdAsync(request.UserId.Value,cancellationToken);

            if (user == null)
            {
                throw new NotFoundException("Цель жалобы не найдена.");
            }
        }
        
        if (request.CommunityId.HasValue)
        {
            var community = await _unitOfWork.CommunitiesRepository.GetByIdAsync(request.CommunityId.Value,cancellationToken);

            if (community == null)
            {
                throw new NotFoundException("Цель жалобы не найдена.");
            }
        }
        
        if (request.PostId.HasValue)
        {
            var post = await _unitOfWork.PostsRepository.GetByIdAsync(request.PostId.Value,cancellationToken);

            if (post == null)
            {
                throw new NotFoundException("Цель жалобы не найдена.");
            }
        }
        
        if (request.ThreadId.HasValue)
        {
            var thread = await _unitOfWork.DiscussionThreadsRepository.GetByIdAsync(request.ThreadId.Value,cancellationToken);

            if (thread == null)
            {
                throw new NotFoundException("Цель жалобы не найдена.");
            }
        }
        
        await _unitOfWork.ReportsRepository.AddAsync(entity, cancellationToken);
        
        _logger.LogInformation("Report created");

        return _mapper.Map<ReportDto>(entity);
    }

    public async Task ChangeReportStatusAsync(Guid reportId, ReportStatus status, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.ReportsRepository.GetByIdAsync(reportId, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException($"Жалоба {reportId} не найдена.");
        }
        
        entity.Status = status;
        
        await _unitOfWork.ReportsRepository.UpdateAsync(entity, cancellationToken);
        
        _logger.LogInformation("Report changed");
    }

    public async Task<List<ReportDto>> GetAllReportsAsync(CancellationToken cancellationToken)
    {
        var entities = await _unitOfWork.ReportsRepository.GetAsync(null, cancellationToken);

        return _mapper.Map<List<ReportDto>>(entities);
    }
}