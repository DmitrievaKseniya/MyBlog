using AutoMapper;
using MyBlog.BLL.Models;
using MyBlog.BLL.ViewModels;
using MyBlog.DAL.Repository;
using MyBlog.DAL.UoW;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBlog.WebService.Extentions;
using Microsoft.AspNetCore.Authorization;

namespace MyBlog.WebService.Controllers
{
    [Route("[controller]/[action]")]
    public class ArticleController : Controller
    {
        private readonly UserManager<User> _userManager;
        private IUnitOfWork _unitOfWork;
        private ArticleRepository _repository;

        public ArticleController(UserManager<User> userManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _repository = _unitOfWork.GetRepository<Article>() as ArticleRepository;
        }

        [HttpGet]
        public async Task<IActionResult> ViewArticle(int id)
        {
            var thisUser = User;
            var resultUser = await _userManager.GetUserAsync(thisUser);

            var article = await GetArticleById(id);

            var model = new ArticleViewModel(article);

            var repostoryComments = _unitOfWork.GetRepository<Comment>() as CommentRepository;

            var comments = await repostoryComments.GetByArticleId(id);

            model.Comments = comments;
            if (resultUser != null)
            {
                model.NewCommentVM = new CommentNewViewModel()
                {
                    IdUser = resultUser.Id,
                    IdArticle = id
                };
            }
            else
            {
                model.NewCommentVM = new CommentNewViewModel();
            }

            return View("Article", model);
        }

        [Authorize]
        [Route("NewArticle")]
        [HttpPost]
        public async Task<IActionResult> NewArticle(ArticleNewViewModel newArticle)
        {
            var thisUser = User;

            var result = await _userManager.GetUserAsync(thisUser);

            var repositiryTag = _unitOfWork.GetRepository<Tag>() as TagRepository;

            var selectedTags = new List<Tag>();
            foreach (int idTag in newArticle.SelectedIdTags)
            {
                selectedTags.Add(await repositiryTag.Get(idTag));
            }

            var item = new Article()
            {
                Title = newArticle.Title,
                Text = newArticle.Text,
                AuthorId = newArticle.UserId,
                Author = result,
                DateTimeArticle = DateTime.Now,
                Tags = selectedTags
            };
            await _repository.Create(item);

            return RedirectToAction("GetAllArticle", "Article");
        }

        [Authorize]
        [Route("NewArticlePage")]
        [HttpGet]
        public async Task<IActionResult> NewArticlePage()
        {
            var thisUser = User;

            var result = await _userManager.GetUserAsync(thisUser);

            var repositiryTag = _unitOfWork.GetRepository<Tag>() as TagRepository;

            var AllTags = await repositiryTag.GetAll();

            var model = new ArticleNewViewModel()
            {
                AllTags = AllTags,
                UserId = result.Id
            };

            return View("NewArticle", model);
        }

        [Route("UpdateArticle")]
        [HttpPost]
        public async Task<IActionResult> UpdateArticle(ArticleEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var article = await GetArticleById(model.IdArticle);

                var repositiryTag = _unitOfWork.GetRepository<Tag>() as TagRepository;

                var selectedTags = new List<Tag>();
                foreach (int idTag in model.SelectedIdTags)
                {
                    selectedTags.Add(await repositiryTag.Get(idTag));
                }

                article.Convert(model, selectedTags);

                await _repository.Update(article);

                return RedirectToAction("GetAllArticle", "Article");
            }
            else
            {
                ModelState.AddModelError("", "Некорректные данные");
                return View("Edit", model);
            }
        }

        [Route("EditArticle/{id}")]
        [HttpGet]
        public async Task<IActionResult> EditArticle(int id)
        {
            var article = await GetArticleById(id);

            var SelectedTags = new List<Tag>();
            foreach (Tag tag in article.Tags)
            {
                SelectedTags.Add(tag);
            }

            var repositiryTag = _unitOfWork.GetRepository<Tag>() as TagRepository;
            var AllTags = await repositiryTag.GetAll();

            var model = new ArticleEditViewModel()
            {
                IdArticle = article.Id,
                AllTags = AllTags,
                SelectedTags = SelectedTags,
                Title = article.Title,
                Text = article.Text
            };

            return View("EditArticle", model);

        }

        [Route("DeleteArticle")]
        [HttpPost]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            var article = await GetArticleById(id);

            _repository.Delete(article);

            return RedirectToAction("GetAllArticle");
        }

        [HttpGet]
        public async Task<IActionResult> GetAllArticle()
        {
            var articles = await _repository.GetAll();

            var model = articles
                .Select(x => new ArticlesListViewModel()
                {
                    ArticlesList = x
                })
                .ToArray();

            return View("ArticlesList", model);
        }

        [Route ("GetArticleByUserId")]
        [HttpGet]
        public async Task<IActionResult> GetArticleByUserId(string userId)
        {
            var articles = _repository.GetArticlesByUserId(userId);

            return View(articles);
        }

        public async Task<Article> GetArticleById(int id)
        {
            return await _repository.Get(id);
        }
    }
}
