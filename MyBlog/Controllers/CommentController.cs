using MyBlog.BLL.Models;
using MyBlog.BLL.ViewModels;
using MyBlog.DAL.Repository;
using MyBlog.DAL.UoW;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyBlog.WebService.Extentions;

namespace MyBlog.WebService.Controllers
{
    public class CommentController : Controller
    {
        private readonly UserManager<User> _userManager;
        private IUnitOfWork _unitOfWork;
        private CommentRepository _repository;

        public CommentController(UserManager<User> userManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _repository = _unitOfWork.GetRepository<Comment>() as CommentRepository;
        }

        [Route ("NewComment")]
        [HttpPost]
        public async Task<IActionResult> NewComment(CommentNewViewModel newComment)
        {
            var result = await _userManager.FindByIdAsync(newComment.IdUser);

            var repository = _unitOfWork.GetRepository<Comment>() as CommentRepository;

            var item = new Comment()
            {
                Text = newComment.Text,
                AuthorId = result.Id,
                ArticleId = newComment.IdArticle
            };
            await repository.Create(item);

            return RedirectToAction("ViewArticle", "Article", new {id = newComment.IdArticle});
        }

        [Route ("UpdateComment")]
        [HttpPost]
        public async Task<IActionResult> UpdateComment(CommentEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var comment = await GetCommentByIdRep(model.IdComment);

                comment.Convert(model);

                await _repository.Update(comment);

                return View();

            }
            else
            {
                ModelState.AddModelError("", "Некорректные данные");
                return View("Edit", model);
            }
        }

        [Route ("DeleteComment")]
        [HttpPost]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = await GetCommentByIdRep(id);

            _repository.Delete(comment);

            return View();
        }

        [Route ("GetAllComments")]
        [HttpGet]
        public async Task<IActionResult> GetAllComments()
        { 

            var comments = _repository.GetAll();

            return View(comments);
        }

        [Route ("GetCommentById")]
        [HttpGet]
        public async Task<IActionResult> GetCommentById(int id)
        {
            var comment = await GetCommentByIdRep(id);

            return View(comment);
        }

        public async Task<Comment> GetCommentByIdRep(int id)
        { 
            return await _repository.Get(id);
        }

        public async Task<List<Comment>> GetCommentsByArticleId(int id)
        {
            return await _repository.GetByArticleId(id);
        }
    }
}
