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
            if (_repository.CategoryRepository.GetAll().FirstOrDefault(x => x.Id == model.Id) != null) throw new CardIndexException("Category already exist!");
            try
            {
                Category _model = _mapper.Map<Category>(model);
                await _repository.CategoryRepository.AddAsync(_model);
                await _repository.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new CardIndexException(ex.Message);
            }
        }

        public async Task DeleteByIdAsync(long modelId)
        {
            if (_repository.CategoryRepository.GetAll().FirstOrDefault(x => x.Id == modelId) == null) throw new CardIndexException("Category not found!");
            await _repository.CategoryRepository.DeleteByIdAsync(modelId);
            await _repository.SaveAsync();
        }

        public IEnumerable<CategoryModel> GetAll()
        {
            var categories = _repository.CategoryRepository.GetAll();
            if (categories == null) throw new CardIndexException("Categories not found!");
            var result = (from b in categories
                          select _mapper.Map<Category, CategoryModel>(b)).ToList();
            return result;
        }

        public ICollection<CategoryModel> GetAllWithDetails()
        {
            var categories = _repository.CategoryRepository.GetAll();
            if (categories == null) throw new CardIndexException("Categories not found!");
            var result = _repository.CategoryRepository.GetAllWithDetails()
                .Select(x => _mapper.Map<Category, CategoryModel>(x)).ToList();
            return result;
        }

        public Task<CategoryModel> GetByIdAsync(long id)
        {
            if (_repository.CategoryRepository.GetAll().FirstOrDefault(x => x.Id == id) == null) throw new CardIndexException("Category not found!");
            var result = _mapper.Map<Category, CategoryModel>(_repository.CategoryRepository.GetByIdAsync(id).Result);
            return Task.FromResult<CategoryModel>(result);
        }

        public ICollection<CardModel> GetCardByCategoryId(long id)
        {
            if (_repository.CategoryRepository.GetAll().FirstOrDefault(x => x.Id == id) == null) throw new CardIndexException("Category not found!");

            var result =_repository.CategoryRepository.GetCardByCategoryId(id)
                .Select(x=> _mapper.Map<Card, CardModel>(x)).ToList();
            return result;
        }

        public Task UpdateAsync(CategoryModel model)
        {
            if (_repository.CategoryRepository.GetAll().FirstOrDefault(x => x.Id == model.Id) == null) throw new CardIndexException("Category not found!");

            _repository.CategoryRepository.Update(_mapper.Map<CategoryModel, Category>(model));
            return _repository.SaveAsync();
        }
        public int Count()
        {
            return _repository.CategoryRepository.GetAll().Count();
        }
    }
}
