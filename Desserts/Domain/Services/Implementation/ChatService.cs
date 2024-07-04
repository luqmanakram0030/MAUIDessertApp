using Desserts.Domain.Helpers.Firebase;
using Desserts.Domain.Models;
using Desserts.Domain.Services.Interface;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desserts.Domain.Services.Implementation
{
    public class ChatService : IChat
    {
        FirebaseClient firebase = new FirebaseClient(FirebaseWebApi.DatabaseLink, new FirebaseOptions
        {
            AuthTokenAsyncFactory = () => Task.FromResult(FirebaseWebApi.DatabaseSecret)
        });
        public async Task<ObservableCollection<MessagesModel>> GetAllMessagesAsync()
        {
            ObservableCollection<MessagesModel> messageList = new ObservableCollection<MessagesModel>();
            var list = (await firebase.Child(nameof(MessagesModel)).OnceAsync<MessagesModel>()).Select(f => new MessagesModel
            {
                DateSent = f.Object.DateSent,
                ReceiverEmail = f.Object.ReceiverEmail,
                SenderEmail = f.Object.SenderEmail,
                Text = f.Object.Text
            }).ToList();
            foreach (var item in list)
            {
                messageList.Add(item);
            }
            return messageList;
        }
        public async Task SendMessageAsync(MessagesModel message)
        {
            try
            {
                var result = await firebase.Child(nameof(MessagesModel)).PostAsync(new MessagesModel()
                {
                    DateSent = message.DateSent,
                    ReceiverEmail = message.ReceiverEmail,
                    SenderEmail = message.SenderEmail,
                    Text = message.Text
                });
            }
            catch (Exception ex)
            { 
            
            }
        }
        public bool CheckInternetConnection()
        {
            var current = Connectivity.NetworkAccess;
            return current == NetworkAccess.Internet;
        }
        public void ShowErrorAsync(string message)
        {
            DependencyService.Get<IToastService>().Show(message);
        }
        public void ShowSuccessAsync(string message)
        {
            DependencyService.Get<IToastService>().Show(message);
        }
    }
}
