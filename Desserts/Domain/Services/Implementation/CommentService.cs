using Desserts.Domain.Models;
using Desserts.Domain.Services.Interface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace Desserts.Domain.Services.Implementation
{
    public class CommentService : ICommentService
    {
        private readonly IDessertService _dessertService;

        public CommentService()
        {
            _dessertService = DependencyService.Resolve<IDessertService>();
        }

        public async Task<bool> PostCommentAsync(DessertModel dessert, string commentText, int count)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(commentText))
                {
                    ShowErrorAsync("Please enter a comment.");
                    return false;
                }

                var current = Connectivity.NetworkAccess;
                if (current != NetworkAccess.Internet)
                {
                    DisplayNoInternetAlertAsync();
                    return false;
                }

                dessert.Comments.Add(new CommentsModel
                {
                    Email = Preferences.Get("Name", ""),
                    Comment = commentText,
                });



                var status = await _dessertService.UpdateDessertAsync(dessert);
                if (status)
                {
                    foreach (var ingredient in dessert.IngredientModels)
                    {
                        ingredient.Weight = ingredient.DefaultWeight * count;
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                ShowErrorAsync(ex.Message);
            }
            return false;
        }

        public ObservableCollection<CommentsModel> GetCommentsAsync(DessertModel dessert)
        {
            return new ObservableCollection<CommentsModel>(dessert.Comments ?? new List<CommentsModel>());
        }
        private void DisplayNoInternetAlertAsync()
        {
            DependencyService.Get<IToastService>().Show("Connect your device to the internet.");
        }
        public void ShowErrorAsync(string message)
        {
            DependencyService.Get<IToastService>().Show(message);
        }
    }
}
