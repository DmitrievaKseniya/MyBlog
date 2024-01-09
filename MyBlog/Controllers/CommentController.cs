using BusinessLogicLayer.Models;
using BusinessLogicLayer.ViewModels;
using DataAccessLayer.Repository;
using DataAccessLayer.UoW;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Extentions;

namespace MyBlog.Controllers
{
    public class CommentController : Controller
    {
        private readonly UserManager<User> _userManager;
        private IUnitOfWork _unitOfWork;

        public CommentController(UserManager<User> userManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
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
                Author = result,
            };
            await repository.Create(item);

            return View();
        }

        [Route ("UpdateComment")]
        [HttpPost]
        public async Task<IActionResult> UpdateComment(CommentEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var comment = await GetCommentByIdRep(model.IdComment);

                comment.Convert(model);

                var repository = _unitOfWork.GetRepository<Comment>() as CommentRepository;

                await repository.Update(comment);

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

            var repository = _unitOfWork.GetRepository<Comment>() as CommentRepository;

            repository.Delete(comment);

            return View();
        }

        [Route ("GetAllComments")]
        [HttpGet]
        public async Task<IActionResult> GetAllComments()
        {
            var repository = _unitOfWork.GetRepository<Comment>() as CommentRepository;

            var comments = repository.GetAll();

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
            var repository = _unitOfWork.GetRepository<Comment>() as CommentRepository;

            return await repository.Get(id);
        }
    }
}
