using Desserts.Domain.Models;
using FirebaseAdmin.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desserts.Domain.Services.Interface
{
    public interface IChat
    {
        Task SendMessageAsync(MessagesModel message);
        Task<ObservableCollection<MessagesModel>> GetAllMessagesAsync();
        bool CheckInternetConnection();
        void ShowErrorAsync(string message);
        void ShowSuccessAsync(string message);
    }
}
