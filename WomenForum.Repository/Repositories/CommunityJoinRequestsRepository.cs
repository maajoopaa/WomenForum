using Templates.Repositories;
using WomenForum.Database;
using WomenForum.Domain.Models;
using WomenForum.Repository.Repositories.Interfaces;

namespace WomenForum.Repository.Repositories;

public class CommunityJoinRequestsRepository(WomenForumDbContext context)
    : BaseRepository<CommunityJoinRequest, WomenForumDbContext>(context), ICommunityJoinRequestsRepository;