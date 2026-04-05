using Templates.Repositories;
using WomenForum.Database;
using WomenForum.Domain.Models;
using WomenForum.Repository.Repositories.Interfaces;

namespace WomenForum.Repository.Repositories;

public class CategoriesRepository(WomenForumDbContext context)
    : BaseRepository<Category, WomenForumDbContext>(context), ICategoriesRepository;