﻿using BLL.Interfaces;
using BLL.Models;
using BLL.Validation;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PL.Controllers
{
    [Produces("application/json")]
    [Route("api/cards/")]
    [ApiController]
    public class CardsController : Controller
    {
        private readonly ICardService _cardService;
        public CardsController(ICardService cardService)
        {
            _cardService = cardService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CardModel>> GetAll()
        {
            try
            {
                var result = _cardService.GetAll();
                return Ok(result);
            }
            catch (CardIndexException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CardModel>> GetById(int id)
        {
            try
            {
                var result = await _cardService.GetByIdAsync(id);
                return Ok(result);
            }
            catch (CardIndexException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        public async Task<ActionResult> Add([FromBody] CardModel model)
        {
            try
            {
                await _cardService.AddAsync(model);
                return Ok();
            }
            catch (CardIndexException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> Update(CardModel cardModel)
        {
            try
            {
                await _cardService.UpdateAsync(cardModel);
                return Ok();
            }
            catch (CardIndexException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _cardService.DeleteByIdAsync(id);
                return Ok();
            }
            catch (CardIndexException ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}