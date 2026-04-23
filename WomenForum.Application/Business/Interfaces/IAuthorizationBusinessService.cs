using Templates.Business.Interfaces;
using WomenForum.Models;
using WomenForum.Models.Requests;

namespace WomenForum.Business.Interfaces;

public interface IAuthorizationBusinessService : IBaseBusinessService
{
    Task<AuthorizationResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken);
    Task<AuthorizationResponse> RegisterAsync(CreateUserRequest request, CancellationToken cancellationToken);
}