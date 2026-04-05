using Templates.Repositories;
using WomenForum.Database;
using WomenForum.Domain.Models;
using WomenForum.Repository.Repositories.Interfaces;

namespace WomenForum.Repository.Repositories;

public class ReportsRepository(WomenForumDbContext context)
    : BaseRepository<Report, WomenForumDbContext>(context), IReportsRepository;