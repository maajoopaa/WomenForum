using AutoMapper;
using Templates.Business;
using WomenForum.Business.Interfaces;
using WomenForum.Domain.Enums;
using WomenForum.Domain.Models;
using WomenForum.Exceptions;
using WomenForum.Helpers;
using WomenForum.Models;
using WomenForum.Models.Requests;
using WomenForum.Repository;

namespace WomenForum.Business;

public class AuthorizationBusinessService(
    JWTHelper jwtHelper,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IHttpContextAccessor httpContextAccessor)
    : BaseBusinessService(httpContextAccessor), IAuthorizationBusinessService
{
    public async Task<AuthorizationResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        User? user = null;

        if (!string.IsNullOrEmpty(request.Email))
        {
            user = await unitOfWork.UsersRepository
                .GetByEmailAsync(request.Email, cancellationToken);
        }
        else if(!string.IsNullOrEmpty(request.Username))
        {
            user = await unitOfWork.UsersRepository
                .GetByUsernameAsync(request.Username, cancellationToken);
        }

        if (user == null || !PasswordHasher.VerifyPassword(user.PasswordHash, request.Password))
        {
            throw new NotFoundException("Такого пользователя не существует.");
        }

        user.LastLogin = DateTime.UtcNow;
        await unitOfWork.UsersRepository.UpdateAsync(user, cancellationToken);

        var token = jwtHelper.GenerateToken(user.Id,user.Username,user.Role);

        return new AuthorizationResponse
        {
            Token = token,
            User = mapper.Map<UserDto>(user)
        };
    }

    public async Task<AuthorizationResponse> RegisterAsync(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var userEntity = mapper.Map<User>(request);
        userEntity.PasswordHash = PasswordHasher.HashPassword(request.Password);
        userEntity.LastLogin = DateTime.UtcNow;

        var userSettings = new UserSettings()
        {
            Theme = Theme.Dark,
            UserId = userEntity.Id
        };

        try
        {
            await unitOfWork.UsersRepository.AddAsync(userEntity,cancellationToken);
            await unitOfWork.UserSettingsRepository.AddAsync(userSettings, cancellationToken);
        }
        catch
        {
            throw new NotFoundException("Такой пользователь уже существует.");
        }

        return await LoginAsync(new LoginRequest
        {
            Username = request.Username,
            Password = request.Password,
        }, cancellationToken);
    }
}