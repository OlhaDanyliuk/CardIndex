using AutoMapper;
using BLL.Interfaces;
using BLL.Models;
using BLL.Validation;
using DAL.Entities;
using DAL.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace BLL.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly IUnitOfWork _repository;
        private readonly IMapper _mapper;

        public CategoriesService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _repository = unitOfWork;
        }
        public async Task AddAsync(CategoryModel model)
        {
            try
            {
                Category _model = _mapper.Map<Category>(model);
                await _repository.CategoryRepository.AddAsync(_model);
            }
            catch (Exception ex)
            {
                throw new CardIndexException(ex.Message);
            }
        }

        public async Task DeleteByIdAsync(int modelId)
        {
            await _repository.CategoryRepository.DeleteByIdAsync(modelId);
        }

        public IEnumerable<CategoryModel> GetAll()
        {
            var categories = _repository.CategoryRepository.GetAll();
            var result = (from b in categories
                          select _mapper.Map<Category, CategoryModel>(b)).ToList();
            return result;
        }

        public ICollection<CategoryModel> GetAllWithDetails()
        {
            var categories = _repository.CategoryRepository.GetAllWithDetails()
                .Select(x => _mapper.Map<Category, CategoryModel>(x)).ToList();
            return categories;
        }

        public Task<CategoryModel> GetByIdAsync(int id)
        {
            var result = _mapper.Map<Category, CategoryModel>(_repository.CategoryRepository.GetByIdAsync(id).Result);
            return Task.FromResult<CategoryModel>(result);
        }

        public ICollection<CardModel> GetCardByCategoryId(long id)
        {
            var result =_repository.CategoryRepository.GetCardByCategoryId(id)
                .Select(x=> _mapper.Map<Card, CardModel>(x)).ToList();
            return result;
        }

        public Task UpdateAsync(CategoryModel model)
        {
            _repository.CategoryRepository.Update(_mapper.Map<CategoryModel, Category>(model));
            return _repository.SaveAsync();
        }
        public int Count()
        {
            return _repository.CategoryRepository.GetAll().Count();
        }
    }
}
