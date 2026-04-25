using AutoMapper;
using Templates.Business;
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
        var entity = _mapper.Map<Category>(request);
        
        await _unitOfWork.CategoriesRepository.AddAsync(entity,cancellationToken);
        
        _logger.LogInformation("Category successfully added");
        
        return _mapper.Map<CategoryDto>(entity);
    }

    public async Task<List<CategoryDto>> GetAllCategoriesAsync(CancellationToken cancellationToken)
    {
        var entities = await _unitOfWork.CategoriesRepository.GetAsync(null, cancellationToken);
        
        return _mapper.Map<List<CategoryDto>>(entities);
    }

    public async Task UpdateCategoryAsync(Guid categoryId, UpdateCategoryRequest request, CancellationToken cancellationToken)
    {
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