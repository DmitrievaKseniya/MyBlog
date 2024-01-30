using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyBlog.BLL.Models;
using MyBlog.BLL.ViewModels;
using MyBlog.WebService.Extentions;

namespace MyBlog.WebService.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<Role> _roleManager;

        public RoleController(RoleManager<Role> roleManager)
        {
            _roleManager = roleManager;
        }

        [Authorize(Roles = "admin")]
        [Route("GetAllRoles")]
        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = _roleManager.Roles.ToList();

            var model = roles
                .Select(x => new RolesListViewModel()
                {
                    RolesList = x,
                })
                .ToArray();

            return View("RolesList", model);
        }

        [Authorize(Roles = "admin")]
        [Route("GetRoleById/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetRoleById(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            var model = new RoleEditViewModel()
            {
                IdRole = role.Id,
                Name = role.Name,
                Description = role.Description
            };

            return View("EditRole", model);
        }

        [Authorize(Roles = "admin")]
        [Route("UpdateRole")]
        [HttpPost]
        public async Task<IActionResult> UpdateRole(RoleEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByIdAsync(model.IdRole);

                role.Convert(model);

                var result = await _roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("GetAllRoles", "Role");
                }
                else
                {
                    return RedirectToAction("GetRoleById", "Role");
                }
            }
            else
            {
                ModelState.AddModelError("", "Некорректные данные");
                return View("Edit", model);
            }
        }

        [Authorize(Roles = "admin")]
        [Route("NewRolePage")]
        [HttpGet]
        public async Task<IActionResult> NewRolePage()
        {
            var model = new RoleNewViewModel();

            return View("NewRole", model);
        }

        [Authorize(Roles = "admin")]
        [Route("NewRole")]
        [HttpPost]
        public async Task<IActionResult> NewRole(RoleNewViewModel newRole)
        {
            if (!(await _roleManager.RoleExistsAsync(newRole.Name)))
            {
                var item = new Role(newRole.Name, newRole.Description);
                await _roleManager.CreateAsync(item);
                return RedirectToAction("GetAllRoles", "Role");
            }
            else
            {
                ModelState.AddModelError("", "Такая роль уже есть");
                return View("Edit");
            }
        }

        [Authorize(Roles = "admin")]
        [Route("DeleteRole")]
        [HttpPost]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            await _roleManager.DeleteAsync(role);

            return RedirectToAction("GetAllRoles", "Role");
        }
    }
}
