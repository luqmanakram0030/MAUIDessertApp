using Desserts.Domain.Models;
using Desserts.Domain.Services.Interface;
using Desserts.Resources;
using Desserts.Views;
using Desserts.Views.CRUD;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desserts.Domain.Services.Implementation
{
    public class MyDessertsService : IMyDessertsService
    {
        private const string FavIcon = "fav_Icon.png";
        private const string Heart = "heart.png";

        private readonly IDessertService _dessertService;

        public MyDessertsService()
        {
            _dessertService = DependencyService.Resolve<IDessertService>();
        }
        public async Task GetMyItemsAsync(ObservableCollection<DessertModel> dessertModels, string userEmail)
        {
            var networkAccess = Connectivity.NetworkAccess;
            if (networkAccess == NetworkAccess.Internet)
            {
                try
                {
                    var desserts = await _dessertService.GetAllDessertsAsync();
                    if (desserts != null && desserts.Count > 0)
                    {
                        foreach (var dessert in desserts)
                        {
                            if ("thibaultcoussement2002@gmail.com" == userEmail || dessert.Email == userEmail)
                            {
                                if (!string.IsNullOrEmpty(dessert.ImageSrc))
                                {
                                    var base64Stream = Convert.FromBase64String(dessert.ImageSrc);
                                    dessert.Image = ImageSource.FromStream(() => new MemoryStream(base64Stream));
                                }
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
                                    var bookmark = dessert.BookMarks.FirstOrDefault(bm => bm.Email == userEmail);
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
                                dessertModels.Add(dessert);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ShowErrorAsync(AppResources.Error_loading_desserts);
                    //ShowErrorAsync("Error loading desserts");
                }
            }
            else
            {
                ShowErrorAsync(AppResources.Connect_your_device_to_the_internet);
                //ShowErrorAsync("No internet connection");
            }
        }

        public async Task DeleteDessertAsync(DessertModel dessertModel, ObservableCollection<DessertModel> dessertModels)
        {
            var networkAccess = Connectivity.NetworkAccess;
            if (networkAccess == NetworkAccess.Internet)
            {
                try
                {
                    var deleteStatus = await _dessertService.DeleteDessertAsync(dessertModel.Key);
                    if (deleteStatus)
                    {
                        dessertModels.Remove(dessertModel);
                        ShowSuccessAsync(AppResources.Dessert_deleted);
                        //ShowSuccessAsync("Dessert deleted.");
                    }
                    else
                    {
                        ShowErrorAsync(AppResources.Something_went_wrong);
                        //ShowErrorAsync("Something went wrong.");
                    }
                }
                catch (Exception ex)
                {
                    ShowErrorAsync(AppResources.Error_loading_desserts);
                    //ShowErrorAsync("Error deleting dessert");
                }
            }
            else
            {
                ShowErrorAsync(AppResources.Connect_your_device_to_the_internet);
                //ShowErrorAsync("No internet connection");
            }
        }

        public async Task RequestPermissionAsync(DessertModel dessertModel)
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
                            _ = await Permissions.RequestAsync<Permissions.StorageRead>();
                        }
                    }
                }
                else
                {
                    var storageReadStatus = await Permissions.CheckStatusAsync<Permissions.StorageRead>();
                    if (storageReadStatus == PermissionStatus.Granted)
                    {
                        await RequestCameraPermissionAsync(dessertModel);
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
                            _ = await Permissions.RequestAsync<Permissions.StorageRead>();
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
                        _ = await Permissions.RequestAsync<Permissions.StorageRead>();
                    }
                }
            }
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
                _ = await Permissions.RequestAsync<Permissions.Camera>();
            }
        }

        private void ShowErrorAsync(string message)
        {
            DependencyService.Get<IToastService>().Show(message);
        }

        private void ShowSuccessAsync(string message)
        {
            DependencyService.Get<IToastService>().Show(message);
        }
    }
}
