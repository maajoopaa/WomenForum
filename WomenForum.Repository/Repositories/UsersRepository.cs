using Microsoft.EntityFrameworkCore;
using Templates.Repositories;
using WomenForum.Database;
using WomenForum.Domain.Models;
using WomenForum.Repository.Repositories.Interfaces;

namespace WomenForum.Repository.Repositories;

public class UsersRepository(WomenForumDbContext context)
    : BaseRepository<User, WomenForumDbContext>(context), IUsersRepository
{
    public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Username == username,cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Email == email,cancellationToken);
    }
}