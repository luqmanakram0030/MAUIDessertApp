using Desserts.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace Desserts.Domain.Services.Interface
{
    public interface IFavoriteService
    {
        Task LoadDessertsAsync(ObservableCollection<DessertModel> dessertModels, string userEmail);
        Task RemoveFromFavoritesAsync(DessertModel dessertModel, ObservableCollection<DessertModel> dessertModels);
        Task NavigateToDessertDetailAsync(DessertModel dessertModel);
    }
}
