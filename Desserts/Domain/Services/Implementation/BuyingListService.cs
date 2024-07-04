using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Desserts.Domain.Helpers.Firebase;
using Desserts.Domain.Models;
using Desserts.Domain.Services.Interface;
using Firebase.Database;
using Firebase.Database.Query;

namespace Desserts.Domain.Services.Implementation
{
    public class BuyingListService : IBuyingListService
    {

        private readonly FirebaseClient _firebaseClient;
        public BuyingListService()
        {
            _firebaseClient = new FirebaseClient(FirebaseWebApi.DatabaseLink, new FirebaseOptions
            {
                AuthTokenAsyncFactory = () => Task.FromResult(FirebaseWebApi.DatabaseSecret)
            });
        }
        public async Task<bool> AddIngredientToShoppingListAsync(IngredientModel model)
        {
            try
            {
                var result = await _firebaseClient.Child(nameof(IngredientModel)).PostAsync(new IngredientModel()
                {
                    Id = model.Id,
                    Heading = model.Heading,
                    Email = Preferences.Get("Email", ""),
                    Weight = model.Weight,
                    UnitOfMeasure = model.UnitOfMeasure,
                    Location = model.Location,
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
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> RemoveIngredientFromShoppingListAsync(string key)
        {
            try
            {
                await _firebaseClient.Child(nameof(IngredientModel)).Child(key).DeleteAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<List<IngredientModel>> GetAllIngredientsFromShoppingListAsync()
        {
            try
            {
                return (await _firebaseClient.Child(nameof(IngredientModel)).OnceAsync<IngredientModel>()).Select(f => new IngredientModel
                {

                    Id = f.Object.Id,
                    Heading = f.Object.Heading,
                    Email = f.Object.Email,
                    Description = f.Object.Description,
                    Weight = f.Object.Weight,
                    UnitOfMeasure = f.Object.UnitOfMeasure,
                    Location = f.Object.Location,
                    Key = f.Key,
                }).ToList();

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("", ex.Message, "OK");
                return null;
            }
        }

        public bool CheckInternetConnection()
        {
            var current = Connectivity.NetworkAccess;
            return current == NetworkAccess.Internet;
        }

        public async Task<ObservableCollection<IngredientModel>> LoadDefaultAsync()
        {
            var email = Preferences.Get("Email", "");
            var list = await GetAllIngredientsFromShoppingListAsync();
            var ingredients = new ObservableCollection<IngredientModel>(list.Where(item => item.Email == email));
            return ingredients;
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

