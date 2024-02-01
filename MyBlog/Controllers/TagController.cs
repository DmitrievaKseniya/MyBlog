using MyBlog.BLL.Models;
using MyBlog.BLL.ViewModels;
using MyBlog.DAL.Repository;
using MyBlog.DAL.UoW;
using Microsoft.AspNetCore.Mvc;
using MyBlog.WebService.Extentions;
using Microsoft.AspNetCore.Authorization;

namespace MyBlog.WebService.Controllers
{
    [Route("[controller]/[action]")]
    public class TagController : Controller
    {
        private IUnitOfWork _unitOfWork;
        private TagRepository _repository;

        public TagController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _repository = _unitOfWork.GetRepository<Tag>() as TagRepository;
        }

        [Authorize(Roles = "moderator, admin")]
        [HttpPost]
        public async Task<IActionResult> NewTag(TagNewViewModel newTag)
        {
            var item = new Tag()
            {
                Name = newTag.Name,
            };
            await _repository.Create(item);

            return RedirectToAction("GetAllTags", "Tag");
        }

        [Authorize(Roles = "moderator, admin")]
        [HttpGet]
        public async Task<IActionResult> NewTagPage()
        {
            var model = new TagNewViewModel();

            return View("NewTag", model);
        }

        [Authorize(Roles = "moderator, admin")]
        [HttpPost]
        public async Task<IActionResult> UpdateTag(TagEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var tag = await GetTagByIdRep(model.IdTag);

                tag.Convert(model);
                
                await _repository.Update(tag);

                return RedirectToAction("GetAllTags", "Tag");

            }
            else
            {
                ModelState.AddModelError("", "Некорректные данные");
                return View(model);
            }
        }

        [Authorize(Roles = "moderator, admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteTag(int id)
        {
            var tag = await GetTagByIdRep(id);

            await _repository.Delete(tag);

            return RedirectToAction("GetAllTags", "Tag");
        }

        [Authorize(Roles = "moderator, admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllTags()
        {
            var tags = await _repository.GetAllTagsWithArticles();

            var model = tags
                .Select(x => new TagsListViewModel()
                {
                    TagsList = x,
                    NumberArticles = x.Articles.Count
                })
                .ToArray();

            return View("TagsList", model);
        }

        [Authorize(Roles = "moderator, admin")]
        [Route ("GetTagById/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetTagById(int id)
        {
            var tag = await GetTagByIdRep(id);

            var model = new TagEditViewModel()
            {
                IdTag = tag.Id,
                Name = tag.Name,
            };

            return View("EditTag", model);
        }

        public async Task<Tag> GetTagByIdRep(int id)
        {
           return await _repository.Get(id);
        }
    }
}
