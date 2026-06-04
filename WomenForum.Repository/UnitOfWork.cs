using WomenForum.Repository.Repositories.Interfaces;

namespace WomenForum.Repository;

public class UnitOfWork : IUnitOfWork
{
    public UnitOfWork(ICategoriesRepository categoriesRepository, ICommentsRepository  commentsRepository,
        ICommunitiesRepository communitiesRepository, IUsersRepository usersRepository,
        IReportsRepository reportsRepository, ISubscriptionsRepository subscriptionsRepository,
        ICommunityJoinRequestsRepository communityJoinRequestsRepository, ICommunityMembersRepository communityMembersRepository,
        IDiscussionThreadsRepository discussionThreadsRepository, IPostsRepository postsRepository,
        ILikesRepository likesRepository, IMessagesRepository messagesRepository, INotificationsRepository notificationsRepository,
        IUserSettingsRepository userSettingsRepository, IWarningsRepository warningsRepository)
    {
        CategoriesRepository = categoriesRepository;
        CommentsRepository = commentsRepository;
        CommunitiesRepository = communitiesRepository;
        UsersRepository = usersRepository;
        ReportsRepository = reportsRepository;
        SubscriptionsRepository = subscriptionsRepository;
        CommunityJoinRequestsRepository = communityJoinRequestsRepository;
        CommunityMembersRepository = communityMembersRepository;
        DiscussionThreadsRepository = discussionThreadsRepository;
        PostsRepository = postsRepository;
        LikesRepository = likesRepository;
        MessagesRepository = messagesRepository;
        NotificationsRepository = notificationsRepository;
        UserSettingsRepository = userSettingsRepository;
        WarningsRepository = warningsRepository;
    }
    
    public ICategoriesRepository CategoriesRepository { get; set; }
    public ICommentsRepository CommentsRepository { get; set; }
    public ICommunitiesRepository CommunitiesRepository { get; set; }
    public ICommunityJoinRequestsRepository CommunityJoinRequestsRepository { get; set; }
    public ICommunityMembersRepository CommunityMembersRepository { get; set; }
    public IDiscussionThreadsRepository DiscussionThreadsRepository { get; set; }
    public ILikesRepository LikesRepository { get; set; }
    public IMessagesRepository MessagesRepository { get; set; }
    public IWarningsRepository WarningsRepository { get; set; }
    public INotificationsRepository NotificationsRepository { get; set; }
    public IPostsRepository PostsRepository { get; set; }
    public IReportsRepository ReportsRepository { get; set; }
    public ISubscriptionsRepository SubscriptionsRepository { get; set; }
    public IUsersRepository UsersRepository { get; set; }
    public IUserSettingsRepository UserSettingsRepository { get; set; }
}