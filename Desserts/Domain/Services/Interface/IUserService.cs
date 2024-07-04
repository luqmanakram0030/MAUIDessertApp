using System.Collections.Generic;
using System.Threading.Tasks;
using Desserts.Domain.Models;

namespace Desserts.Domain.Services.Interface
{
    public interface IUserService
    {
        Task<bool> RegisterAsync(ApplicationUserModel user);
        Task<ApplicationUserModel> LoginAsync(string email);
        bool CheckInternetConnection();
        Task<ApplicationUserModel> LoginAsync(string email, string password);
        Task<bool> RegisterUserAsync(ApplicationUserModel user, string confirmPassword);
        Task NavigateToRegisterPageAsync();
        Task NavigateToForgotPasswordPageAsync();
        void ShowErrorAsync(string message);
        void ShowSuccessAsync(string message);
    }
}

