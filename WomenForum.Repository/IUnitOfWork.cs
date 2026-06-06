using WomenForum.Repository.Repositories.Interfaces;

namespace WomenForum.Repository;

public interface IUnitOfWork
{
    ICategoriesRepository CategoriesRepository { get; set; }
    ICommentsRepository CommentsRepository { get; set; }
    ICommunitiesRepository CommunitiesRepository { get; set; }
    ICommunityJoinRequestsRepository CommunityJoinRequestsRepository { get; set; }
    ICommunityMembersRepository CommunityMembersRepository { get; set; }
    IDiscussionThreadsRepository DiscussionThreadsRepository { get; set; }
    ILikesRepository LikesRepository { get; set; }
    IMessagesRepository MessagesRepository { get; set; }
    IWarningsRepository WarningsRepository { get; set; }
    INotificationsRepository NotificationsRepository { get; set; }
    IPostsRepository PostsRepository { get; set; }
    IReportsRepository ReportsRepository { get; set; }
    ISubscriptionsRepository SubscriptionsRepository { get; set; }
    IUsersRepository UsersRepository { get; set; }
    IUserActivitiesRepository UserActivitiesRepository { get; set; }
    IUserSettingsRepository UserSettingsRepository { get; set; }
}