using Desserts.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace Desserts.Domain.Services.Interface
{
    public interface IMyDessertsService
    {
        Task GetMyItemsAsync(ObservableCollection<DessertModel> dessertModels, string userEmail);
        Task DeleteDessertAsync(DessertModel dessertModel, ObservableCollection<DessertModel> dessertModels);
        Task RequestPermissionAsync(DessertModel dessertModel);
        Task RequestCameraPermissionAsync(DessertModel dessertModel);
    }
}
