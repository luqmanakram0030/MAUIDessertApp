using Desserts.Domain.Models;
using Desserts.Domain.Services.Interface;
using Desserts.Resources;
using Desserts.Views;
using Desserts.Views.Other;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desserts.Domain.Services.Implementation
{
    public class FavoriteService : IFavoriteService
    {
        private const string FavIcon = "fav_Icon.png";
        private const string Heart = "heart.png";

        private readonly IDessertService _dessertService;

        public FavoriteService()
        {
            _dessertService = DependencyService.Resolve<IDessertService>();
        }
        public async Task LoadDessertsAsync(ObservableCollection<DessertModel> dessertModels, string userEmail)
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                try
                {
                    var desserts = await _dessertService.GetAllDessertsAsync();
                    if (desserts != null && desserts.Count > 0)
                    {
                        foreach (var dessert in desserts)
                        {
                            if (dessert.Comments == null || dessert.Comments.Count == 0)
                            {
                                dessert.Comments = new List<CommentsModel>();
                            }
                            if (dessert.BookMarks == null || dessert.BookMarks.Count == 0)
                            {
                                dessert.BookMarks = new List<BookMarkModel>();
                                dessert.FavImage = FavIcon;
                            }
                            else
                            {
                                var bookmark = dessert.BookMarks.FirstOrDefault(a => a.Email == userEmail);
                                if (bookmark == null)
                                {
                                    dessert.FavImage = FavIcon;
                                }
                                else
                                {
                                    dessert.FavImage = Heart;
                                    if (!string.IsNullOrEmpty(dessert.ImageSrc))
                                    {
                                        byte[] Base64Stream = Convert.FromBase64String(dessert.ImageSrc);
                                        dessert.Image = ImageSource.FromStream(() => new MemoryStream(Base64Stream));
                                    }
                                    if (dessert.Difficulty == "Easy")
                                        dessert.Difficulty = AppResources.Easy;
                                    else if (dessert.Difficulty == "Intermediate")
                                        dessert.Difficulty = AppResources.Intermediate;
                                    else if (dessert.Difficulty == "Hard")
                                        dessert.Difficulty = AppResources.Hard;
                                    else if (dessert.Difficulty == "Very Hard")
                                        dessert.Difficulty = AppResources.Veryhard;

                                    dessertModels.Add(dessert);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    DisplayErrorAlertAsync(AppResources.Error_loading_desserts);
                    //DisplayErrorAlertAsync("Error loading desserts");
                }
            }
            else
            {
                DisplayErrorAlertAsync(AppResources.Connect_your_device_to_the_internet);
                //DisplayErrorAlertAsync("No internet connection");
            }
        }

        public async Task NavigateToDessertDetailAsync(DessertModel dessertModel)
        {
            try
            {
                if (dessertModel.BookMarks == null || dessertModel.BookMarks.Count == 0)
                {
                    dessertModel.BookMarks = new List<BookMarkModel>();
                }
                if (dessertModel.Comments == null || dessertModel.Comments.Count == 0)
                {
                    dessertModel.Comments = new List<CommentsModel>();
                }
                await Application.Current.MainPage.Navigation.PushAsync(new DessertDetailPage(dessertModel));
            }
            catch (Exception ex)
            {
                DisplayErrorAlertAsync(AppResources.Error_navigating_to_dessert_detail);
                //DisplayErrorAlertAsync("Error navigating to dessert detail");
            }
        }

        public async Task RemoveFromFavoritesAsync(DessertModel dessertModel, ObservableCollection<DessertModel> dessertModels)
        {
            try
            {
                var result = dessertModel.BookMarks.FirstOrDefault(i => i.Email == Preferences.Get("Email", ""));
                dessertModel.BookMarks.Remove(result);
                var status = await _dessertService.UpdateDessertAsync(dessertModel);
                if (status)
                {
                    dessertModel.FavImage = FavIcon;
                    dessertModels.Remove(dessertModel);
                }
            }
            catch (Exception ex)
            {
                DisplayErrorAlertAsync(AppResources.Error_removing_from_favorites);
                //DisplayErrorAlertAsync("Error removing from favorites");
            }
        }

        private void DisplayErrorAlertAsync(string message)
        {
            DependencyService.Get<IToastService>().Show(message);
        }
    }
}
