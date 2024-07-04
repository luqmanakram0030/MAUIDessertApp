using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Desserts.Domain.Models;

namespace Desserts.Domain.Services.Interface
{
    public interface IDessertService
    {
        Task<bool> AddDessertAsync(DessertModel model);
        Task<bool> UpdateDessertAsync(DessertModel model);
        Task<bool> DeleteDessertAsync(string key);
        Task<List<DessertModel>> GetAllDessertsAsync();
        Task<byte[]> ProcessMediaAsync(Stream stream);
        Task<string> CapturePhotoAsync();
        Task<string> PickPhotoAsync();
        Task<VideoModel> PickVideoAsync();
        Task<VideoModel> GetVideoAsync(string videoName);
        Task<bool> UploadVideo(VideoModel video);
        Task<bool> ToggleFavorite(DessertModel dessert);
        void ShowErrorAsync(string message);
        void ShowSuccessAsync(string message);
    }
}

