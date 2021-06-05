using BLL.Interfaces;
using BLL.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PL.Controllers
{
    [Produces("application/json")]
    [Route("api/roles/")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Moderator")]
        public ActionResult<IEnumerable<UserRoleModel>> GetAll()
        {
            try
            {
                var result = _roleService.GetAll();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<ActionResult<UserRoleModel>> GetById(int id)
        {
            try
            {
                var result = await _roleService.GetByIdAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("create")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Add([FromBody] UserRoleModel model)
        {
            try
            {
                await _roleService.AddAsync(model);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Update(UserRoleModel model)
        {
            try
            {
                await _roleService.UpdateAsync(model);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("remove/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _roleService.DeleteByIdAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
