using API.Models.Roles;
using API.Models.Users;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBlog.BLL.Models;
using MyBlog.DAL.UoW;
using System.Data;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private IMapper _mapper;

        public UserController(UserManager<User> userManager, RoleManager<Role> roleManager, IMapper mapper, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _signInManager = signInManager;
        }

        /// <summary>
        /// Получение всех пользователей
        /// </summary>
        /// <returns>Возврат массива тэгов</returns>
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userManager.Users
                .Include(x => x.UserRoles)
                    .ThenInclude(x => x.Role)
                .Select(x => new GetUsersResponse
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    MiddleName = x.MiddleName,
                    BirthDate = x.BirthDate,
                    Email = x.Email,
                    UserName = x.UserName,
                    Roles = x.UserRoles.Select(r => new RoleViews
                    {
                        Id = r.Role.Id,
                        Name = r.Role.Name,
                        Description = r.Role.Description
                    })
                    .ToList()
                })
                .ToArrayAsync();

            return Ok(users);
        }

        /// <summary>
        /// Получение пользователя по ID
        /// </summary>
        /// <param name="id">ID пользователя</param>
        /// <returns>Возврат пользователя</returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetUser([FromRoute] string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return BadRequest($"Ошибка: Пользователь с идентификатором {id} не существует.");

            var userResp = _mapper.Map<User, GetUsersResponse>(user);

            var listNameRoles = await _userManager.GetRolesAsync(user);
            foreach (var nameRole in listNameRoles)
            {
                var role = await _roleManager.FindByNameAsync(nameRole);
                userResp.Roles.Add(_mapper.Map<Role, RoleViews>(role));
            }

            return Ok(userResp);
        }

        /// <summary>
        /// Добавление нового пользователя
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     POST 
        ///     {
        ///        "FirstName" : "Иван",
        ///        "LastName" : "Иванов",
        ///        "MiddleName" : "Иванович",
        ///        "Email" : "ivan@mail.ru",
        ///        "BirthDate" : "2000-01-01",
        ///        "Password" : "Qwerty12345!"
        ///     }
        ///
        /// </remarks>
        /// <param name="request">Пользователь</param>
        /// <returns>Новый пользователь</returns>
        [HttpPost]
        public async Task<IActionResult> Add(AddUserRequest request)
        {
            if ((request.Email == "") || (request.Email == null))
                return BadRequest($"Ошибка: Не указано обязательное поле Email");

            if ((request.Password == "") || (request.Password == null))
                return BadRequest($"Ошибка: Не указано обязательное поле Password");

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user != null)
                return BadRequest($"Ошибка: Пользователь с email {request.Email} уже существует!");

            var newUser = _mapper.Map<AddUserRequest, User>(request);
            var result = await _userManager.CreateAsync(newUser, request.Password);

            if (!result.Succeeded)
            {
                var textError = "";
                foreach (var error in result.Errors)
                {
                    textError = $"{textError}\n{error.Description}";
                }
                return BadRequest($"Ошибка: При создании пользователя произошли ошибки:{textError}");
            }

            return Created(newUser.Id, _mapper.Map<User, GetUsersResponse>(newUser));
        }

        /// <summary>
        /// Удаление пользователя
        /// </summary>
        /// <param name="id">ID пользователя</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return BadRequest($"Ошибка: Пользователь с идентификатором {id} не существует.");

            await _userManager.DeleteAsync(user);

            return Ok();
        }

        /// <summary>
        /// Изменение пользователя
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     POST 
        ///     {
        ///        "FirstName" : "Иван",
        ///        "LastName" : "Иванов",
        ///        "MiddleName" : "Иванович",
        ///        "Email" : "ivan@mail.ru",
        ///        "BirthDate" : "2000-01-01"
        ///     }
        ///
        /// </remarks>
        /// <param name="id">ID пользователя</param>
        /// <param name="request">Пользователь</param>
        /// <returns>Измененный пользователь</returns>
        [HttpPatch]
        [Route("{id}")]
        public async Task<IActionResult> Edit([FromRoute] string id, [FromBody] EditUserRequest request)
        {
            var userById = await _userManager.FindByIdAsync(id);
            if (userById == null)
                return BadRequest($"Ошибка: Пользователь с идентификатором {id} не существует.");

            var userByEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userByEmail != null)
                return BadRequest($"Ошибка: Пользователь с email {request.Email} уже существует!");

            if (!string.IsNullOrEmpty(request.FirstName))
                userById.FirstName = request.FirstName;

            if  (!string.IsNullOrEmpty(request.LastName))
                userById.LastName = request.LastName;

            if (!string.IsNullOrEmpty(request.Email))
            {
                userById.Email = request.Email;
                userById.UserName = request.Email;
            }

            if (!string.IsNullOrEmpty(request.MiddleName))
                userById.MiddleName = request.MiddleName;

            if ((request.BirthDate != DateTime.MinValue) && (request.BirthDate != null))
                userById.BirthDate = request.BirthDate.Value;

            var result = await _userManager.UpdateAsync(userById);

            if (!result.Succeeded)
            {
                var textError = "";
                foreach (var error in result.Errors)
                {
                    textError = $"{textError}\n{error.Description}";
                }
                return BadRequest($"Ошибка: При обновлении пользователя с id = {userById.Id} произошли ошибки:{textError}");
            }

            return Ok(userById);
        }
    }
}
