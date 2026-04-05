using Templates.Repositories;
using WomenForum.Database;
using WomenForum.Domain.Models;
using WomenForum.Repository.Repositories.Interfaces;

namespace WomenForum.Repository.Repositories;

public class PostsRepository(WomenForumDbContext context)
    : BaseRepository<Post, WomenForumDbContext>(context), IPostsRepository;