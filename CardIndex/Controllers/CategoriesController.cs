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
    [Route("api/categories/")]
    [ApiController]
    public class CategoriesController : Controller
    {
        private readonly ICategoriesService _categoriesService;
        public CategoriesController(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CategoryModel>> GetAll()
        {
            try
            {
                var result = _categoriesService.GetAll();
                return Ok(result);
            }
            catch (CardIndexException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryModel>> GetById(int id)
        {
            try
            {
                var result = await _categoriesService.GetByIdAsync(id);
                return Ok(result);
            }
            catch (CardIndexException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        public async Task<ActionResult> Add([FromBody] CategoryModel model)
        {
            try
            {
                model.Id = _categoriesService.Count() + 1;
                await _categoriesService.AddAsync(model);
                return Ok();
            }
            catch (CardIndexException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> Update(CategoryModel cardModel)
        {
            try
            {
                await _categoriesService.UpdateAsync(cardModel);
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
                await _categoriesService.DeleteByIdAsync(id);
                return Ok();
            }
            catch (CardIndexException ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
