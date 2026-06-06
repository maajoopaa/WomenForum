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
        
        CreateMap<CreateCommentRequest, Comment>();
        
        CreateMap<CreateMessageRequest, Message>();
        
        CreateMap<CreateWarningRequest, Warning>();
        
        CreateMap<CreateDiscussionThreadRequest, DiscussionThread>();
        
        CreateMap<CreatePostRequest, Post>();
        
        CreateMap<CreateReportRequest, Report>();

        CreateMap<User, UserDto>()
            .ForMember(x => x.FollowingCount, y => y.MapFrom(z => z.Following.Count))
            .ForMember(x => x.FollowersCount, y => y.MapFrom(z => z.Followers.Count))
            .ForMember(dest => dest.IsCurrentUserSubscriber, opt => opt.MapFrom((src, dest, destMember, context) =>
            {
                if (context.Items.TryGetValue("CurrentUserId", out var userIdObj) && userIdObj is Guid currentUserId)
                {
                    return src.Followers.Any(m => m.SubscriberId == currentUserId);
                }
                return false;
            }));
        
        CreateMap<Category, CategoryDto>();
        
        CreateMap<Warning, WarningDto>();
        
        CreateMap<UserSettings, UserSettingsDto>();

        CreateMap<Comment, CommentDto>();

        CreateMap<Community, CommunityDto>()
            .ForMember(x => x.Subscribers, y => y.MapFrom(z => z.Members.Count))
            .ForMember(dest => dest.IsCurrentUserSubscriber, opt => opt.MapFrom((src, dest, destMember, context) =>
            {
                if (context.Items.TryGetValue("CurrentUserId", out var userIdObj) && userIdObj is Guid currentUserId)
                {
                    return src.Members.Any(m => m.UserId == currentUserId);
                }
                return false;
            }));
            
        CreateMap<CommunityJoinRequest, CommunityJoinRequestDto>();
        
        CreateMap<CommunityMember, CommunityMemberDto>();
        
        CreateMap<DiscussionThread, DiscussionThreadDto>();
        
        CreateMap<Like, LikeDto>();

        CreateMap<Message, MessageDto>()
            .ForMember(x => x.ReplyCount, y => y.MapFrom(z => z.Replies.Count))
            .ForMember(x => x.ParentMessage, y => y.MapFrom(z => z.ParentMessage));

        CreateMap<Notification, NotificationDto>();
        
        CreateMap<Post, PostDto>();
        
        CreateMap<Report, ReportDto>();
        
        CreateMap<Subscription, SubscriptionDto>();
        
        CreateMap<UserActivity, UserActivityDto>();
    }
}