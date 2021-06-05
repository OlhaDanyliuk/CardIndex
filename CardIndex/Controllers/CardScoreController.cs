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
    [Route("api/cardscore/")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CardScoreController : Controller
    {
        private readonly ICardScoreService _cardScoreService;
        public CardScoreController(ICardScoreService cardScoreService)
        {
            _cardScoreService = cardScoreService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Moderator")]
        public ActionResult<IEnumerable<CardScoreModel>> GetAll()
        {
            try
            {
                var result = _cardScoreService.GetAll();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<ActionResult<CardScoreModel>> GetById(int id)
        {
            try
            {
                var result = await _cardScoreService.GetByIdAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("create")]
        [Authorize(Roles = "Moderator")]
        public async Task<ActionResult> Add([FromBody] CardScoreModel model)
        {
            try
            {
                await _cardScoreService.AddAsync(model);
                return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update")]
        [Authorize(Roles = "Moderator")]
        public async Task<ActionResult> Update(CardScoreModel cardModel)
        {
            try
            {
                await _cardScoreService.UpdateAsync(cardModel);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("remove/{id}")]
        [Authorize(Roles = "Moderator")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _cardScoreService.DeleteByIdAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
