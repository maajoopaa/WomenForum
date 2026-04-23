using Templates.Repositories.Interfaces;
using WomenForum.Domain.Models;

namespace WomenForum.Repository.Repositories.Interfaces;

public interface IUsersRepository : IBaseRepository<User>
{
    public Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken);
    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
}