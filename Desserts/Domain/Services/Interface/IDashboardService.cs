using Desserts.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace Desserts.Domain.Services.Interface
{
    public interface IDashboardService
    {
        Task<ObservableCollection<DessertModel>> GetItemsAsync();
        Task<ObservableCollection<DessertModel>> GetMyItemsAsync();
        Task<DessertModel> FavoriteDessertAsync(DessertModel dessertModel);
        Task ToggleCommentsAsync(DessertModel dessertModel);
        Task<bool> DeleteDessertAsync(DessertModel dessertModel);
        Task SaveDataAsync(ObservableCollection<DessertModel> dessertModels);
        Task<bool> RequestPermissionAsync(DessertModel dessertModel);
        Task RequestCameraPermissionAsync(DessertModel dessertModel);
        Task SelectDessertAsync(DessertModel dessertModel);
        void ShowErrorAsync(string message);
        void ShowSuccessAsync(string message);
    }
}

