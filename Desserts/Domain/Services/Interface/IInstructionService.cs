using Desserts.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace Desserts.Domain.Services.Interface
{
    public interface IInstructionService
    {
        void AddInstruction(ObservableCollection<InstructionModel> instructionList, InstructionModel instruction, ObservableCollection<IngredientModel> ingredientList, ObservableCollection<IngredientModel> displayIngredientList);
        Task<bool> SaveDessertAsync(DessertModel dessert, ObservableCollection<InstructionModel> instructionList);
        ObservableCollection<IngredientModel> AddIngredient(ObservableCollection<IngredientModel> ingredientList, IngredientModel selectedIngredient);
        ObservableCollection<IngredientModel> RemoveIngredient(ObservableCollection<IngredientModel> ingredientList, IngredientModel selectedIngredient);
        bool CheckInternetConnection();
        ImageSource GetDessertImage(string imageSrc);
        ObservableCollection<InstructionModel> GetInstructionsList(DessertModel dessert, int count);
        ObservableCollection<InstructionModel> AdjustQuantity(ObservableCollection<InstructionModel> instructions, int count);
        Task NavigateBackAsync(ObservableCollection<InstructionModel> instructions, int count);
        void ShowErrorAsync(string message);
        void ShowSuccessAsync(string message);
    }
}
