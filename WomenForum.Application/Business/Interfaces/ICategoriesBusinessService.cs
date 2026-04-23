using Templates.Business.Interfaces;
using WomenForum.Models;
using WomenForum.Models.Requests;

namespace WomenForum.Business.Interfaces;

public interface ICategoriesBusinessService : IBaseBusinessService
{
    public Task<CategoryDto> AddCategoryAsync(CreateCategoryRequest request, CancellationToken cancellationToken);
    public Task<List<CategoryDto>> GetAllCategoriesAsync(CancellationToken cancellationToken);
    public Task UpdateCategoryAsync(Guid categoryId, UpdateCategoryRequest request, CancellationToken cancellationToken);
}