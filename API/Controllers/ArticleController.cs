using API.Models.Articles;
using API.Models.Tags;
using API.Models.Users;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyBlog.BLL.Models;
using MyBlog.BLL.ViewModels;
using MyBlog.DAL.Repository;
using MyBlog.DAL.UoW;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ArticleController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private IUnitOfWork _unitOfWork;
        private ArticleRepository _repository;
        private TagRepository _tagRepository;
        private IMapper _mapper;

        public ArticleController(UserManager<User> userManager, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _repository = _unitOfWork.GetRepository<Article>() as ArticleRepository;
            _mapper = mapper;
            _tagRepository = _unitOfWork.GetRepository<Tag>() as TagRepository;
        }

        /// <summary>
        /// Получение всех статей
        /// </summary>
        /// <returns>Возврат массива статей</returns>
        [HttpGet]
        public async Task<IActionResult> GetArticles()
        {
            var articles = await _repository.GetAll();

            var articlesResp = articles
                .Select(x => new GetArticlesResponse()
                {
                    Id = x.Id,
                    Author = x.Author,
                    DateTimeArticle = x.DateTimeArticle,
                    Text = x.Text,
                    Title = x.Title,
                    Tags = x.Tags.Select(t => new TagViews
                    {
                        Id = t.Id,
                        Name = t.Name
                    })
                    .ToList()
                })
                .ToArray();

            return Ok(articlesResp);
        }

        /// <summary>
        /// Получение статьи по ID
        /// </summary>
        /// <param name="id">ID статьи</param>
        /// <returns>Возврат статьи</returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetArticle([FromRoute] int id)
        {
            var article = await _repository.Get(id);
            if (article == null)
                return BadRequest($"Ошибка: Статьи с идентификатором {id} не существует.");

            var articleResp = _mapper.Map<Article, GetArticlesResponse>(article);

            return Ok(articleResp);
        }

        /// <summary>
        /// Получение всех статей пользователя по ID пользователя
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        /// <returns>Возврат массива статей конкретного пользователя</returns>
        [HttpGet]
        [Route("{userId}")]
        public async Task<IActionResult> GetUsersArticles([FromRoute] string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return BadRequest($"Ошибка: Пользователь с идентификатором {userId} не существует.");

            var articles = await _repository.GetArticlesByUserId(userId);

            var articlesResp = articles
                .Select(x => new GetArticlesResponse()
                {
                    Id = x.Id,
                    Author = x.Author,
                    DateTimeArticle = x.DateTimeArticle,
                    Text = x.Text,
                    Title = x.Title,
                    Tags = x.Tags.Select(t => new TagViews
                    {
                        Id = t.Id,
                        Name = t.Name
                    })
                    .ToList()
                })
                .ToArray();

            return Ok(articlesResp);
        }

        /// <summary>
        /// Создание статьи
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     POST 
        ///     {
        ///        "UserId" : "05c86774-1274-40d9-bebf-0bcd2669f7f2", 
        ///        "Title" : "Очень интересная статья",
        ///        "Text" : "Текст самой интересной статьи"
        ///     }
        ///
        /// </remarks>
        /// <param name="request">Статья</param>
        /// <returns>Новая статья</returns>
        [HttpPost]
        public async Task<IActionResult> Add(AddArticleRequest request)
        {
            if ((request.Title == "") || (request.Title == null))
                return BadRequest($"Ошибка: Не указано обязательное поле Title");

            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
                return BadRequest($"Ошибка: Пользователь с идентификатором {request.UserId} не существует.");

            var newArticle = new Article()
            {
                Title = request.Title,
                Text = request.Text,
                AuthorId = request.UserId,
                Author = user,
                DateTimeArticle = DateTime.Now
            };

            await _repository.Create(newArticle);

            return Created(newArticle.Id.ToString(), _mapper.Map<Article, GetArticlesResponse>(newArticle));
        }

        /// <summary>
        /// Удаление статьи
        /// </summary>
        /// <param name="id">ID статьи</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var article = await _repository.Get(id);
            if (article == null)
                return BadRequest($"Ошибка: Статьи с идентификатором {id} не существует.");

            _repository.Delete(article);

            return Ok();
        }

        /// <summary>
        /// Изменение статьи
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     POST 
        ///     {
        ///        "UserId" : "05c86774-1274-40d9-bebf-0bcd2669f7f2", 
        ///        "Title" : "Очень интересная статья",
        ///        "Text" : "Текст самой интересной статьи"
        ///     }
        ///
        /// </remarks>
        /// <param name="id">ID статьи</param>
        /// <param name="request">Статья</param>
        /// <returns>Измененная статья</returns>
        [HttpPatch]
        [Route("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] EditArticleRequest request)
        {
            var article = await _repository.Get(id);
            if (article == null)
                return BadRequest($"Ошибка: Статьи с идентификатором {id} не существует.");

            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
                return BadRequest($"Ошибка: Пользователь с идентификатором {request.UserId} не существует.");

            if (!string.IsNullOrEmpty(request.Text))
                article.Text = request.Text;

            if (!string.IsNullOrEmpty(request.Title))
                article.Title = request.Title;

            if (!string.IsNullOrEmpty(request.UserId))
            {
                article.AuthorId = request.UserId;
                article.Author = user;
            }

            await _repository.Update(article);

            var articleResp = _mapper.Map<Article, GetArticlesResponse>(article);

            return Ok(articleResp);
        }

        /// <summary>
        /// Изменение тэгов в статье
        /// </summary>
        /// <param name="id">ID статьи</param>
        /// <param name="idTags">Массив ID тэгов</param>
        /// <returns>Измененная статья</returns>
        [HttpPatch]
        [Route("{id}")]
        public async Task<IActionResult> EditArticlesTags([FromRoute] int id, [FromBody] int[] idTags)
        {
            var article = await _repository.Get(id);
            if (article == null)
                return BadRequest($"Ошибка: Статьи с идентификатором {id} не существует.");

            foreach (var idTag in idTags)
            {
                var tag = await _tagRepository.Get(idTag);

                if (tag == null)
                    return BadRequest($"Ошибка: Тэга с идентификатором {id} не существует.");

                if (!article.Tags.Contains(tag))
                    article.Tags.Add(tag);
            }

            await _repository.Update(article);

            var articleResp = _mapper.Map<Article, GetArticlesResponse>(article);

            return Ok(articleResp);
        }
    }
}
