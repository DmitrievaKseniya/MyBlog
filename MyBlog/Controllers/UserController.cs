using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBlog.BLL.Models;
using MyBlog.BLL.ViewModels;
using MyBlog.DAL.Repository;
using MyBlog.DAL.UoW;
using MyBlog.WebService.Extentions;
using System.Data;

namespace MyBlog.WebService.Controllers
{
    [Route("[controller]/[action]")]
    public class UserController : Controller
    {
        private IMapper _mapper;

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Role> _roleManager;
        private IUnitOfWork _unitOfWork;

        public UserController(IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> MyPage()
        {
            var user = User;

            var result = await _userManager.GetUserAsync(user);

            var model = new UserViewModel(result);

            model.Roles = await GetRolesByUser(model.User);

            model.Articles = await GetArticlesByUser(model.User);

            return View("User", model);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> UserPage(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            var model = new UserViewModel(user);

            model.Roles = await GetRolesByUser(model.User);

            model.Articles = await GetArticlesByUser(model.User);

            return View("UserPage", model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View("Login");
        }

        [HttpGet]
        public async Task<IActionResult> RegisterPage()
        {
            var model = new RegisterViewModel();

            return View("Register", model);
        }

        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _mapper.Map<User>(model);

                var result = await _userManager.CreateAsync(user, model.PasswordReg);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "employee");

                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("MyPage", "User");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> NewUserPage()
        {
            var model = new UserNewViewModel()
            {
                AllRoles = _roleManager.Roles.ToList()
            };

            return View("NewUser", model);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> NewUser(UserNewViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _mapper.Map<User>(model);

                var result = await _userManager.CreateAsync(user, model.PasswordReg);

                if (result.Succeeded)
                {
                    if (model.SelectedNameRoles != null)
                    {
                        foreach (var role in model.SelectedNameRoles)
                        {
                            await _userManager.AddToRoleAsync(user, role);
                        }
                    }

                    await _userManager.AddToRoleAsync(user, "employee");

                    return RedirectToAction("GetAllUsers", "User");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            model.AllRoles = _roleManager.Roles.ToList();
            return View(model);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var user = User;

            var result = await _userManager.GetUserAsync(user);

            var editmodel = _mapper.Map<UserEditViewModel>(result);

            editmodel.AllRoles = _roleManager.Roles.ToList();

            editmodel.SelectedRoles = await GetRolesByUser(result);

            return View("Edit", editmodel);
        }

        [Authorize(Roles = "admin")]
        [Route("EditFromUserList/{id}")]
        [HttpGet]
        public async Task<IActionResult> EditFromUserList(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            var editmodel = _mapper.Map<UserEditViewModel>(user);

            editmodel.AllRoles = _roleManager.Roles.ToList();

            editmodel.SelectedRoles = await GetRolesByUser(user);

            return View("EditUser", editmodel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Update(UserEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.UserId);

                user.Convert(model);

                var result = await _userManager.UpdateAsync(user);

                var oldRolesUser = await GetRolesByUser(user);
                foreach (var oldRole in oldRolesUser)
                {
                    if (oldRole.Name != "employee")
                    {
                        if (model.SelectedNameRoles == null)
                        {
                            await _userManager.RemoveFromRoleAsync(user, oldRole.Name);
                        }
                        else if (!model.SelectedNameRoles.Contains(oldRole.Name))
                        {
                            await _userManager.RemoveFromRoleAsync(user, oldRole.Name);
                        }
                    }
                }
                if (model.SelectedNameRoles != null)
                {
                    await _userManager.AddToRolesAsync(user, model.SelectedNameRoles);
                }
                if (result.Succeeded)
                {
                    return RedirectToAction("MyPage", "User");
                }
                else
                {
                    ModelState.AddModelError("", "Некорректные данные");
                }
            }
            else
            {
                ModelState.AddModelError("", "Некорректные данные");
            }

            model.AllRoles = _roleManager.Roles.ToList();
            return View("Edit", model);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> UpdateFromUserList(UserEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.UserId);

                user.Convert(model);

                var result = await _userManager.UpdateAsync(user);

                var oldRolesUser = await GetRolesByUser(user);
                foreach (var oldRole in oldRolesUser)
                {
                    if (oldRole.Name != "employee")
                    {
                        if (model.SelectedNameRoles == null)
                        {
                            await _userManager.RemoveFromRoleAsync(user, oldRole.Name);
                        }
                        else if (!model.SelectedNameRoles.Contains(oldRole.Name))
                        {
                            await _userManager.RemoveFromRoleAsync(user, oldRole.Name);
                        }
                    }
                }
                if (model.SelectedNameRoles != null)
                {
                    await _userManager.AddToRolesAsync(user, model.SelectedNameRoles);
                }
                if (result.Succeeded)
                {
                    return RedirectToAction("GetAllUsers", "User");
                }
                else
                {
                    ModelState.AddModelError("", "Некорректные данные");
                }
            }
            else
            {
                ModelState.AddModelError("", "Некорректные данные");
            }

            model.AllRoles = _roleManager.Roles.ToList();
            return View("EditUser", model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteUser()
        {
            var user = User;

            var result = await _userManager.GetUserAsync(user);

            await _userManager.DeleteAsync(result);

            return RedirectToAction("Logout", "User");
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteUserFromUserList(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            await _userManager.DeleteAsync(user);

            return RedirectToAction("GetAllUsers", "User");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManager.Users.ToArrayAsync();

            var modelList = new List<UserViewModel>();
            foreach (var user in users)
            {
                var roles = await GetRolesByUser(user);
                modelList.Add(new UserViewModel(user, roles));
            }

            var model = modelList.ToArray();

            return View("UsersList", model);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("MyPage", "User");
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<List<Role>> GetRolesByUser(User user)
        {
            var listNameRoles = await _userManager.GetRolesAsync(user);

            var roles = new List<Role>();

            foreach (var nameRole in listNameRoles) 
            {
                roles.Add(await _roleManager.FindByNameAsync(nameRole));
            }
            return roles;
        }

        public async Task<List<Article>> GetArticlesByUser(User user)
        {
            var repositoryArticle = _unitOfWork.GetRepository<Article>() as ArticleRepository;

            var articles = await repositoryArticle.GetArticlesByUserId(user.Id);

            return articles;
        }
    }
}
