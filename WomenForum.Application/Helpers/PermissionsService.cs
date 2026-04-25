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
        
        if (user.Role is Role.Moderator)
        {
            return [PermissionTypes.Read,PermissionTypes.Write];
        }
        
        resourceType = resourceType.ToLower();
        switch (resourceType)
        {
            case "categories":
                return [PermissionTypes.Read];

            case "comments":
                var comment = await _unitOfWork.CommentsRepository.GetByIdAsync(resourceId,cancellationToken);

                return comment?.CreatedById == userId ? _allPermissionsList : [PermissionTypes.Read];

            case "communities":
                var community = await _unitOfWork.CommunitiesRepository.GetByIdAsync(resourceId, cancellationToken);
                
                var communityMember = community?.Members?.FirstOrDefault(x => x.UserId == userId);
                
                if (communityMember?.Role is CommunityRole.Owner or CommunityRole.Admin)
                    return _allPermissionsList;

                if (communityMember?.Role == CommunityRole.Moderator)
                    return [PermissionTypes.Read, PermissionTypes.Write];

                if (communityMember?.Role == CommunityRole.Member || community?.Visibility == VisibilityType.Public)
                {
                    return [PermissionTypes.Read];
                }

                return [];
            
            case "community-members":
                var resourceMember = await _unitOfWork.CommunityMembersRepository.GetByIdAsync(resourceId, cancellationToken);

                if (resourceMember == null)
                    return [];
                
                var resourceCommunity =
                    await _unitOfWork.CommunitiesRepository.GetByIdAsync(resourceMember.CommunityId, cancellationToken);

                var currentResourceMember = resourceCommunity?.Members.FirstOrDefault(x => x.UserId == userId);

                if (currentResourceMember == null)
                {
                    return [];
                }

                if (currentResourceMember.Role > resourceMember.Role)
                {
                    return _allPermissionsList;
                }

                return [PermissionTypes.Read];
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
                if (userId == resourceId)
                {
                    return _allPermissionsList;
                }
                
                var resource = await _unitOfWork.UsersRepository.GetByIdAsync(resourceId, cancellationToken);

                if (resource?.Followers.FirstOrDefault(x => x.Id == userId) != null || resource?.Visibility == VisibilityType.Public)
                {
                    return [PermissionTypes.Read];
                }

                return [];
            default:
                return [];
        }
    }
}