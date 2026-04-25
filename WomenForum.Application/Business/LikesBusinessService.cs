using AutoMapper;
using Templates.Business;
using WomenForum.Business.Interfaces;
using WomenForum.Domain.Models;
using WomenForum.Exceptions;
using WomenForum.Models;
using WomenForum.Repository;

namespace WomenForum.Business;

public class LikesBusinessService : BaseBusinessService, ILikesBusinessService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<LikesBusinessService> _logger;

    public LikesBusinessService(
        IHttpContextAccessor httpContextAccessor,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<LikesBusinessService> logger) : base(httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task ChangeLikeStatusAsync(Guid postId, CancellationToken cancellationToken)
    {
        var post = await _unitOfWork.PostsRepository.GetByIdAsync(postId, cancellationToken);

        if (post == null)
        {
            throw new NotFoundException($"Пост {postId} не найден.");
        }

        var existingLike = post.Likes.FirstOrDefault(x => x.LikedById == UserId);

        if (existingLike == null)
        {
            var like = new Like
            {
                LikedById = UserId,
                PostId = postId,
            };

            await _unitOfWork.LikesRepository.AddAsync(like, cancellationToken);
            
            _logger.LogInformation("Like successfully added");

            return;
        }

        await _unitOfWork.LikesRepository.DeleteAsync(existingLike, cancellationToken);
        
        _logger.LogInformation("Like successfully deleted");
    }
}