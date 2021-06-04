using AutoMapper;
using BLL.Interfaces;
using BLL.Models;
using BLL.Validation;
using DAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class RoleService : IRoleService
    {

        private readonly IMapper _mapper;
        private readonly RoleManager<Role> _roleManager;

        public RoleService(IMapper mapper, RoleManager<Role> roleManager)
        {
            _mapper = mapper;
            _roleManager = roleManager;
        }
        public async Task AddAsync(UserRoleModel model)
        {
            if (_roleManager.FindByNameAsync(model.Role).Result != null)
                throw new CardIndexException($"{model.Role} already exist");
            try
            {
                await _roleManager.CreateAsync(_mapper.Map<Role>(model));
            }
            catch (Exception ex)
            {
                throw new CardIndexException(ex.Message);
            }
        }

        public int Count()
        {
           return _roleManager.Roles.Count();
        }

        public async Task DeleteByIdAsync(long modelId)
        {
            var role = _roleManager.Roles.FirstOrDefault(x => x.Id == modelId);
            if (role == null) throw new CardIndexException("Role not found");
            await _roleManager.DeleteAsync(role);
        }

        public IEnumerable<UserRoleModel> GetAll()
        {
            var roles = _roleManager.Roles.ToList();
            if (roles.Count==0) throw new CardIndexException("Roles not found");
            var result = (from r in roles
                          select _mapper.Map<UserRoleModel>(r)).ToList();
            return result;
        }

        public Task<UserRoleModel> GetByIdAsync(long id)
        {
            var result = _mapper.Map<Role, UserRoleModel>(_roleManager.FindByIdAsync(id.ToString()).Result);
            if (result == null) throw new CardIndexException("Role not found");
            return Task.FromResult<UserRoleModel>(result);
        }

        public async Task UpdateAsync(UserRoleModel model)
        {
            if (_roleManager.Roles.FirstOrDefault(x => x.Id == model.Id) == null) throw new CardIndexException("Role not found");
            await _roleManager.UpdateAsync(_mapper.Map<Role>(model));
        }

    }
}
