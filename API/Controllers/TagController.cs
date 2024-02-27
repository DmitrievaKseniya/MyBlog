using API.Models.Articles;
using API.Models.Tags;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyBlog.BLL.Models;
using MyBlog.DAL.Repository;
using MyBlog.DAL.UoW;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class TagController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        private TagRepository _repository;
        private IMapper _mapper;

        public TagController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _repository = _unitOfWork.GetRepository<Tag>() as TagRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Получение всех тэгов
        /// </summary>
        /// <returns>Возврат массива тэгов</returns>
        [HttpGet]
        public async Task<IActionResult> GetTags()
        {
            var tags = await _repository.GetAll();

            var tagsResp = tags
                .Select(x => new GetTagsResponse()
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToArray();

            return Ok(tagsResp);
        }

        /// <summary>
        /// Получение тэга по ID
        /// </summary>
        /// <param name="id">ID тэга</param>
        /// <returns>Возврат тэга</returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetTag([FromRoute] int id)
        {
           var tag = await _repository.Get(id);
            if (tag == null)
                return BadRequest($"Ошибка: Тэга с идентификатором {id} не существует.");

            var tagResp = _mapper.Map<Tag, GetTagsResponse>(tag);

            return Ok(tagResp);
        }

        /// <summary>
        /// Добавление нового тэга
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     POST 
        ///     {
        ///        "Name" : "Наименование нового тэга"
        ///     }
        ///
        /// </remarks>
        /// <param name="request">Тэг</param>
        /// <returns>Новый тэг</returns>
        [HttpPost]
        public async Task<IActionResult> Add(AddTagRequest request)
        {
            if ((request.Name == null) || (request.Name == null))
            {
                return BadRequest($"Ошибка: Не указано обязательное поле Name");
            }
            var newTag = new Tag()
            {
                Name = request.Name
            };

            await _repository.Create(newTag);
            
            return Created(newTag.Id.ToString(), _mapper.Map<Tag, GetTagsResponse>(newTag));
        }

        /// <summary>
        /// Удаление тэга
        /// </summary>
        /// <param name="id">ID тэга</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var tag = await _repository.Get(id);
            if (tag == null)
                return BadRequest($"Ошибка: Тэга с идентификатором {id} не существует.");

            _repository.Delete(tag);

            return Ok();
        }

        /// <summary>
        /// Изменение тэга
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     POST 
        ///     {
        ///        "Name" : "Новое наименование тэга"
        ///     }
        ///
        /// </remarks>
        /// <param name="id">ID тэга</param>
        /// <param name="request">Тэг</param>
        /// <returns>Измененный тэг</returns>
        [HttpPatch]
        [Route("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] EditTagRequest request)
        {
            var tag = await _repository.Get(id);
            if (tag == null)
                return BadRequest($"Ошибка: Тэга с идентификатором {id} не существует.");

            if (!string.IsNullOrEmpty(request.Name))
                tag.Name = request.Name;

            await _repository.Update(tag);

            var tagResp = _mapper.Map<Tag, GetTagsResponse>(tag);

            return Ok(tagResp);
        }
    }
}
