using API.Models.Articles;
using API.Models.Comments;
using API.Models.Users;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyBlog.BLL.Models;
using MyBlog.DAL.Repository;
using MyBlog.DAL.UoW;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class CommentController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private IUnitOfWork _unitOfWork;
        private CommentRepository _repository;
        private ArticleRepository _articleRepository;
        private IMapper _mapper;

        public CommentController(UserManager<User> userManager, IUnitOfWork unitOfWork, IMapper mapper = null)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _repository = _unitOfWork.GetRepository<Comment>() as CommentRepository;
            _articleRepository = _unitOfWork.GetRepository<Article>() as ArticleRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Получение всех комментариев
        /// </summary>
        /// <returns>Возврат массива комментариев</returns>
        [HttpGet]
        public async Task<IActionResult> GetComments()
        {
            var comments = await _repository.GetAll();

            var commentsResp = comments
                .Select(x => new GetCommentsResponse()
                {
                    Id = x.Id,
                    Text = x.Text,
                    AuthorId = x.AuthorId,
                    ArticleId = x.ArticleId
                })
                .ToArray();

            return Ok(commentsResp);
        }

        /// <summary>
        /// Получение комментария по ID
        /// </summary>
        /// <param name="id">ID комментария</param>
        /// <returns>Возврат комментария</returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetComment([FromRoute] int id)
        {
            var comment = await _repository.Get(id);
            if (comment ==  null) 
                return BadRequest($"Ошибка: Комментария с идентификатором {id} не существует.");

            var commentResp = _mapper.Map<Comment, GetCommentsResponse>(comment);

            return Ok(commentResp);
        }

        /// <summary>
        /// Получение всех комментариев для конкретной статьи по ID статьи
        /// </summary>
        /// <param name="articleId">ID статьи</param>
        /// <returns>Возврат массива комментариев конкретной статьи</returns>
        [HttpGet]
        [Route("{articleId}")]
        public async Task<IActionResult> GetCommentsByArticleId([FromRoute] int articleId)
        {
            var article = await _articleRepository.Get(articleId);
            if (article == null)
                return BadRequest($"Ошибка: Статьи с идентификатором {articleId} не существует.");

            var comments = await _repository.GetByArticleId(articleId);

            var commentsResp = comments
                .Select(x => new GetCommentsResponse()
                {
                    Id = x.Id,
                    Text = x.Text,
                    AuthorId = x.AuthorId,
                    ArticleId = articleId
                })
                .ToArray();

            return Ok(commentsResp);
        }

        /// <summary>
        /// Получение всех комментариев конкретного пользователя по ID пользователя
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        /// <returns>Возврат массива комментариев конкретного пользователя</returns>
        [HttpGet]
        [Route("{userId}")]
        public async Task<IActionResult> GetCommentsByUserId([FromRoute] string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return BadRequest($"Ошибка: Пользователя с идентификатором {userId} не существует.");

            var comments = await _repository.GetByUserId(userId);

            var commentsResp = comments
                .Select(x => new GetCommentsResponse()
                {
                    Id = x.Id,
                    Text = x.Text,
                    AuthorId = userId,
                    ArticleId = x.ArticleId
                })
                .ToArray();

            return Ok(commentsResp);
        }

        /// <summary>
        /// Добавление нового комментария к статье
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     POST 
        ///     {
        ///        "Text" : "Новый комментарий", 
        ///        "AuthorId" : "05c86774-1274-40d9-bebf-0bcd2669f7f2",
        ///        "ArticleId" : 7
        ///     }
        ///
        /// </remarks>
        /// <param name="request">Комментарий</param>
        /// <returns>Новый комментарий для статьи</returns>
        [HttpPost]
        public async Task<IActionResult> Add(AddCommentRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.AuthorId);
            if (user == null)
                return BadRequest($"Ошибка: Пользователя с идентификатором {request.AuthorId} не существует.");

            var article = await _articleRepository.Get(request.ArticleId);
            if (article == null)
                return BadRequest($"Ошибка: Статьи с идентификатором {request.ArticleId} не существует.");

            var newComment = new Comment()
            {
                AuthorId = request.AuthorId,
                ArticleId = request.ArticleId,
                Text = request.Text
            };
            await _repository.Create(newComment);

            return Created(newComment.Id.ToString(), _mapper.Map<Comment, GetCommentsResponse>(newComment));
        }

        /// <summary>
        /// Удаление комментария
        /// </summary>
        /// <param name="id">ID комментария</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var comment = await _repository.Get(id);
            if (comment == null)
                return BadRequest($"Ошибка: Комментария с идентификатором {id} не существует.");

            _repository.Delete(comment);

            return Ok();
        }

        /// <summary>
        /// Изменение комментария
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     POST 
        ///     {
        ///        "Text" : "Новый текст комментария", 
        ///        "AuthorId" : "05c86774-1274-40d9-bebf-0bcd2669f7f2",
        ///        "ArticleId" : 7
        ///     }
        ///
        /// </remarks>
        /// <param name="id">ID комментария</param>
        /// <param name="request">Комментарий</param>
        /// <returns>Измененный комментарий</returns>
        [HttpPatch]
        [Route("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] EditCommentRequest request)
        {
            var comment = await _repository.Get(id);
            if (comment == null)
                return BadRequest($"Ошибка: Комментария с идентификатором {id} не существует.");

            if (!string.IsNullOrEmpty(request.Text))
                comment.Text = request.Text;

            if (!string.IsNullOrEmpty(request.AuthorId))
            {
                var user = await _userManager.FindByIdAsync(request.AuthorId);
                if (user == null)
                    return BadRequest($"Ошибка: Пользователя с идентификатором {request.AuthorId} не существует.");

                comment.AuthorId = request.AuthorId;
            }

            if (!(request.ArticleId == 0) && !(request.ArticleId == null))
            {
                var article = await _articleRepository.Get(request.ArticleId);
                if (article == null)
                    return BadRequest($"Ошибка: Статьи с идентификатором {request.ArticleId} не существует.");

                comment.ArticleId = request.ArticleId;
            }

            await _repository.Update(comment);

            var commentResp = _mapper.Map<Comment, GetCommentsResponse>(comment);

            return Ok(commentResp);
        }
    }
}
