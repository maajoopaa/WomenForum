using AutoMapper;
using Templates.Business;
using Templates.Models;
using WomenForum.Business.Interfaces;
using WomenForum.Domain.Models;
using WomenForum.Exceptions;
using WomenForum.Helpers.Interfaces;
using WomenForum.Models;
using WomenForum.Models.Requests;
using WomenForum.Repository;

namespace WomenForum.Business;

public class MessagesBusinessService : BaseBusinessService, IMessagesBusinessService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<MessagesBusinessService> _logger;
    private readonly IPermissionsService _permissionsService;

    public MessagesBusinessService(
        IHttpContextAccessor httpContextAccessor,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<MessagesBusinessService> logger,
        IPermissionsService permissionsService) : base(httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
        _permissionsService = permissionsService;
    }

    public async Task<MessageDto> AddMessageAsync(Guid threadId, CreateMessageRequest request, CancellationToken cancellationToken)
    {
        var thread = await _unitOfWork.DiscussionThreadsRepository.GetByIdAsync(threadId, cancellationToken);

        if (thread == null)
        {
            throw new NotFoundException($"Тема для обсуждения {threadId} не найдена.");
        }
        
        var entity = _mapper.Map<Message>(request);
        
        entity.DiscussionThreadId = threadId;
        entity.CreatedById = UserId;
        
        await _unitOfWork.MessagesRepository.AddAsync(entity, cancellationToken);
        
        _logger.LogInformation("Message successfully added");
        
        return _mapper.Map<MessageDto>(entity);
    }

    public async Task UpdateMessageAsync(Guid messageId, UpdateMessageRequest request, CancellationToken cancellationToken)
    {
        var permissions =
            await _permissionsService.GetUserPermissionsAsync("messages", UserId, messageId, cancellationToken);

        if (!permissions.Contains(PermissionTypes.Write))
        {
            throw new NoPermissionException("У вас недостаточно прав для этого действия.");
        }
        
        var entity = await _unitOfWork.MessagesRepository.GetByIdAsync(messageId, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException($"Сообщение {messageId} не найдено.");
        }

        entity.Content = request.Content;

        await _unitOfWork.MessagesRepository.UpdateAsync(entity, cancellationToken);
        
        _logger.LogInformation("Message successfully updated");
    }

    public async Task DeleteMessagesAsync(List<Guid> messageIds, CancellationToken cancellationToken)
    {
        var entities = await _unitOfWork.MessagesRepository.GetAsync(x =>
            messageIds.Contains(x.Id), cancellationToken);

        foreach (var message in entities)
        {
            var permissions =
                await _permissionsService.GetUserPermissionsAsync("messages", UserId, message.Id, cancellationToken);

            if (!permissions.Contains(PermissionTypes.Delete))
            {
                throw new NoPermissionException("У вас недостаточно прав для этого действия.");
            }
        }

        await _unitOfWork.MessagesRepository.DeleteRangeAsync(entities, cancellationToken);
        
        _logger.LogInformation("Messages successfully deleted");
    }

    public async Task<PagedResult<MessageDto>> GetMessagesByDiscussionThreadIdAsync(Guid discussionThreadId, PaginationParameters paginationParameters, CancellationToken cancellationToken)
    {
        var pagedEntities = await _unitOfWork.MessagesRepository.GetPagedAsync(x =>
            x.DiscussionThreadId == discussionThreadId, paginationParameters.PageNumber, paginationParameters.PageSize, cancellationToken);

        pagedEntities.Items = pagedEntities.Items.OrderBy(x => x.CreatedAt).ToList();
        
        return new PagedResult<MessageDto>(
            _mapper.Map<List<MessageDto>>(pagedEntities.Items),
            pagedEntities.TotalCount,
            pagedEntities.PageNumber,
            pagedEntities.PageSize
        );
    }

    public async Task<PagedResult<MessageDto>> GetRepliesAsync(Guid messageId, PaginationParameters paginationParameters, CancellationToken cancellationToken)
    {
        var pagedReplies = await _unitOfWork.MessagesRepository.GetPagedAsync(x =>
            x.ParentMessageId == messageId, paginationParameters.PageNumber, paginationParameters.PageSize, cancellationToken);
        
        return new PagedResult<MessageDto>(
            _mapper.Map<List<MessageDto>>(pagedReplies.Items),
            pagedReplies.TotalCount,
            pagedReplies.PageNumber,
            pagedReplies.PageSize
        );
    }
}