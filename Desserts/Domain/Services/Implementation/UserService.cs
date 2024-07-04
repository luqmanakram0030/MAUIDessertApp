using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Database;
using System.Linq;
using Firebase.Database.Query;
using Desserts.Domain.Models;
using Desserts.Domain.Services.Interface;
using Desserts.Domain.Helpers.Firebase;
using System.Runtime.CompilerServices;
using Firebase.Auth;
using Desserts.Views;
using Desserts.Views.Management;

namespace Desserts.Domain.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly FirebaseClient _firebaseClient;
        private readonly FirebaseAuthProvider _firebaseAuthProvider;

        public UserService()
        {
            _firebaseAuthProvider = new FirebaseAuthProvider(new FirebaseConfig(FirebaseWebApi.WebAPIKey));
            _firebaseClient = new FirebaseClient(FirebaseWebApi.DatabaseLink, new FirebaseOptions
            {
                AuthTokenAsyncFactory = () => Task.FromResult(FirebaseWebApi.DatabaseSecret)
            });
        }


        public async Task<bool> RegisterAsync(ApplicationUserModel user)
        {
            var result = await _firebaseClient.Child(nameof(ApplicationUserModel)).PostAsync(new ApplicationUserModel()
            {
                UserId = Guid.NewGuid(),
                Email = user.Email,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                Password = user.Password

            });

            if (result.Object != null)
            {
                return true;
            }
            else
            {
                return false;

            }
        }

        public async Task<ApplicationUserModel> LoginAsync(string email)
        {
            try
            {
                var GetPerson = (await _firebaseClient.Child(nameof(ApplicationUserModel)).OnceAsync<ApplicationUserModel>())
                .Where(a => a.Object.Email == email).FirstOrDefault();

                if (GetPerson != null)
                {

                    var content = GetPerson.Object;
                    return content;

                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return null;
            }
        }

        public bool CheckInternetConnection()
        {
            var current = Connectivity.NetworkAccess;
            return current == NetworkAccess.Internet;
        }

        public async Task<ApplicationUserModel> LoginAsync(string email, string password)
        {
            var user = await LoginAsync(email.Trim().ToLower());
            if (user != null)
            {
                var auth = await _firebaseAuthProvider.SignInWithEmailAndPasswordAsync(email.Trim().ToLower(), password);
                var content = await auth.GetFreshAuthAsync();

                Preferences.Set("Email", email.Trim().ToLower());
                Preferences.Set("Islogined", "true");
                Preferences.Set("Name", user.FullName.Trim());

                ShowSuccessAsync("Hi, Welcome to our dessert recipe app!");
                return user;
            }
            return null;
        }

        public async Task<bool> RegisterUserAsync(ApplicationUserModel user, string confirmPassword)
        {
            if (!CheckInternetConnection())
            {
                ShowErrorAsync("Connect your device to internet.");
                return false;
            }

            if (string.IsNullOrEmpty(user.Email))
            {
                ShowErrorAsync("Please enter your email.");
                return false;
            }

            if (string.IsNullOrEmpty(user.FullName))
            {
                ShowErrorAsync("Please enter your name.");
                return false;
            }

            if (user.Password != confirmPassword)
            {
                ShowErrorAsync("Match passwords please.");
                return false;
            }

            try
            {
                user.Email = user.Email.ToLower().Trim();
                var auth = await _firebaseAuthProvider.CreateUserWithEmailAndPasswordAsync(user.Email.Trim().ToLower(), user.Password);

                await RegisterAsync(user);

                var content = await auth.GetFreshAuthAsync();

                Preferences.Set("Email", user.Email.Trim().ToLower());
                Preferences.Set("Name", user.FullName.Trim());

                ShowSuccessAsync("Registered");
                return true;
            }
            catch (FirebaseAuthException ex)
            {
                var message = $"Firebase authentication error: {ex.InnerException?.Message ?? ex.Message}";
                ShowErrorAsync(message);
                return false;
            }
            catch (Exception)
            {
                ShowErrorAsync("Account already exists");
                return false;
            }
        }

        public async Task NavigateToRegisterPageAsync()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new RegisterPage());
        }

        public async Task NavigateToForgotPasswordPageAsync()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new ForgotPasswordPage());
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

