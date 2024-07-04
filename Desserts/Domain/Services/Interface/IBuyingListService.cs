using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Desserts.Domain.Models;

namespace Desserts.Domain.Services.Interface
{
    public interface IBuyingListService
    {
        Task<bool> AddIngredientToShoppingListAsync(IngredientModel model);
        Task<bool> RemoveIngredientFromShoppingListAsync(string key);
        Task<List<IngredientModel>> GetAllIngredientsFromShoppingListAsync();
        bool CheckInternetConnection();
        Task<ObservableCollection<IngredientModel>> LoadDefaultAsync();
        void ShowErrorAsync(string message);
        void ShowSuccessAsync(string message);
    }
}

