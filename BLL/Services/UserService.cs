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
        public UserService(IUnitOfWork unitOfWork, IMapper mapper, JwtConfig jwt)
        {
            _mapper = mapper;
            _repository = unitOfWork;
            jwtConfig = jwt;
        }

        public async Task AddAsync(UserModel model)
        {
            try
            {
                User _model = _mapper.Map<User>(model);
                await _repository.UserManager.CreateAsync(_model);
                await _repository.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new CardIndexException(ex.Message);
            }
        }
        public async Task DeleteByIdAsync(long modelId)
        {
            var user = _repository.UserManager.Users.FirstOrDefault(x => x.Id == modelId);
            if (user==null) throw new CardIndexException("User not found");
            await _repository.UserManager.DeleteAsync(user);
        }

        public IEnumerable<UserModel> GetAll()
        {
            var user = _repository.UserManager.Users;
            if (user.Count() == 0) throw new CardIndexException("Users  not found");
            var result = (from b in user
                          select _mapper.Map<User, UserModel>(b)).ToList();
            return result;
        }

<<<<<<< HEAD
        public IEnumerable<UserModel> GetUsersRole()
        {
            var userrole = _repository.UserManager.GetUsersInRoleAsync("RegisteredUser").Result;
            var result = (from user in userrole
                          select _mapper.Map<User, UserModel>(user)).ToList();
            return result;
        }

=======
>>>>>>> parent of 7012d18 (fixed registration error with roles)
        public Task<UserModel> GetByIdAsync(long id)
        {
            var result = _mapper.Map<User, UserModel>(_repository.UserManager.FindByIdAsync(id.ToString()).Result);
            if (result == null) throw new CardIndexException("User not found");
            return Task.FromResult<UserModel>(result);
        }

        public Task UpdateAsync(UserModel model)
        {
            if(_repository.UserManager.Users.FirstOrDefault(x=>x.Id==model.Id)==null) throw new CardIndexException("Users not found");
            _repository.UserManager.UpdateAsync(_mapper.Map<User>(model));
            return _repository.SaveAsync();
        }

        public int Count()
        {
            return _repository.UserManager.Users.Count();
        }

        public async Task<AuthenticationResult> Signup(SignupModel signup)
        {
            if (_repository.UserManager.Users.Any(x => x.Email == signup.Email))
                return new AuthenticationResult
                {
                    Errors = new[] { "User already exist" }
                };
            try
            {
                int salt = 12;
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(signup.Password, salt);
                User user = new User
                {
                    UserName = signup.UserName,
                    PasswordHash = passwordHash,
                    Email = signup.Email
                };

                var user_result = await _repository.UserManager.CreateAsync(user, signup.Password);
                if (user_result.Succeeded)
                {
<<<<<<< HEAD
                    var currentUser = await _repository.UserManager.FindByNameAsync(user.UserName);
                    await _repository.UserManager.AddToRoleAsync(currentUser, "RegisteredUser");
=======
                    await userManager.AddToRoleAsync(user, "RegisteredUser");
>>>>>>> parent of 7012d18 (fixed registration error with roles)
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
            var role = _repository.UserManager.GetRolesAsync(user);
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
            var user = _repository.UserManager.Users.First(x => x.Email == login.Email);
            if (user == null)
                return new AuthenticationResult
                {
                    Errors = new[] { "User not exist" }
                };
            bool verified = BCrypt.Net.BCrypt.Verify(login.Password, user.PasswordHash);
            if (!verified)
                return new AuthenticationResult
                {
                    Errors = new[] { "Invalid Password" }
                };
            return GenerateAuthenticationResult(user);
        }
    }
}
