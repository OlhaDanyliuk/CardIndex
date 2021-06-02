using BLL.Models;
using DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministrationController : ControllerBase
    {
        private readonly RoleManager<IdentityRole<long>> roleManager;
        private readonly UserManager<User> userManager;
        public AdministrationController(RoleManager<IdentityRole<long>> _roleManager, UserManager<User> _userManager)
        {
            roleManager = _roleManager;
            userManager = _userManager;
        }
        [HttpGet]
        public List<IdentityRole<long>> Get()
        {
            return roleManager.Roles.ToList();
        }

        [HttpPost]
        public async Task<ActionResult> CreateRole(UserRole role)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(x => x.Errors.Select(x => x.ErrorMessage)));
            }
            IdentityRole<long> identityRole = new IdentityRole<long>
            {
                Name = role.RoleName
            };
            IdentityResult result = await roleManager.CreateAsync(identityRole);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return BadRequest();
            }
            return Ok();
        }

        [HttpDelete]

        public async Task<ActionResult> DeleteRole(string name)
        {
            IdentityRole<long> identityRole = roleManager.Roles.First(x => x.Name == name);
            if (identityRole == null)
            {
                return BadRequest(ModelState.Values.SelectMany(x => x.Errors.Select(x => x.ErrorMessage)));
            }
            IdentityResult result = await roleManager.DeleteAsync(identityRole);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return BadRequest();
            }
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Edit(string userId, string role)
        {
            var user = await userManager.FindByIdAsync(userId);
            var oldRoleNames = await userManager.GetRolesAsync(user);

            if (oldRoleNames.First() != role)
            {
                await userManager.RemoveFromRoleAsync(user, oldRoleNames.First());
                await userManager.AddToRoleAsync(user, role);
            }
            return Ok();

        }
    }
}
