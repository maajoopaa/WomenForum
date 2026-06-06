using Templates.Business;
using Templates.Repositories;
using WomenForum.Database;
using WomenForum.Domain.Models;
using WomenForum.Repository.Repositories.Interfaces;

namespace WomenForum.Repository.Repositories;

public class UserActivitiesRepository(WomenForumDbContext context)
    : BaseRepository<UserActivity, WomenForumDbContext>(context), IUserActivitiesRepository;