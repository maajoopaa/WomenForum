using Templates.Repositories;
using WomenForum.Database;
using WomenForum.Domain.Models;
using WomenForum.Repository.Repositories.Interfaces;

namespace WomenForum.Repository.Repositories;

public class UsersRepository(WomenForumDbContext context)
    : BaseRepository<User, WomenForumDbContext>(context), IUsersRepository;