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
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _repository;
        private readonly IMapper _mapper;
        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _repository = unitOfWork;
        }

        public async Task AddAsync(UserModel model)
        {
            try
            {
                User _model = _mapper.Map<User>(model);
                _repository.UserRepository.AddAsync(_model);
                await _repository.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new CardIndexException(ex.Message);
            }
        }
        public async Task DeleteByIdAsync(int modelId)
        {
            if (_repository.UserRepository.GetAll().Any(x => x.Id == modelId)) throw new CardIndexException();
            await _repository.UserRepository.DeleteByIdAsync(modelId);
        }

        public IEnumerable<UserModel> GetAll()
        {
            var user = _repository.UserRepository.GetAll();
            var result = (from b in user
                          select _mapper.Map<User, UserModel>(b)).ToList();
            return result;
        }

        public Task<UserModel> GetByIdAsync(int id)
        {
            var result = _mapper.Map<User, UserModel>(_repository.UserRepository.GetByIdAsync(id).Result);
            return Task.FromResult<UserModel>(result);
        }

        public Task UpdateAsync(UserModel model)
        {
            _repository.UserRepository.Update(_mapper.Map<User>(model));
            return _repository.SaveAsync();
        }

        public int Count()
        {
            return _repository.UserRepository.GetAll().Count();
        }
    }
}
