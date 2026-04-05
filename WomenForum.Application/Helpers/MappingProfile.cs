using AutoMapper;
using WomenForum.Domain.Models;
using WomenForum.Models;
using WomenForum.Models.Requests;

namespace WomenForum.Helpers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateUserRequest, User>();
        
        CreateMap<CreateCategoryRequest, Category>();
        
        CreateMap<CreateCommunityRequest, Community>();
        
        CreateMap<CreateDiscussionThreadRequest, DiscussionThread>();
        
        CreateMap<CreatePostRequest, Post>();

        CreateMap<User, UserDto>()
            .ForMember(x => x.FollowingCount, y => y.MapFrom(z => z.Following.Count))
            .ForMember(x => x.FollowersCount, y => y.MapFrom(z => z.Followers.Count));
        
        CreateMap<Category, CategoryDto>();
        
        CreateMap<UserSettings, UserSettingsDto>();

        CreateMap<Comment, CommentDto>();
        
        CreateMap<Community, CommunityDto>();
        
        CreateMap<CommunityJoinRequest, CommunityJoinRequestDto>();
        
        CreateMap<CommunityMember, CommunityMemberDto>();
        
        CreateMap<DiscussionThread, DiscussionThreadDto>();
        
        CreateMap<Like, LikeDto>();

        CreateMap<Message, MessageDto>()
            .ForMember(x => x.ReplyCount, y => y.MapFrom(z => z.Replies.Count));
        
        CreateMap<Notification, NotificationDto>();
        
        CreateMap<Post, PostDto>();
        
        CreateMap<Report, ReportDto>();
        
        CreateMap<Subscription, SubscriptionDto>();
    }
}