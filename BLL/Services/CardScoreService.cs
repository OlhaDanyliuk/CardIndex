using AutoMapper;
using BLL.Interfaces;
using BLL.Models;
using BLL.Validation;
using DAL.Entities;
using DAL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class CardScoreService : ICardScoreService
    {
        private readonly IUnitOfWork _repository;
        private readonly IMapper _mapper;
        public CardScoreService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _repository = unitOfWork;
        }

        public Task AddAsync(CardScoreModel model)
        {
            try
            {
                CardScore _model = _mapper.Map<CardScore>(model);
                _repository.CardScoreRepository.AddAsync(_model);
                return _repository.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new CardIndexException(ex.Message);
            }
        }

        public Task DeleteByIdAsync(int modelId)
        {
            if (_repository.CardScoreRepository.GetAll().Any(x => x.Id == modelId)) throw new CardIndexException();
            _repository.CardScoreRepository.DeleteByIdAsync(modelId);
            return _repository.SaveAsync();
        }

        public IEnumerable<CardScoreModel> GetAll()
        {
            var cardScores = _repository.CardScoreRepository.GetAll();
            var result = (from b in cardScores
                          select _mapper.Map<CardScore, CardScoreModel>(b)).ToList();
            return result;
        }

        public Task<CardScoreModel> GetByIdAsync(int id)
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
