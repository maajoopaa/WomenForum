using WomenForum.Domain.Enums;
using WomenForum.Helpers.Interfaces;
using WomenForum.Models;
using WomenForum.Repository;

namespace WomenForum.Helpers;

public class PermissionsService : IPermissionsService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly List<PermissionTypes> _allPermissionsList =
        [PermissionTypes.Read, PermissionTypes.Write, PermissionTypes.Delete];

    public PermissionsService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<List<PermissionTypes>> GetUserPermissionsAsync(string resourceType, Guid userId, Guid resourceId, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.UsersRepository.GetByIdAsync(userId, cancellationToken);

        if (user == null)
        {
            return [];
        }
        
        if (user.Role is Role.Administrator)
        {
            return _allPermissionsList;
        }
        
        resourceType = resourceType.ToLower();
        switch (resourceType)
        {
            case "categories":
                return user.Role is Role.Moderator ? [PermissionTypes.Read,PermissionTypes.Write] : ([PermissionTypes.Read]);

            case "comments":
                if (user.Role is Role.Moderator)
                {
                    return [PermissionTypes.Read,PermissionTypes.Write];
                }
                
                var comment = await _unitOfWork.CommentsRepository.GetByIdAsync(resourceId,cancellationToken);

                return comment?.CreatedById == userId ? _allPermissionsList : [PermissionTypes.Read];

            case "communities":
                if (user.Role is Role.Moderator)
                {
                    return [PermissionTypes.Read,PermissionTypes.Write];
                }
                
                var community = await _unitOfWork.CommunitiesRepository.GetByIdAsync(resourceId, cancellationToken);

                if (community?.CreatedById == userId)
                {
                    return _allPermissionsList;
                }
                
                var communityMember = community?.Members.FirstOrDefault(x => x.UserId == userId);
                
                if (communityMember == null)
                    return [PermissionTypes.Read];
                
                if (communityMember.Role is CommunityRole.Owner or CommunityRole.Admin)
                    return _allPermissionsList;

                return [PermissionTypes.Read, PermissionTypes.Write];

            case "threads":
                if (user.Role is Role.Moderator)
                {
                    return [PermissionTypes.Read,PermissionTypes.Write];
                }
                
                var thread = await _unitOfWork.DiscussionThreadsRepository.GetByIdAsync(resourceId, cancellationToken);

                return thread?.CreatedById == userId ? _allPermissionsList : [PermissionTypes.Read];

            case "messages":
                if (user.Role is Role.Moderator)
                {
                    return [PermissionTypes.Read,PermissionTypes.Write];
                }
                
                var message = await _unitOfWork.MessagesRepository.GetByIdAsync(resourceId, cancellationToken);

                return message?.CreatedById == userId ? _allPermissionsList : [PermissionTypes.Read];

            case "posts":
                if (user.Role is Role.Moderator)
                {
                    return [PermissionTypes.Read,PermissionTypes.Write];
                }
                
                var post = await _unitOfWork.PostsRepository.GetByIdAsync(resourceId, cancellationToken);

                return post?.AuthorUserId == userId ? _allPermissionsList : [PermissionTypes.Read];

            case "reports":
                return user.Role == Role.Moderator ? _allPermissionsList : [];

            case "users":
                return userId == resourceId ? _allPermissionsList : [PermissionTypes.Read];

            default:
                return [];
        }
    }
}