using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Desserts.Domain.Services.Interface
{
    public interface IPasswordResetService
    {
        Task<bool> SendPasswordResetEmailAsync(string email);
        bool CheckInternetConnection();
        void ShowErrorAsync(string message);
        void ShowSuccessAsync(string message);
    }
}
