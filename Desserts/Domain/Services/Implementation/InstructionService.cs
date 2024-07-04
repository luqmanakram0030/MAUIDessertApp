using Desserts.Domain.Models;
using Desserts.Domain.Services.Interface;
using Desserts.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Desserts.Domain.Services.Implementation
{
    public class InstructionService : IInstructionService
    {
        private readonly IDessertService _dessertService;

        public InstructionService()
        {
            _dessertService = DependencyService.Resolve<IDessertService>();
        }
        public void AddInstruction(ObservableCollection<InstructionModel> instructionList, InstructionModel instruction, ObservableCollection<IngredientModel> ingredientList, ObservableCollection<IngredientModel> displayIngredientList)
        {
            if (string.IsNullOrWhiteSpace(instruction?.Description))
            {
                ShowErrorAsync(AppResources.Add_description);
                //ShowErrorAsync("Add description.");
                return;
            }
            string displayIngredientNames = "";
            int count = 1;
            foreach (var existingInstruction in instructionList)
            {
                existingInstruction.Id = count;
                count++;
            }
            instruction.Id = count;

            instruction.IsHeadingVisible = true;
            foreach (var ingredient in displayIngredientList)
            {
                if (string.IsNullOrEmpty(displayIngredientNames))
                {
                    displayIngredientNames = ingredient.DisplayName;
                }
                else
                {
                    displayIngredientNames = displayIngredientNames + ", " + ingredient.DisplayName;
                }
            }

            if (!string.IsNullOrEmpty(instruction.HeadingDescription))
            {
                instruction.IsHeadingDescVisible = true;
            }
            instruction.IsIngVisible = !string.IsNullOrWhiteSpace(displayIngredientNames);

            instruction.IngredientList = ingredientList;
            instruction.IngHeading = displayIngredientNames;
                

            instructionList.Add(instruction);
        }

        public async Task<bool> SaveDessertAsync(DessertModel dessert, ObservableCollection<InstructionModel> instructionList)
        {
            if (instructionList.Count == 0)
            {
                ShowErrorAsync(AppResources.Please_add_at_least_one_instruction);
                //ShowErrorAsync("Please add at least one instruction.");
            }

            dessert.Email = Preferences.Get("Email", "");
            dessert.InstructionsList = instructionList;

            if (string.IsNullOrEmpty(dessert.Key))
            {
                return await _dessertService.AddDessertAsync(dessert);
            }
            else
            {
                return await _dessertService.UpdateDessertAsync(dessert);
            }
        }
        public ObservableCollection<IngredientModel> AddIngredient(ObservableCollection<IngredientModel> ingredientList, IngredientModel selectedIngredient)
        {
            ingredientList.Add(selectedIngredient);

            return ingredientList;
        }
        public ObservableCollection<IngredientModel> RemoveIngredient(ObservableCollection<IngredientModel> ingredientList, IngredientModel selectedIngredient)
        {
            ingredientList.Remove(selectedIngredient);
            return ingredientList;
        }
        
        public bool CheckInternetConnection()
        {
            var current = Connectivity.NetworkAccess;
            return current == NetworkAccess.Internet;
        }

        public ImageSource GetDessertImage(string imageSrc)
        {
            if (!string.IsNullOrEmpty(imageSrc))
            {
                byte[] base64Stream = Convert.FromBase64String(imageSrc);
                return ImageSource.FromStream(() => new MemoryStream(base64Stream));
            }

            return null;
        }

        public ObservableCollection<InstructionModel> GetInstructionsList(DessertModel dessert, int count)
        {
            var instructionsList = new ObservableCollection<InstructionModel>(dessert.InstructionsList);

            foreach (var instruction in instructionsList)
            {
                if (instruction.IngredientList != null)
                {
                    foreach (var ingredient in instruction.IngredientList)
                    {
                        ingredient.Weight = ingredient.DefaultWeight * count;
                    }
                }
            }

            return instructionsList;
        }

        public async Task NavigateBackAsync(ObservableCollection<InstructionModel> instructions, int count)
        {
            foreach (var instruction in instructions)
            {
                if (instruction.IngredientList != null)
                {
                    foreach (var ingredient in instruction.IngredientList)
                    {
                        ingredient.Weight /= count;
                    }
                }
            }

            await Application.Current.MainPage.Navigation.PopModalAsync();
        }
        public void ShowErrorAsync(string message)
        {
            DependencyService.Get<IToastService>().Show(message);
        }

        public void ShowSuccessAsync(string message)
        {
            DependencyService.Get<IToastService>().Show(message);
        }

        public ObservableCollection<InstructionModel> AdjustQuantity(ObservableCollection<InstructionModel> instructions, int count)
        {
            var tempInstruction = new ObservableCollection<InstructionModel>();

            foreach (var instruction in instructions)
            {
                if (instruction.IngredientList != null)
                {
                    foreach (var ingredient in instruction.IngredientList)
                    {
                        ingredient.Weight = ingredient.DefaultWeight * count;
                    }
                }
                tempInstruction.Add(instruction);
            }

            return tempInstruction;
        }
    }
}
