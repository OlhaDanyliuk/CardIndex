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
            if (user==null) throw new CardIndexException("User not found");
            await _userManager.DeleteAsync(user);
        }

        public IEnumerable<UserModel> GetAll()
        {
            var user = _userManager.Users;
            if (user.Count() == 0) throw new CardIndexException("Users  not found");
            var result = (from b in user
                          select _mapper.Map<User, UserModel>(b)).ToList();
            return result;
        }

        public IEnumerable<UserModel> GetUsersRole()
        {
            var userrole = _userManager.GetUsersInRoleAsync("RegisteredUser").Result;
            var result = (from user in userrole
                          select _mapper.Map<User, UserModel>(user)).ToList();
            return result;
        }

        public Task<UserModel> GetByIdAsync(long id)
        {
            var result = _mapper.Map<User, UserModel>(_userManager.FindByIdAsync(id.ToString()).Result);
            if (result == null) throw new CardIndexException("User not found");
            return Task.FromResult<UserModel>(result);
        }

        public Task UpdateAsync(UserModel model)
        {
            if(_userManager.Users.FirstOrDefault(x=>x.Id==model.Id)==null) throw new CardIndexException("Users not found");
            _userManager.UpdateAsync(_mapper.Map<User>(model));
            return _repository.SaveAsync();
        }

        public int Count()
        {
            return _userManager.Users.Count();
        }

        public async Task<AuthenticationResult> Signup(SignupModel signup)
        {
            if (_userManager.Users.Any(x => x.Email == signup.Email))
                return new AuthenticationResult
                {
                    Errors = new[] { "User already exist" }
                };
            try
            {

                string mySalt = BCrypt.Net.BCrypt.GenerateSalt();
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(signup.Password, mySalt);
                User user = new User
                {
                    UserName = signup.UserName,
                    PasswordHash = passwordHash,
                    Email = signup.Email
                };

                var user_result = await _userManager.CreateAsync(user, signup.Password);
                if (user_result.Succeeded)
                {
                    var currentUser = await _userManager.FindByNameAsync(user.UserName);
                    await _userManager.AddToRoleAsync(currentUser, "RegisteredUser");
                }
                var result = GenerateAuthenticationResult(user);
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

        private AuthenticationResult GenerateAuthenticationResult(User user)
        {

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtConfig.Secret);
            var role =_userManager.GetRolesAsync(user);
            IdentityOptions identityOptions = new IdentityOptions();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(identityOptions.ClaimsIdentity.RoleClaimType, role.Result.FirstOrDefault())
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
        public AuthenticationResult Login(LoginModel login)
        {
            var user =_userManager.Users.FirstOrDefault(x => x.Email == login.Email);
            if (user == null)
                return new AuthenticationResult
                {
                    Errors = new[] { "User not exist" }
                };

            bool verified = BCrypt.Net.BCrypt.EnhancedVerify(login.Password, user.PasswordHash);
            if (!verified)
                return new AuthenticationResult
                {
                    Errors = new[] { "Invalid Password" }
                };
            return GenerateAuthenticationResult(user);
        }
    }
}
