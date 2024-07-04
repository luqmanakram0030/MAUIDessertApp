using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Desserts.Domain.Models;
using Desserts.Domain.Services.Interface;
using Desserts.ViewModels.Base;
using Desserts.Views;
using Desserts.Views.Other;

namespace Desserts.Domain.Services.Implementation
{
    public class DessertDetailService : BaseViewModel, IDessertDetailService
    {
        private readonly IDessertService _dessertService;
        private readonly IBuyingListService _shoppingListService;

        public DessertDetailService()
        {
            _dessertService = DependencyService.Resolve<IDessertService>();
            _shoppingListService = DependencyService.Resolve<IBuyingListService>();
        }

        public void SetupPage(DessertModel dessert, int count, out ObservableCollection<IngredientModel> ingredientList, out ObservableCollection<InstructionModel> instructionList, out ImageSource dessertImage, out string dessertName, out string description, out string time, out string difficulty, out ImageSource favoriteButtonSource)
        {
            instructionList = new ObservableCollection<InstructionModel>();
            ingredientList = new ObservableCollection<IngredientModel>();

            SetupBookmarks(dessert, out favoriteButtonSource);
            SetupImage(dessert, out dessertImage);
            SetupTextFields(dessert, out dessertName, out description, out time, out difficulty);
            SetupIngredients(dessert, count, ingredientList);
        }

        private void SetupBookmarks(DessertModel dessert, out ImageSource favoriteButtonSource)
        {
            favoriteButtonSource = dessert.BookMarks == null || !dessert.BookMarks.Any()
                ? ImageSource.FromFile("Assets/fav_Icon.png")
                : ImageSource.FromFile("Assets/heart.png");
        }

        private void SetupImage(DessertModel dessert, out ImageSource dessertImage)
        {
            if (!string.IsNullOrEmpty(dessert.ImageSrc))
            {
                byte[] base64Stream = Convert.FromBase64String(dessert.ImageSrc);
                dessertImage = ImageSource.FromStream(() => new MemoryStream(base64Stream));
            }
            else
            {
                dessertImage = null;
            }
        }

        private void SetupTextFields(DessertModel dessert, out string dessertName, out string description, out string time, out string difficulty)
        {
            dessertName = dessert.Name;
            description = dessert.Description;
            time = dessert.Time;
            difficulty = dessert.Difficulty;
        }

        private void SetupIngredients(DessertModel dessert, int count, ObservableCollection<IngredientModel> ingredientList)
        {
            ingredientList.Clear();
            foreach (var ingredientItem in dessert.IngredientModels)
            {
                if (ingredientItem.IsHeadingVisible)
                {
                    ingredientItem.Weight = ingredientItem.DefaultWeight * count;
                    ingredientList.Add(ingredientItem);
                }
            }
            OnPropertyChanged(nameof(ingredientList));
        }

        public async Task<bool> AddToCartAsync(IngredientModel item)
        {
            try
            {
                return await _shoppingListService.AddIngredientToShoppingListAsync(item);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("", ex.Message, "Ok");
                return false;
            }
        }

        public void UpdateIncreaseIngredientWeights(DessertModel dessert, int count)
        {
            foreach (var item in dessert.IngredientModels)
            {
                item.Weight = (item.Weight / (count - 1)) * count;
            }
        }

        public void UpdateDecreaseIngredientWeights(DessertModel dessert, int count)
        {
            foreach (var item in dessert.IngredientModels)
            {
                item.Weight = (item.Weight / (count + 1)) * count;
            }
        }

        public async Task NavigateToInstructions(DessertModel dessert, int count)
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new InstructionsPage(dessert, count));
        }

        public async Task NavigateToComments(DessertModel dessert, int count)
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new CommentPage(dessert, count));
        }

        public async Task NavigateBack()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        public async Task<bool> ToggleFavorite(DessertModel dessert, ImageSource favoriteButtonSource)
        {
            var email = Preferences.Get("Email", "");
            var result = dessert.BookMarks.FirstOrDefault(i => i.Email == email);
            if (result == null)
            {
                dessert.BookMarks.Add(new BookMarkModel { Email = email });
                var status = await _dessertService.UpdateDessertAsync(dessert);
                if (status)
                {
                    favoriteButtonSource = ImageSource.FromFile("Assets/heart.png");
                    return true;
                }
            }
            else
            {
                dessert.BookMarks.Remove(result);
                var status = await _dessertService.UpdateDessertAsync(dessert);
                if (status)
                {
                    favoriteButtonSource = ImageSource.FromFile("Assets/fav_Icon.png");
                    return true;
                }
            }
            return false;
        }
    }
}
