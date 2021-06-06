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
        private readonly IHttpContextAccessor _httpContextAccessor;
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
                var email = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email).Value;
                if (email == null) throw new CardIndexException("User not found");
                var user = await _userManager.FindByEmailAsync(email);
                if (!_userManager.IsInRoleAsync(user, "Moderator").Result) throw new CardIndexException("You do not have moderator rights");
                CardScore _model = _mapper.Map<CardScore>(model);
                _model.UserId = user.Id; 
                if (_repository.CardScoreRepository.GetAll().FirstOrDefault(x => x.CardId == _model.CardId && x.UserId == _model.UserId) != null)
                    throw new CardIndexException("You have already rated this card");

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
            if (_repository.CardScoreRepository.GetAll().FirstOrDefault(x => x.Id == modelId)==null) throw new CardIndexException("Card scores not found");
            await _repository.CardScoreRepository.DeleteByIdAsync(modelId);
        }

        public IEnumerable<CardScoreModel> GetAll()
        {
            var scores= _repository.CardScoreRepository.GetAll();
            if (scores == null) throw new CardIndexException("Card scores not found");
            var result = (from b in scores
                          select _mapper.Map<CardScore, CardScoreModel>(b)).ToList();
            return result;
        }

        public Task<CardScoreModel> GetByIdAsync(long id)
        {
            if (_repository.CardScoreRepository.GetAll().FirstOrDefault(x => x.Id == id) == null) throw new CardIndexException("Card score not found");
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
