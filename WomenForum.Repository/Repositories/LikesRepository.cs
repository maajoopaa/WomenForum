using Templates.Repositories;
using WomenForum.Database;
using WomenForum.Domain.Models;
using WomenForum.Repository.Repositories.Interfaces;

namespace WomenForum.Repository.Repositories;

public class LikesRepository(WomenForumDbContext context)
    : BaseRepository<Like, WomenForumDbContext>(context), ILikesRepository;