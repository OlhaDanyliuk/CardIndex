using BLL.Interfaces;
using BLL.Models;
using DAL.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DAL.Entities;
using BLL.Validation;
using System.Linq;

namespace BLL.Services
{
    public class CardService : ICardService
    {
        private readonly IUnitOfWork _repository;
        private readonly IMapper _mapper;
        public CardService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _repository = unitOfWork;
            CalculateAverageScore();

        }
        
        public void CalculateAverageScore()
        {
            foreach(var card in _repository.CardRepository.GetAll())
            {
                if (card.Assessment.Count != 0)
                    card.AverageScore = Math.Round(card.Assessment.Select(x => x.Assessment).Average(),2);
                else
                    card.AverageScore = 0;
            }
        }
        public async Task AddAsync(CardModel model)
        {
            try { 
                Card _model = _mapper.Map<Card>(model);
                _model.Category = _repository.CategoryRepository.GetByIdAsync(model.CategoryId).Result;
                await _repository.CardRepository.AddAsync(_model);
                await _repository.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new CardIndexException(ex.Message);
            }
        }

        public async Task DeleteByIdAsync(long modelId)
        {
            if (_repository.CardRepository.GetAll().FirstOrDefault(x => x.Id == modelId)==null) throw new CardIndexException("Card not found!");
            await _repository.CardRepository.DeleteByIdAsync(modelId);
            await _repository.SaveAsync();
        }

        public IEnumerable<CardModel> GetAll()
        {
            var cards = _repository.CardRepository.GetAll();
            if (cards.Count() == 0) throw new CardIndexException("Cards not found!");
            var result = (from b in cards
                          select _mapper.Map<Card, CardModel>(b)).ToList();
            return result;
        }

        public ICollection<CardModel> GetAllWithDetails()
        {
            if (_repository.CardRepository.GetAll() == null) throw new CardIndexException("Cards not found!");
            return _repository.CardRepository.GetAllWithDetails().Select(x=>_mapper.Map<CardModel>(x)).ToList();
        }

        public double GetAverageScoreByCardId(long id)
        {
            if (_repository.CardRepository.GetAll().FirstOrDefault(x => x.Id == id) == null) throw new CardIndexException("Card not found!");
            return _repository.CardRepository.GetAverageScoreByCardId(id);
        }

        public Task<CardModel> GetByIdAsync(long id)
        {
            if (_repository.CardRepository.GetAll().FirstOrDefault(x => x.Id == id) == null) throw new CardIndexException("Card not found!");
            var result = _mapper.Map<Card, CardModel>(_repository.CardRepository.GetByIdAsync(id).Result);
            return Task.FromResult<CardModel>(result);
        }

        public CardModel GetWithDetailsById(long id)
        {
            if (_repository.CardRepository.GetAll().FirstOrDefault(x => x.Id == id) == null) throw new CardIndexException("Card not found!");
            return _mapper.Map<CardModel>(_repository.CardRepository.GetWithDetailsById(id));
        }

        public Task UpdateAsync(CardModel model)
        {
            if (String.IsNullOrEmpty(model.Name) || String.IsNullOrEmpty(model.Text) || model.CategoryId == 0 ) throw new CardIndexException("Inccorect card model data");
            if (model.CategoryId != 0 && _repository.CategoryRepository.GetAll().FirstOrDefault(x=>x.Id==model.CategoryId)==null) throw new CardIndexException("Category not found");
            if(_repository.CardRepository.GetByIdAsync(model.Id).Result==null) throw new CardIndexException("Card not found");
            _repository.CardRepository.Update(_mapper.Map<Card>(model));
            return _repository.SaveAsync();
        }
        public int Count()
        {
            return _repository.CardRepository.GetAll().Count();
        }
    }
}
