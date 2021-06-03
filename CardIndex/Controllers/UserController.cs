using BLL.Interfaces;
using BLL.Models;
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
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<UserModel>> GetAll()
        {
            try
            {
                var result = _userService.GetAll();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
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
        [HttpDelete("remove/{id}")]
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
        public async Task<ActionResult> Register(SignupModel userRegistration)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(x => x.Errors.Select(x => x.ErrorMessage)));
            }
            var authResponse = await _userService.Signup(userRegistration);
            if (!authResponse.Success)
            {
                return BadRequest(authResponse.Errors);
            }
            return Ok(authResponse.Token);
        }

        [HttpPost("login")]
        public ActionResult Login(LoginModel login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(x => x.Errors.Select(x => x.ErrorMessage)));
            }
            var authResponse = _userService.Login(login);
            if (!authResponse.Success)
            {
                return BadRequest(authResponse.Errors);
            }
            return Ok(authResponse.Token);

        }

    }
}
