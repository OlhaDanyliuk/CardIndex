using AutoMapper;
using BLL.Configuration;
using BLL.Interfaces;
using BLL.Models;
using BLL.Validation;
using DAL.Entities;
using DAL.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class UserService : IUserService
    {
        private readonly JwtConfig jwtConfig;

        private readonly IUnitOfWork _repository;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        public UserService(IUnitOfWork unitOfWork, IMapper mapper, JwtConfig jwt, UserManager<User> userManager)
        {
            _mapper = mapper;
            _repository = unitOfWork;
            jwtConfig = jwt;
            _userManager = userManager;
        }


        public async Task AddAsync(UserModel model)
        {
            try
            {
                User _model = _mapper.Map<User>(model);
                await _userManager.CreateAsync(_model);
                await _repository.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new CardIndexException(ex.Message);
            }
        }
        public async Task DeleteByIdAsync(long modelId)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Id == modelId);
            if (user == null) throw new CardIndexException("User not found");
            await _userManager.DeleteAsync(user);
        }

        public async Task<IEnumerable<UserModel>> GetAllAsync()
        {
            var user = _userManager.Users.ToList();
            if (!user.Any()) throw new CardIndexException("Users  not found");
            var result = new List<UserModel>();
            foreach (var u in user)
            {
                UserModel userModel = _mapper.Map<User, UserModel>(u);
                var roles =await _userManager.GetRolesAsync(u);
                if (roles.Count > 0)
                    userModel.Role = roles.FirstOrDefault();
                result.Add(userModel);
            }
            return result;
        }



        public async Task<UserModel> GetByIdAsync(long id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) throw new CardIndexException("User not found");
            var result = _mapper.Map<User, UserModel>(user);
            var role = await _userManager.GetRolesAsync(user);
            result.Role = role.First();
            return result;
        }

        public Task UpdateAsync(UserModel model)
        {
            if (_userManager.Users.FirstOrDefault(x => x.Id == model.Id) == null) throw new CardIndexException("Users not found");
            _userManager.UpdateAsync(_mapper.Map<User>(model));
            return _repository.SaveAsync();
        }

        public int Count()
        {
            return _userManager.Users.Count();
        }

        public async Task<AuthenticationResult> SignupAsync(SignupModel signup)
        {
            if (_userManager.Users.Any(x => x.Email == signup.Email))
                return new AuthenticationResult
                {
                    Errors = new[] { "User already exist" }
                };
            try
            {
                User user = new User
                {
                    UserName = signup.UserName,
                    PasswordHash = signup.Password,
                    Email = signup.Email
                };

                var user_result = await _userManager.CreateAsync(user, signup.Password);
                if (user_result.Succeeded)
                {
                    var currentUser = await _userManager.FindByNameAsync(user.UserName);
                    await _userManager.AddToRoleAsync(currentUser, "RegisteredUser");
                }
                var result =await GenerateAuthenticationResultAsync(user);
                return result;
            }
            catch (Exception ex)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { ex.Message }
                };
            }
        }

        private async Task<AuthenticationResult> GenerateAuthenticationResultAsync(User user)
        {

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtConfig.Secret);
            var role = await _userManager.GetRolesAsync(user);
            IdentityOptions identityOptions = new IdentityOptions();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(identityOptions.ClaimsIdentity.RoleClaimType, role.FirstOrDefault())
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)

            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new AuthenticationResult
            {
                Success = true,
                Token = tokenHandler.WriteToken(token)
            };
        }
        public async Task<AuthenticationResult> LoginAsync(LoginModel login)
        {
            var user = _userManager.Users.SingleOrDefault(x => x.Email == login.Email);
            if (user == null)
                return new AuthenticationResult
                {
                    Errors = new[] { "User not exist" }
                };

            var verified = _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, login.Password);
            if (verified != PasswordVerificationResult.Success)
                return new AuthenticationResult
                {
                    Errors = new[] { "Invalid Password" }
                };
            var result=await GenerateAuthenticationResultAsync(user);
            return result;
        }

        public IEnumerable<UserModel> GetUsersRole(string userRole)
        {
            var userrole = _userManager.GetUsersInRoleAsync(userRole).Result;
            var result = (from user in userrole
                          select _mapper.Map<User, UserModel>(user)).ToList();
            foreach(var u in result)
            {
                var role=  _userManager.GetRolesAsync(_mapper.Map<User>(u)).Result;
                u.Role = role.First();
            }
            return result;
        }

        public async Task ChangeUserRole(UserModel userModel)
        {
            var user = _mapper.Map<User>(userModel);
            if (_userManager.IsInRoleAsync(user, userModel.Role).Result)
                throw new CardIndexException($"User already is in role {userModel.Role}");

            var _user = _userManager.FindByIdAsync(user.Id.ToString()).Result;
            var previousRole = _userManager.GetRolesAsync(user).Result.FirstOrDefault();
            if(previousRole!=null)
                await _userManager.RemoveFromRoleAsync(_user, previousRole);
            await _userManager.AddToRoleAsync(_user, userModel.Role);
        }

        public async Task RemoveUserRole(UserModel userModel)
        {
            var user = await _userManager.FindByIdAsync(userModel.Id.ToString());
            if (!_userManager.IsInRoleAsync(user, userModel.Role).Result)
                throw new CardIndexException($"User already is in role {userModel.Role}");

            await _userManager.RemoveFromRoleAsync(user, userModel.Role);
        }

        public IEnumerable<UserModel> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
