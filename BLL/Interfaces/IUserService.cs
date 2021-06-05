using BLL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IUserService:ICrud<UserModel>
    {
        Task<IEnumerable<UserModel>> GetAllAsync();
        Task<AuthenticationResult> SignupAsync(SignupModel signup);
        Task<AuthenticationResult> LoginAsync(LoginModel signup);
        IEnumerable<UserModel> GetUsersRole(string userRole);
        Task ChangeUserRole(UserModel user);
        Task RemoveUserRole(UserModel user);

    }
}
