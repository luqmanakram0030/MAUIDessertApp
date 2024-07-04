using Desserts.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace Desserts.Domain.Services.Interface
{
    public interface IIngredientService
    {
        void AddIngredient(IngredientModel ingredient, ObservableCollection<IngredientModel> ingredientList, int count);
        void DeleteIngredient(IngredientModel ingredient, ObservableCollection<IngredientModel> ingredientList);
        ObservableCollection<IngredientModel> UpdateIngredientWeights(ObservableCollection<IngredientModel> ingredients, int count);
        ObservableCollection<IngredientModel> SetupIngredients(IEnumerable<IngredientModel> ingredients, int count);
        Task NextStepAsync(DessertModel dessert, ObservableCollection<IngredientModel> ingredientList, VideoModel video, INavigation navigation);
        Task<bool> SaveDessertAsync(DessertModel Dessert, ObservableCollection<IngredientModel> IngList, int Id);
        void ShowErrorAsync(string message);
    }
}
