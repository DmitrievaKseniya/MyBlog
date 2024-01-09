using AutoMapper;
using BusinessLogicLayer.Models;
using BusinessLogicLayer.ViewModels;
using DataAccessLayer.Repository;
using DataAccessLayer.UoW;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBlog.Extentions;

namespace MyBlog.Controllers
{
    public class ArticleController : Controller
    {
        private readonly UserManager<User> _userManager;
        private IUnitOfWork _unitOfWork;

        public ArticleController(UserManager<User> userManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        [Route("NewArticle")]
        [HttpPost]
        public async Task<IActionResult> NewArticle(NewArticleViewModel newArticle)
        {
            //var thisUser = User;

            //var result = await _userManager.GetUserAsync(thisUser);

            var result = await _userManager.FindByIdAsync(newArticle.IdUser);

            var repository = _unitOfWork.GetRepository<Article>() as ArticleRepository;

            var item = new Article()
            {
                Title = newArticle.Title,
                Text = newArticle.Text,
                Author = result,
                DateTimeArticle = DateTime.Now,
            };
            await repository.Create(item);

            return View();
        }

        [Route("UpdateArticle")]
        [HttpPost]
        public async Task<IActionResult> UpdateArticle(ArticleEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var article = await GetArticleById(model.IdArticle);

                article.Convert(model);

                var repository = _unitOfWork.GetRepository<Article>() as ArticleRepository;

                await repository.Update(article);

                return View();

            }
            else
            {
                ModelState.AddModelError("", "Некорректные данные");
                return View("Edit", model);
            }
        }

        [Route("DeleteArticle")]
        [HttpPost]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            var article = await GetArticleById(id);

            var repository = _unitOfWork.GetRepository<Article>() as ArticleRepository;

            repository.Delete(article);

            return View();
        }

        [Route("GetAllArticle")]
        [HttpGet]
        public async Task<IActionResult> GetAllArticle()
        {
            var repository = _unitOfWork.GetRepository<Article>() as ArticleRepository;

            var articles = repository.GetAll();

            return View(articles);
        }

        [Route ("GetArticleByUserId")]
        [HttpGet]
        public async Task<IActionResult> GetArticleByUserId(string userId)
        {
            var repository = _unitOfWork.GetRepository<Article>() as ArticleRepository;

            var articles = repository.GetArticlesByUserId(userId);

            return View(articles);
        }

        public async Task<Article> GetArticleById(int id)
        {
            var repository = _unitOfWork.GetRepository<Article>() as ArticleRepository;

            return await repository.Get(id);
        }
    }
}
