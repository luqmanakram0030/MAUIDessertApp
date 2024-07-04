using System;
using Firebase.Database;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Firebase.Database.Query;
using Desserts.Domain.Models;
using Desserts.Domain.Services.Interface;
using Desserts.Domain.Helpers.Firebase;
using System.IO;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Desserts.Domain.Services.Implementation
{
    public class DessertService : IDessertService
    {

        private readonly FirebaseClient _firebaseClient;
        public DessertService()
        {
            _firebaseClient = new FirebaseClient(FirebaseWebApi.DatabaseLink, new FirebaseOptions
            {
                AuthTokenAsyncFactory = () => Task.FromResult(FirebaseWebApi.DatabaseSecret)
            }); 
            var jsonFileName = "Desserts.Resources.Assets.desserts_key.json";
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(DessertService)).Assembly;
            Stream stream = assembly.GetManifestResourceStream(jsonFileName);
            string jsonString = "";
            using (var reader = new System.IO.StreamReader(stream))
            {
                jsonString = reader.ReadToEnd();
            }
            var credential = GoogleCredential.FromJson(jsonString)
            .CreateScoped(DriveService.ScopeConstants.Drive);
            service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential
            });
        }

        public async Task<bool> AddDessertAsync(DessertModel model)
        {
            try
            {
                var result = await _firebaseClient.Child(nameof(DessertModel)).PostAsync(new DessertModel()
                {
                    Id = model.Id,
                    Email = model.Email,
                    MakerName = Preferences.Get("Name", ""),
                    Name = model.Name,
                    Description = model.Description,
                    IngredientModels = model.IngredientModels,
                    InstructionsList = model.InstructionsList,
                    Time = model.Time,
                    Video_Name = model.Video_Name,
                    Difficulty = model.Difficulty,
                    DessertImages = model.DessertImages,
                    ImageSrc = model.ImageSrc,
                    BookMarks = model.BookMarks,
                    Comments = model.Comments

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

        public async Task<bool> UpdateDessertAsync(DessertModel model)
        {
            try
            {
                await _firebaseClient.Child(nameof(DessertModel)).Child(model.Key).PutAsync(new DessertModel()
                {
                    Id = model.Id,
                    Email = model.Email,
                    MakerName = Preferences.Get("Name", ""),
                    Name = model.Name,
                    Description = model.Description,
                    IngredientModels = model.IngredientModels,
                    InstructionsList = model.InstructionsList,
                    Time = model.Time,
                    Video_Name = model.Video_Name,
                    Difficulty = model.Difficulty,
                    DessertImages = model.DessertImages,
                    ImageSrc = model.ImageSrc,
                    BookMarks = model.BookMarks,
                    Comments = model.Comments
                });
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> DeleteDessertAsync(string Key)
        {
            try
            {
                await _firebaseClient.Child(nameof(DessertModel)).Child(Key).DeleteAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<List<DessertModel>> GetAllDessertsAsync()
        {
            try
            {
                return (await _firebaseClient.Child(nameof(DessertModel)).OnceAsync<DessertModel>()).Select(f => new DessertModel
                {
                    Id = f.Object.Id,
                    Email = f.Object.Email,
                    MakerName = f.Object.MakerName,
                    Name = f.Object.Name,
                    Description = f.Object.Description,
                    IngredientModels = f.Object.IngredientModels,
                    InstructionsList = f.Object.InstructionsList,
                    Time = f.Object.Time,
                    Video_Name = f.Object.Video_Name,
                    Difficulty = f.Object.Difficulty,
                    ImageSrc = f.Object.ImageSrc,
                    DessertImages = f.Object.DessertImages,
                    BookMarks = f.Object.BookMarks,
                    Comments = f.Object.Comments,
                    Key = f.Key
                }).ToList();

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("", ex.Message, "OK");
                return null;
            }
        }
        public async Task<byte[]> ProcessMediaAsync(Stream stream)
        {
            var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }

        public async Task<string> CapturePhotoAsync()
        {
            if (!MediaPicker.IsCaptureSupported)
            {
                ShowErrorAsync("Camera is not available");
                return null;
            }

            var photo = await MediaPicker.CapturePhotoAsync(new MediaPickerOptions
            {
                Title = "Take a photo"
            });

            if (photo != null)
            {
                var stream = await photo.OpenReadAsync();
                var bytes = await ProcessMediaAsync(stream);
                return Convert.ToBase64String(bytes);
            }

            return null;
        }

        public async Task<string> PickPhotoAsync()
        {
            var photo = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Select a photo"
            });

            if (photo != null)
            {
                var stream = await photo.OpenReadAsync();
                var bytes = await ProcessMediaAsync(stream);
                return Convert.ToBase64String(bytes);
            }

            return null;
        }

        public async Task<VideoModel> PickVideoAsync()
        {
            var videoModel = new VideoModel();
            var video = await MediaPicker.PickVideoAsync(new MediaPickerOptions
            {
                Title = "Select a Video"
            });

            if (video != null)
            {
                videoModel.stream = await video.OpenReadAsync();
                videoModel.FileName = video.FileName;
                var bytes = await ProcessMediaAsync(videoModel.stream);
                videoModel.videoData = Convert.ToBase64String(bytes);
                return videoModel;
            }

            return null;
        }

        public async Task<bool> ToggleFavorite(DessertModel dessert)
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                try
                {
                    var result = dessert.BookMarks.FirstOrDefault(i => i.Email == Preferences.Get("Email", ""));
                    if (result == null)
                    {
                        dessert.BookMarks.Add(new BookMarkModel() { Email = Preferences.Get("Email", "") });
                        var status = await UpdateDessertAsync(dessert);
                        if (status)
                        {
                            ShowSuccessAsync("Dessert bookmarked!");
                            return true;
                        }
                    }
                    else
                    {
                        dessert.BookMarks.Remove(result);
                        var status = await UpdateDessertAsync(dessert);
                        if (status)
                        {
                            ShowSuccessAsync("Removed Dessert from bookmarked!");
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    ShowErrorAsync(ex.Message);
                    return false;
                }
            }
            else
            {
                ShowErrorAsync("Connect your device to the internet");
                return false;
            }

            return false;
        }

        public void ShowSuccessAsync(string message)
        {
            DependencyService.Get<IToastService>().Show(message);
        }
        public void ShowErrorAsync(string message)
        {
            DependencyService.Get<IToastService>().Show(message);
        }

        #region Upload_And_Getting_Video_Methods
        DriveService service = new DriveService();
        FileList data = new FileList();

        public async Task<VideoModel> GetVideoAsync(string videoName)
        {
            VideoModel video = new VideoModel();
            try
            {
                string fileName = videoName;
                string folderID = "1C5_wTtCepnCpfvcYRTTuA3F7CjgmAE--";
                string link = "https://drive.google.com/file/d/";
                var request = service.Files.List();
                data = request.ExecuteAsync().Result;
                foreach (var file in data.Files)
                {
                    string url = link + file.Id + "/preview";
                    if (videoName == file.Name)
                    {
                        video.videoUrl = url;
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("", ex.Message, "Ok");
            }
            return video;
        }

        /*
        public async Task<bool> DownloadFile(string fileId)
        {
            var response = false;
            try
            {
                string saveTo = "";
#if WINDOWS
                saveTo = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
#elif ANDROID
                saveTo = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
                saveTo = saveTo + "/Download";
#endif
                string fileName = "";
                var request = service.Files.List();
                var list = request.ExecuteAsync().Result;
                FilesResource.GetRequest getrequest = service.Files.Get(fileId);
                fileName = getrequest.Execute().Name;
                var filePath = Path.Combine(saveTo, fileName);
                FileStream fileStream = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write);
                getrequest.Download(fileStream);
                fileStream.Close();
                response = true;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("", ex.Message, "Ok");
                response = false;
            }
            return response;
        }
        */
        public async Task<bool> UploadVideo(VideoModel videoModel)
        {
            var response = false;
            try
            {
                string folderId = "1C5_wTtCepnCpfvcYRTTuA3F7CjgmAE--";
                var fileMetadata = new Google.Apis.Drive.v3.Data.File()
                {
                    Name = Path.GetFileName(videoModel.FileName),
                    Parents = new List<string>() { folderId }
                };
                FilesResource.CreateMediaUpload request;
                request = service.Files.Create(fileMetadata,videoModel.stream, "");
                request.Fields = "id";
                request.Upload();
                var responceBody = request.ResponseBody;
                response = true;
            }
            catch (Exception ex)
            {
                response = false;
            }

            return response;
        }
        /*
        public async Task<bool> DeleteFile(string fileId)
        {
            var response = false;
            try
            {
                service.Files.Delete(fileId).Execute();
                response = true;
            }
            catch (Exception ex)
            {
                //throw new Exception("Request Files.Delete failed.", ex);
                response = false;
            }
            return response;
        }
        */
        #endregion
    }
}

