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
    [Route("api/users/")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]

        [Authorize(Roles = "Admin, Moderator")]
        public async Task<ActionResult<IEnumerable<UserModel>>> GetAll()
        {
            try
            {
                var result = await _userService.GetAllAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("usersRole")]
        [Authorize(Roles = "Admin, Moderator")]
        public ActionResult<IEnumerable<UserModel>> GetUsersRole(string userRole)
        {
            try
            {
                var result = _userService.GetUsersRole(userRole);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<ActionResult<CategoryModel>> GetById(int id)
        {
            try
            {
                var result = await _userService.GetByIdAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("create")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Add([FromBody] UserModel model)
        {
            try
            {
                await _userService.AddAsync(model);
                return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update")]
        [Authorize]
        public async Task<ActionResult> Update(UserModel userModel)
        {
            try
            {
                await _userService.UpdateAsync(userModel);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("changeUserRole")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> ChangeUserRole(UserModel userModel)
        {
            try
            {
                await _userService.ChangeUserRole(userModel);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("removeUserRole")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> RemoveUserRole(UserModel user)
        {
            try
            {
                await _userService.RemoveUserRole(user);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("remove/{id}")]
        [Authorize]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _userService.DeleteByIdAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("signup")]
        [AllowAnonymous]
        public async Task<ActionResult> Register(SignupModel userRegistration)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(x => x.Errors.Select(x => x.ErrorMessage)));
            }
            var authResponse = await _userService.SignupAsync(userRegistration);
            if (!authResponse.Success)
            {
                return BadRequest(authResponse.Errors);
            }
            return Ok(authResponse.Token);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginModel login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(x => x.Errors.Select(x => x.ErrorMessage)));
            }
            var authResponse = await _userService.LoginAsync(login);
            if (!authResponse.Success)
            {
                return BadRequest(authResponse.Errors);
            }
            return Ok(authResponse.Token);

        }

    }
}
