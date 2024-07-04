using Desserts.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace Desserts.Domain.Services.Interface
{
    public interface IDessertDetailService
    {
        void SetupPage(DessertModel dessert, int count, out ObservableCollection<IngredientModel> ingredientList, out ObservableCollection<InstructionModel> instructionList, out ImageSource dessertImage, out string dessertName, out string description, out string time, out string difficulty, out ImageSource favoriteButtonSource);
        Task<bool> AddToCartAsync(IngredientModel item);
        void UpdateIncreaseIngredientWeights(DessertModel dessert, int count);
        void UpdateDecreaseIngredientWeights(DessertModel dessert, int count);
        Task NavigateToInstructions(DessertModel dessert, int count);
        Task NavigateToComments(DessertModel dessert, int count);
        Task NavigateBack();
        Task<bool> ToggleFavorite(DessertModel dessert, ImageSource favoriteButtonSource);
    }
}
