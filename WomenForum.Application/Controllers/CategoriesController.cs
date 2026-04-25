using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WomenForum.Business.Interfaces;
using WomenForum.Models;
using WomenForum.Models.Requests;

namespace WomenForum.Controllers;

[ApiController]
[Route("categories")]
[Produces("application/json")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoriesBusinessService _categoriesBusinessService;

    public CategoriesController(ICategoriesBusinessService categoriesBusinessService)
    {
        _categoriesBusinessService = categoriesBusinessService;
    }

    [HttpGet]
    public async Task<ActionResult<List<CategoryDto>>> GetAllAsync(CancellationToken cancellationToken)
    {
        var result = await _categoriesBusinessService.GetAllCategoriesAsync(cancellationToken);

        return Ok(result);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<CategoryDto>> AddAsync([FromBody] CreateCategoryRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _categoriesBusinessService.AddCategoryAsync(request, cancellationToken);

        return Ok(result);
    }
    
    [Authorize]
    [HttpPut("{categoryId:guid}")]
    public async Task<ActionResult<CategoryDto>> UpdateAsync(Guid categoryId, [FromBody] UpdateCategoryRequest request,
        CancellationToken cancellationToken)
    {
        await _categoriesBusinessService.UpdateCategoryAsync(categoryId, request, cancellationToken);

        return NoContent();
    }
}