using Desserts.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace Desserts.Domain.Services.Interface
{
    public interface ICommentService
    {
        Task<bool> PostCommentAsync(DessertModel dessert, string commentText, int count);
        ObservableCollection<CommentsModel> GetCommentsAsync(DessertModel dessert);
    }
}
