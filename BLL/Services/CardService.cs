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
        }

        public Task AddAsync(CardModel model)
        {
            try
            {
                Card _model = _mapper.Map<Card>(model);
                _repository.CardRepository.AddAsync(_model);
                return _repository.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new CardIndexException(ex.Message);
            }
        }

        public Task DeleteByIdAsync(int modelId)
        {
            if (_repository.CardRepository.GetAll().Any(x => x.Id == modelId)) throw new CardIndexException();
            _repository.CardRepository.DeleteByIdAsync(modelId);
            return _repository.SaveAsync();
        }

        public IEnumerable<CardModel> GetAll()
        {
            var cards = _repository.CardRepository.GetAll();
            var result = (from b in cards
                          select _mapper.Map<Card, CardModel>(b)).ToList();
            return result;
        }

        public ICollection<CardModel> GetAllWithDetails()
        {
            return _repository.CardRepository.GetAllWithDetails().Select(x=>_mapper.Map<CardModel>(x)).ToList();
        }

        public double GetAverageScoreByCardId(long id)
        {
            return _repository.CardRepository.GetAverageScoreByCardId(id);
        }

        public Task<CardModel> GetByIdAsync(int id)
        {
            var result = _mapper.Map<Card, CardModel>(_repository.CardRepository.GetByIdAsync(id).Result);
            return Task.FromResult<CardModel>(result);
        }

        public CardModel GetWithDetailsById(long id)
        {
            return _mapper.Map<CardModel>(_repository.CardRepository.GetWithDetailsById(id));
        }

        public Task UpdateAsync(CardModel model)
        {
            //if (String.IsNullOrEmpty(model.) || String.IsNullOrEmpty(model.Author) || model.Year > DateTime.Now.Year) throw new LibraryException();
            _repository.CardRepository.Update(_mapper.Map<Card>(model));
            return _repository.SaveAsync();
        }
    }
}
