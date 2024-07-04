using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Desserts.Domain.Models;
using Desserts.Domain.Services.Interface;
using Desserts.Resources;
using Desserts.Views;
using Desserts.Views.CRUD;
using Desserts.Views.Other;

namespace Desserts.Domain.Services.Implementation
{
    public class DashboardService : IDashboardService
    {
        private const string FavIcon = "fav_Icon.png";
        private const string Heart = "heart.png";
        private readonly IDessertService _dessertService;

        public DashboardService()
        {
            _dessertService = DependencyService.Resolve<IDessertService>();
        }

        public async Task<ObservableCollection<DessertModel>> GetItemsAsync()
        {
            var networkAccess = Connectivity.NetworkAccess;
            var dessertModels = new ObservableCollection<DessertModel>();
            if (networkAccess == NetworkAccess.Internet)
            {
                try
                {
                    var desserts = await _dessertService.GetAllDessertsAsync();
                    if (desserts != null)
                    {
                        foreach (var dessert in desserts)
                        {
                            PopulateDessertFields(dessert);
                            dessertModels.Add(dessert);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ShowErrorAsync(ex.Message);
                }
            }
            else
            {
                ShowErrorAsync(AppResources.Connect_your_device_to_the_internet);
                //ShowErrorAsync("Connect your device to the internet.");
            }
            return dessertModels;
        }

        public async Task<ObservableCollection<DessertModel>> GetMyItemsAsync()
        {
            var networkAccess = Connectivity.NetworkAccess;
            var dessertModels = new ObservableCollection<DessertModel>();
            if (networkAccess == NetworkAccess.Internet)
            {
                try
                {
                    var email = Preferences.Get("Email", "");
                    var desserts = await _dessertService.GetAllDessertsAsync();
                    if (desserts != null)
                    {
                        foreach (var dessert in desserts)
                        {
                            if ("thibaultcoussement2002@gmail.com" == email || dessert.Email == email)
                            {
                                PopulateDessertFields(dessert);
                                dessertModels.Add(dessert);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ShowErrorAsync(ex.Message);
                }
            }
            else
            {
                ShowErrorAsync(AppResources.Connect_your_device_to_the_internet);
                //ShowErrorAsync("Connect your device to the internet.");
            }
            return dessertModels;
        }

        public async Task<DessertModel> FavoriteDessertAsync(DessertModel dessertModel)
        {
          
            var networkAccess = Connectivity.NetworkAccess;
            if (networkAccess == NetworkAccess.Internet)
            {
                try
                {
                    var bookmark = dessertModel.BookMarks.FirstOrDefault(bm => bm.Email == Preferences.Get("Email", ""));
                    if (bookmark == null)
                    {
                        dessertModel.BookMarks.Add(new BookMarkModel { Email = Preferences.Get("Email", "") });

                        var updateStatus = await _dessertService.UpdateDessertAsync(dessertModel);
                        if (updateStatus)
                        {
                            dessertModel.FavImage = Heart;
                            ShowSuccessAsync(AppResources.Dessert_bookmarked);
                           
                            return dessertModel;
                        }
                    }
                    else
                    {
                        dessertModel.BookMarks.Remove(bookmark);

                        var updateStatus = await _dessertService.UpdateDessertAsync(dessertModel);
                        if (updateStatus)
                        {
                            dessertModel.FavImage = FavIcon;
                            ShowSuccessAsync(AppResources.Dessert_bookmark_removed);
                            return dessertModel;
                        }
                    }
                }
                catch (Exception ex)
                {
                    ShowErrorAsync(ex.Message);
                    return null;
                }
            }
            else
            {
                ShowErrorAsync(AppResources.Connect_your_device_to_the_internet);
                //ShowErrorAsync("Connect your device to the internet.");
            }
            return dessertModel;
        }

        public Task ToggleCommentsAsync(DessertModel dessertModel)
        {
            dessertModel.IsExpanded = !dessertModel.IsExpanded;
            return Task.CompletedTask;
        }

        public async Task<bool> DeleteDessertAsync(DessertModel dessertModel)
        {
            var networkAccess = Connectivity.NetworkAccess;
            if (networkAccess == NetworkAccess.Internet)
            {
                try
                {
                    var deleteStatus = await _dessertService.DeleteDessertAsync(dessertModel.Key);
                    if (deleteStatus)
                    {
                        ShowSuccessAsync(AppResources.Dessert_deleted);
                        return true;
                    }
                    else
                    {
                        ShowErrorAsync(AppResources.Something_went_wrong);
                    }
                }
                catch (Exception ex)
                {
                    ShowErrorAsync(ex.Message);
                }
            }
            else
            {
                ShowErrorAsync(AppResources.Connect_your_device_to_the_internet);
                //ShowErrorAsync("Connect your device to the internet.");
            }
            return false;
        }

        public async Task SaveDataAsync(ObservableCollection<DessertModel> dessertModels)
        {
            foreach (var dessert in dessertModels)
            {
                await _dessertService.AddDessertAsync(dessert);
            }
        }

        public async Task<bool> RequestPermissionAsync(DessertModel dessertModel)
        {
            var platform = DeviceInfo.Platform;
            if (platform != DevicePlatform.UWP)
            {
                if (OperatingSystem.IsAndroidVersionAtLeast(33) || OperatingSystem.IsAndroidVersionAtLeast(34))
                {
                    var mediaStatus = await Permissions.CheckStatusAsync<Permissions.Media>();
                    if (mediaStatus == PermissionStatus.Granted)
                    {
                        await RequestCameraPermissionAsync(dessertModel);
                        return true;
                    }
                    else
                    {
                        mediaStatus = await Permissions.RequestAsync<Permissions.Media>();
                        if (Permissions.ShouldShowRationale<Permissions.Media>())
                        {
                            ShowErrorAsync(AppResources.App_needs_media_permission);
                            //ShowErrorAsync("App needs media permission");
                        }
                        if (mediaStatus != PermissionStatus.Granted)
                        {
                            await Permissions.RequestAsync<Permissions.StorageRead>();
                        }
                    }
                }
                else
                {
                    var storageReadStatus = await Permissions.CheckStatusAsync<Permissions.StorageRead>();
                    if (storageReadStatus == PermissionStatus.Granted)
                    {
                        await RequestCameraPermissionAsync(dessertModel);
                        return true;
                    }
                    else
                    {
                        storageReadStatus = await Permissions.RequestAsync<Permissions.StorageRead>();
                        if (Permissions.ShouldShowRationale<Permissions.StorageRead>())
                        {
                            ShowErrorAsync(AppResources.App_needs_storage_permission);
                            //ShowErrorAsync("App needs storage permission");
                        }
                        if (storageReadStatus != PermissionStatus.Granted)
                        {
                            await Permissions.RequestAsync<Permissions.StorageRead>();
                        }
                    }
                }
            }
            else
            {
                var storageReadStatus = await Permissions.CheckStatusAsync<Permissions.StorageRead>();
                if (storageReadStatus == PermissionStatus.Granted)
                {
                    await RequestCameraPermissionAsync(dessertModel);
                    return true;
                }
                else
                {
                    storageReadStatus = await Permissions.RequestAsync<Permissions.StorageRead>();
                    if (Permissions.ShouldShowRationale<Permissions.StorageRead>())
                    {
                        ShowErrorAsync(AppResources.App_needs_storage_permission);
                        //ShowErrorAsync("App needs storage permission");
                    }
                    if (storageReadStatus != PermissionStatus.Granted)
                    {
                        await Permissions.RequestAsync<Permissions.StorageRead>();
                    }
                }
            }
            return false;
        }

        public async Task RequestCameraPermissionAsync(DessertModel dessertModel)
        {
            var cameraStatus = await Permissions.CheckStatusAsync<Permissions.Camera>();
            if (cameraStatus == PermissionStatus.Granted)
            {
                await Application.Current.MainPage.Navigation.PushAsync(new AddDessertPage(dessertModel));
            }
            else
            {
                cameraStatus = await Permissions.RequestAsync<Permissions.Camera>();
            }

            if (Permissions.ShouldShowRationale<Permissions.Camera>())
            {
                ShowErrorAsync(AppResources.App_needs_camera_permission);
                //ShowErrorAsync("App needs camera permission");
            }

            if (cameraStatus != PermissionStatus.Granted)
            {
                await Permissions.RequestAsync<Permissions.Camera>();
            }
        }

        public async Task SelectDessertAsync(DessertModel dessertModel)
        {
            try
            {
                if (dessertModel.BookMarks == null || dessertModel.BookMarks.Count() == 0)
                {
                    dessertModel.BookMarks = new List<BookMarkModel>();
                }
                if (dessertModel.Comments == null || dessertModel.Comments.Count() == 0)
                {
                    dessertModel.Comments = new List<CommentsModel>();
                }
                await Application.Current.MainPage.Navigation.PushAsync(new DessertDetailPage(dessertModel));
            }
            catch (Exception ex)
            {
                ShowErrorAsync(ex.Message);
            }
        }

        private void PopulateDessertFields(DessertModel dessert)
        {
            if (!string.IsNullOrEmpty(dessert.ImageSrc))
            {
                var base64Stream = Convert.FromBase64String(dessert.ImageSrc);
                dessert.Image = ImageSource.FromStream(() => new MemoryStream(base64Stream));
            }
            if (dessert.Comments == null || dessert.Comments.Count() == 0)
            {
                dessert.Comments = new List<CommentsModel>();
            }
            if (dessert.BookMarks == null || dessert.BookMarks.Count() == 0)
            {
                dessert.BookMarks = new List<BookMarkModel>();
                dessert.FavImage = FavIcon;
            }
            else
            {
                var email = Preferences.Get("Email", "");
                var bookmark = dessert.BookMarks.FirstOrDefault(bm => bm.Email == email);
                if (bookmark == null)
                {
                    dessert.FavImage = FavIcon;
                }
                else
                {
                    dessert.FavImage = Heart;
                }
            }

            if (dessert.Difficulty == "Easy")
                dessert.Difficulty = AppResources.Easy;
            else if (dessert.Difficulty == "Intermediate")
                dessert.Difficulty = AppResources.Intermediate;
            else if (dessert.Difficulty == "Hard")
                dessert.Difficulty = AppResources.Hard;
            else if (dessert.Difficulty == "Very Hard")
                dessert.Difficulty = AppResources.Veryhard;

            switch (Device.RuntimePlatform)
            {
                case Device.UWP:
                    dessert.IsWindows = true;
                    break;
                default:
                    dessert.IsWindows = false;
                    break;
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
