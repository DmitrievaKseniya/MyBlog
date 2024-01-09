using BusinessLogicLayer.Models;
using BusinessLogicLayer.ViewModels;
using DataAccessLayer.Repository;
using DataAccessLayer.UoW;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Extentions;

namespace MyBlog.Controllers
{
    public class TagController : Controller
    {
        private IUnitOfWork _unitOfWork;

        public TagController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Route ("NewTag")]
        [HttpPost]
        public async Task<IActionResult> NewTag(TagNewViewModel newTag)
        {
            var repository = _unitOfWork.GetRepository<Tag>() as TagRepository;

            var item = new Tag()
            {
                Name = newTag.Name,
            };
            await repository.Create(item);

            return View();
        }

        [Route ("UpdateTag")]
        [HttpPost]
        public async Task<IActionResult> UpdateTag(TagEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var tag = await GetTagByIdRep(model.IdTag);

                tag.Convert(model);

                var repository = _unitOfWork.GetRepository<Tag>() as TagRepository;

                await repository.Update(tag);

                return View();

            }
            else
            {
                ModelState.AddModelError("", "Некорректные данные");
                return View("Edit", model);
            }
        }

        [Route ("DeleteTag")]
        [HttpPost]
        public async Task<IActionResult> DeleteTag(int id)
        {
            var tag = await GetTagByIdRep(id);

            var repository = _unitOfWork.GetRepository<Tag>() as TagRepository;

            repository.Delete(tag);

            return View();
        }

        [Route ("GetAllTags")]
        [HttpGet]
        public async Task<IActionResult> GetAllTags()
        {
            var repository = _unitOfWork.GetRepository<Tag>() as TagRepository;

            var tags = repository.GetAll();

            return View(tags);
        }

        [Route ("GetTagById")]
        [HttpGet]
        public async Task<IActionResult> GetTagById(int id)
        {
            var tag = await GetTagByIdRep(id);

            return View(tag);
        }

        public async Task<Tag> GetTagByIdRep(int id)
        {
            var repository = _unitOfWork.GetRepository<Tag>() as TagRepository;

            return await repository.Get(id);
        }
    }
}
