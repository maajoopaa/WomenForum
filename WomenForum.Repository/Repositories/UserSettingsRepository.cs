using Templates.Repositories;
using WomenForum.Database;
using WomenForum.Domain.Models;
using WomenForum.Repository.Repositories.Interfaces;

namespace WomenForum.Repository.Repositories;

public class UserSettingsRepository(WomenForumDbContext context)
    : BaseRepository<UserSettings, WomenForumDbContext>(context), IUserSettingsRepository;