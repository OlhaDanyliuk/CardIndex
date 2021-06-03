using BLL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IUserService:ICrud<UserModel>
    {
        Task<AuthenticationResult> Signup(SignupModel signup);
        AuthenticationResult Login(LoginModel signup);
<<<<<<< HEAD
        IEnumerable<UserModel> GetUsersRole();

=======
>>>>>>> parent of 7012d18 (fixed registration error with roles)
    }
}
