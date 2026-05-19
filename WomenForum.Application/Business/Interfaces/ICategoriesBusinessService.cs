using Templates.Business.Interfaces;
using Templates.Models;
using WomenForum.Models;
using WomenForum.Models.Requests;

namespace WomenForum.Business.Interfaces;

public interface ICategoriesBusinessService : IBaseBusinessService
{
    public Task<CategoryDto> AddCategoryAsync(CreateCategoryRequest request, CancellationToken cancellationToken);
    public Task<PagedResult<CategoryDto>> GetAllCategoriesAsync(PaginationParameters paginationParameters, CancellationToken cancellationToken);
    public Task UpdateCategoryAsync(Guid categoryId, UpdateCategoryRequest request, CancellationToken cancellationToken);
}