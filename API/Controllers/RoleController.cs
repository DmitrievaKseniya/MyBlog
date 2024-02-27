using API.Models.Roles;
using API.Models.Users;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBlog.BLL.Models;
using MyBlog.BLL.ViewModels;
using System.Data;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;
        private IMapper _mapper;

        public RoleController(RoleManager<Role> roleManager, IMapper mapper, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _mapper = mapper;
            _userManager = userManager;
        }

        /// <summary>
        /// Получение всех ролей
        /// </summary>
        /// <returns>Возврат массива ролей</returns>
        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _roleManager.Roles
                .Select(x => new GetRolesResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                })
                .ToArrayAsync();

            return Ok(roles);
        }

        /// <summary>
        /// Получение роли по ID
        /// </summary>
        /// <param name="id">ID роли</param>
        /// <returns>Возврат роли</returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetRole([FromRoute] string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            
            if (role == null) 
                return BadRequest($"Ошибка: Роль с идентификатором {id} не существует.");

            var roleResp = _mapper.Map<Role, GetRolesResponse>(role);

            return Ok(roleResp);
        }

        /// <summary>
        /// Получение ролей конкретного пользователя по ID пользователя
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        /// <returns>Возврат массива ролей конкретного пользователя</returns>
        [HttpGet]
        [Route("{userId}")]
        public async Task<IActionResult> GetUsersRoles([FromRoute] string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return BadRequest($"Ошибка: Пользователь с идентификатором {userId} не существует.");

            var listNameRoles = await _userManager.GetRolesAsync(user);

            var listUsersRoles = new List<GetRolesResponse>();
            foreach (var nameRole in listNameRoles)
            {
                var role = await _roleManager.FindByNameAsync(nameRole);
                listUsersRoles.Add(_mapper.Map<Role, GetRolesResponse>(role));
            }

            return Ok(listUsersRoles);
        }

        /// <summary>
        /// Добавление новой роли
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     POST 
        ///     {
        ///        "Name" : "Наименование новой роли", 
        ///        "Description" : "Описание новой роли"
        ///     }
        ///
        /// </remarks>
        /// <param name="request">Роль</param>
        /// <returns>Новая роль</returns>
        [HttpPost]
        public async Task<IActionResult> Add(AddRoleRequest request)
        {
            if ((request.Name == "") || (request.Name == null))
                return BadRequest($"Ошибка: Не указано обязательное поле Name");

            if (await _roleManager.RoleExistsAsync(request.Name))
                return BadRequest($"Ошибка: Роль с именем {request.Name} уже существует!");

            var newRole = new Role(request.Name, request.Description);
            var result = await _roleManager.CreateAsync(newRole);
            if (!result.Succeeded)
            {
                var textError = "";
                foreach (var error in result.Errors)
                {
                    textError = $"{textError}\n{error.Description}";
                }
                return BadRequest($"Ошибка: При создании роли произошли ошибки:{textError}");
            }

            return Created(newRole.Id, _mapper.Map<Role, GetRolesResponse>(newRole));
        }

        /// <summary>
        /// Удаление роли
        /// </summary>
        /// <param name="id">ID роли</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
                return BadRequest($"Ошибка: Роль с идентификатором {id} не существует.");

            await _roleManager.DeleteAsync(role);

            return Ok();
        }

        /// <summary>
        /// Изменение роли
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     POST 
        ///     {
        ///        "Name" : "Новое наименование роли", 
        ///        "Description" : "Новое описание роли"
        ///     }
        ///
        /// </remarks>
        /// <param name="id">ID роли</param>
        /// <param name="request">Роль</param>
        /// <returns>Измененная роль</returns>
        [HttpPatch]
        [Route("{id}")]
        public async Task<IActionResult> Edit([FromRoute] string id, [FromBody] EditRoleRequest request)
        {
            var roleById = await _roleManager.FindByIdAsync(id);
            if (roleById == null)
                return BadRequest($"Ошибка: Роль с идентификатором {id} не существует.");

            if (await _roleManager.RoleExistsAsync(request.Name))
                return BadRequest($"Ошибка: Роль с именем {request.Name} уже существует!");

            if (!string.IsNullOrEmpty(request.Name))
                roleById.Name = request.Name;

            if (!string.IsNullOrEmpty(request.Description))
                roleById.Description = request.Description;

            var result = await _roleManager.UpdateAsync(roleById);

            if (!result.Succeeded)
            {
                var textError = "";
                foreach (var error in result.Errors)
                {
                    textError = $"{textError}\n{error.Description}";
                }
                return BadRequest($"Ошибка: При обновлении роли с id = {roleById.Id} произошли ошибки:{textError}");
            }

            return Ok(roleById);
        }
    } 
}
