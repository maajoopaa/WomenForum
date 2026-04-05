using Templates.Repositories;
using WomenForum.Database;
using WomenForum.Domain.Models;
using WomenForum.Repository.Repositories.Interfaces;

namespace WomenForum.Repository.Repositories;

public class NotificationsRepository(WomenForumDbContext context)
    : BaseRepository<Notification, WomenForumDbContext>(context), INotificationsRepository;