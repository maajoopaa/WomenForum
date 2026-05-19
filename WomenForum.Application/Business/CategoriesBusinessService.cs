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

public class CategoriesBusinessService : BaseBusinessService,ICategoriesBusinessService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CategoriesBusinessService> _logger;
    private readonly IPermissionsService _permissionsService;

    public CategoriesBusinessService(
        IHttpContextAccessor httpContextAccessor,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<CategoriesBusinessService> logger,
        IPermissionsService permissionsService) : base(httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
        _permissionsService = permissionsService;
    }

    public async Task<CategoryDto> AddCategoryAsync(CreateCategoryRequest request, CancellationToken cancellationToken)
    {
        var permissions =
            await _permissionsService.GetUserPermissionsAsync("categories", UserId, Guid.Empty, cancellationToken);

        if (!permissions.Contains(PermissionTypes.Write))
        {
            throw new NoPermissionException("У вас недостаточно прав для этого действия.");
        }
        
        var entity = _mapper.Map<Category>(request);
        
        await _unitOfWork.CategoriesRepository.AddAsync(entity,cancellationToken);
        
        _logger.LogInformation("Category successfully added");
        
        return _mapper.Map<CategoryDto>(entity);
    }

    public async Task<PagedResult<CategoryDto>> GetAllCategoriesAsync(PaginationParameters paginationParameters, CancellationToken cancellationToken)
    {
        var pagedEntities = await _unitOfWork.CategoriesRepository.GetPagedAsync(null, paginationParameters.PageNumber, paginationParameters.PageSize, cancellationToken);
        
        return new PagedResult<CategoryDto>(
            _mapper.Map<List<CategoryDto>>(pagedEntities.Items),
            pagedEntities.TotalCount,
            pagedEntities.PageNumber,
            pagedEntities.PageSize
        );
    }

    public async Task UpdateCategoryAsync(Guid categoryId, UpdateCategoryRequest request, CancellationToken cancellationToken)
    {
        var permissions =
            await _permissionsService.GetUserPermissionsAsync("categories", UserId, categoryId, cancellationToken);

        if (!permissions.Contains(PermissionTypes.Write))
        {
            throw new NoPermissionException("У вас недостаточно прав для этого действия.");
        }
        
        var entity = await _unitOfWork.CategoriesRepository.GetByIdAsync(categoryId, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException($"Категории {categoryId} не найдено.");
        }
        
        entity.Title = request.Title;
        entity.Description = request.Description;
        entity.Logo = request.Logo;

        await _unitOfWork.CategoriesRepository.UpdateAsync(entity, cancellationToken);
        
        _logger.LogInformation("Category successfully updated");
    }
}