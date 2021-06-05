using AutoMapper;
using BLL.Interfaces;
using BLL.Models;
using BLL.Validation;
using DAL.Entities;
using DAL.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class CardScoreService : ICardScoreService
    {
        private readonly IUnitOfWork _repository;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private IHttpContextAccessor _httpContextAccessor;
        public CardScoreService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _repository = unitOfWork;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async  Task AddAsync(CardScoreModel model)
        {
            try
            {
                CardScore _model = _mapper.Map<CardScore>(model);
                var email = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x=>x.Type=="email").Value;
                var user = await _userManager.FindByEmailAsync(email);
                await _repository.CardScoreRepository.AddAsync(_model);
            }
            catch (Exception ex)
            {
                throw new CardIndexException(ex.Message);
            }
        }

        public int Count()
        {
           return _repository.CardScoreRepository.GetAll().Count();
        }

        public async Task DeleteByIdAsync(long modelId)
        {
            if (_repository.CardScoreRepository.GetAll().Any(x => x.Id == modelId)) throw new CardIndexException();
            await _repository.CardScoreRepository.DeleteByIdAsync(modelId);
        }

        public IEnumerable<CardScoreModel> GetAll()
        {
            var cardScores = _repository.CardScoreRepository.GetAll();
            var result = (from b in cardScores
                          select _mapper.Map<CardScore, CardScoreModel>(b)).ToList();
            return result;
        }

        public Task<CardScoreModel> GetByIdAsync(long id)
        {
            var result = _mapper.Map<CardScore, CardScoreModel>(_repository.CardScoreRepository.GetByIdAsync(id).Result);
            return Task.FromResult<CardScoreModel>(result);
        }

        public Task UpdateAsync(CardScoreModel model)
        {
            _repository.CardScoreRepository.Update(_mapper.Map<CardScore>(model));
            return _repository.SaveAsync();
        }
    }
}
