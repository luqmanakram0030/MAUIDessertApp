using Desserts.Domain.Models;
using Desserts.Domain.Services.Interface;
using Desserts.Resources;
using Desserts.Views;
using Desserts.Views.CRUD;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace Desserts.Domain.Services.Implementation
{
    public class IngredientService : IIngredientService
    {
        private readonly IDessertService _dessertService;
        private int Count { get; set; }

        public IngredientService()
        {
            _dessertService = DependencyService.Resolve<IDessertService>();
            Count = 1;
        }
        public void AddIngredient(IngredientModel ingredient, ObservableCollection<IngredientModel> ingredientList, int count)
        {
            Count = 1;
            foreach (var item in ingredientList)
            {
                ingredient.Id = Count;
                Count++;
            }
            ingredient.IsHeadingVisible = true;
            ingredient.Id = Count;
            ingredient.DefaultWeight = ingredient.Weight / 4;
            ingredient.Weight = ingredient.Weight;

            if (string.IsNullOrWhiteSpace(ingredient.Heading))
            {
                ingredient.IsHeadingVisible = false;
            }
            Count++;
            ingredientList.Add(ingredient);
        }

        public void DeleteIngredient(IngredientModel ingredient, ObservableCollection<IngredientModel> ingredientList)
        {
            ingredientList.Remove(ingredient);
            Count = 1;
            foreach (var Ingredient in ingredientList)
            {
                Ingredient.Id = Count;
                Count++;
            }
        }

        public async Task<bool> SaveDessertAsync(DessertModel Dessert, ObservableCollection<IngredientModel> IngList, int Id)
        {
            if (!Connectivity.NetworkAccess.Equals(NetworkAccess.Internet))
            {
                DisplayNoInternetAlertAsync();
                return false;
            }

            try
            {
                if (!string.IsNullOrEmpty(Dessert.Name) &&
                    !string.IsNullOrEmpty(Dessert.Difficulty) &&
                    !string.IsNullOrEmpty(Dessert.Description) &&
                    !string.IsNullOrEmpty(Dessert.Time) &&
                    !string.IsNullOrEmpty(Dessert.ImageSrc) &&
                    IngList.Count != 0 &&
                    IngList != null)
                {
                    Dessert.Email = Preferences.Get("Email", "");
                    Dessert.IngredientModels = IngList;

                    bool status;
                    if (Id == 0)
                    {
                        status = await _dessertService.AddDessertAsync(Dessert);
                        if (status)
                        {
                            DisplaySuccessAlertAsync(AppResources.Dessert_saved);
                            //DisplaySuccessAlertAsync("Dessert saved.");
                        }
                        else
                        {
                            ShowErrorAsync(AppResources.Something_went_wrong);
                            //ShowErrorAsync("Something went wrong.");
                        }
                    }
                    else
                    {
                        status = await _dessertService.UpdateDessertAsync(Dessert);
                        if (status)
                        {
                            DisplaySuccessAlertAsync(AppResources.Dessert_updated);
                            //DisplaySuccessAlertAsync("Dessert updated.");
                        }
                        else
                        {
                            ShowErrorAsync(AppResources.Something_went_wrong);
                            //ShowErrorAsync("Something went wrong.");
                        }
                    }

                    return status;
                }
                else
                {
                    ShowErrorAsync(AppResources.Please_add_details);
                    //ShowErrorAsync("Please add details.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                ShowErrorAsync(ex.Message);
                return false;
            }
        }

        public async Task NextStepAsync(DessertModel dessert, ObservableCollection<IngredientModel> ingredientList, VideoModel video, INavigation navigation)
        {
            if (ingredientList.Count == 0)
            {
                ShowErrorAsync(AppResources.Please_add_at_least_one_ingredient );
                //ShowErrorAsync("Please add at least one ingredient");
                return;
            }

            dessert.IngredientModels = ingredientList;
            await navigation.PushAsync(new AddInstructionsPage(dessert , video));
        }
        public void ShowErrorAsync(string message)
        {
            DependencyService.Get<IToastService>().Show(message);
        }

        private void DisplaySuccessAlertAsync(string message)
        {
            DependencyService.Get<IToastService>().Show(message);
        }

        private void DisplayNoInternetAlertAsync()
        {
            DependencyService.Get<IToastService>().Show(AppResources.Connect_your_device_to_the_internet);
            //DependencyService.Get<IToastService>().Show("Connect your device to the internet.");
        }

        public ObservableCollection<IngredientModel> UpdateIngredientWeights(ObservableCollection<IngredientModel> ingredients, int count)
        {
            var updatedIngredients = new ObservableCollection<IngredientModel>();
            foreach (var ingredient in ingredients)
            {
                if (ingredient.IsHeadingVisible)
                {
                    ingredient.Weight = ingredient.DefaultWeight * count;
                    updatedIngredients.Add(ingredient);
                }
            }
            return updatedIngredients;
        }

        public ObservableCollection<IngredientModel> SetupIngredients(IEnumerable<IngredientModel> ingredients, int count)
        {
            var ingredientList = new ObservableCollection<IngredientModel>();
            foreach (var ingredient in ingredients)
            {
                if (ingredient.IsHeadingVisible)
                {
                    ingredient.Weight = ingredient.DefaultWeight * count;
                    ingredientList.Add(ingredient);
                }
            }
            return ingredientList;
        }
    }
}
