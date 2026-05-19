using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Templates.Models;
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
    public async Task<ActionResult<PagedResult<CategoryDto>>> GetAllAsync([FromQuery] PaginationParameters paginationParameters, CancellationToken cancellationToken)
    {
        var result = await _categoriesBusinessService.GetAllCategoriesAsync(paginationParameters, cancellationToken);

        return Ok(result);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<CategoryDto>> AddAsync([FromBody] CreateCategoryRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _categoriesBusinessService.AddCategoryAsync(request, cancellationToken);

        return Created(string.Empty, result);
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