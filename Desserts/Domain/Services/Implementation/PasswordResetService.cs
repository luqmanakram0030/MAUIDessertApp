using Desserts.Domain.Helpers.Firebase;
using Desserts.Domain.Services.Interface;
using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Desserts.Domain.Services.Implementation
{
    public class PasswordResetService : IPasswordResetService
    {
        public PasswordResetService()
        {
        }
        public bool CheckInternetConnection()
        {
            var current = Connectivity.NetworkAccess;
            return current == NetworkAccess.Internet;
        }

        public async Task<bool> SendPasswordResetEmailAsync(string email)
        {
            try
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(FirebaseWebApi.WebAPIKey));
                await authProvider.SendPasswordResetEmailAsync(email.Trim().ToLower());
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void ShowErrorAsync(string message)
        {
            DependencyService.Get<IToastService>().Show(message);
        }

        public void ShowSuccessAsync(string message)
        {
            DependencyService.Get<IToastService>().Show(message);
        }
    }
}
