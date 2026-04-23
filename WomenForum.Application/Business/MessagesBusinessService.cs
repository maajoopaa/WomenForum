using AutoMapper;
using Templates.Business;
using WomenForum.Business.Interfaces;
using WomenForum.Domain.Models;
using WomenForum.Exceptions;
using WomenForum.Models;
using WomenForum.Models.Requests;
using WomenForum.Repository;

namespace WomenForum.Business;

public class MessagesBusinessService : BaseBusinessService, IMessagesBusinessService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public MessagesBusinessService(
        IHttpContextAccessor httpContextAccessor,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger logger) : base(httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
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
        
        await _unitOfWork.MessagesRepository.AddAsync(entity, cancellationToken);
        
        _logger.LogInformation("Message successfully added");
        
        return _mapper.Map<MessageDto>(entity);
    }

    public async Task UpdateMessageAsync(Guid messageId, UpdateMessageRequest request, CancellationToken cancellationToken)
    {
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

        await _unitOfWork.MessagesRepository.DeleteRangeAsync(entities, cancellationToken);
        
        _logger.LogInformation("Messages successfully deleted");
    }

    public async Task<List<MessageDto>> GetMessagesByDiscussionThreadIdAsync(Guid discussionThreadId, CancellationToken cancellationToken)
    {
        var entities = await _unitOfWork.MessagesRepository.GetAsync(x =>
            x.DiscussionThreadId == discussionThreadId, cancellationToken);
        
        return _mapper.Map<List<MessageDto>>(entities);
    }

    public async Task<List<MessageDto>> GetRepliesAsync(Guid messageId, CancellationToken cancellationToken)
    {
        var replies = await _unitOfWork.MessagesRepository.GetAsync(x =>
            x.ParentMessageId == messageId, cancellationToken);
        
        return _mapper.Map<List<MessageDto>>(replies);
    }
}