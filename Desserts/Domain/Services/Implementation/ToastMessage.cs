using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Desserts.Domain.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desserts.Domain.Services.Implementation
{
    public class ToastMessage : IToastService
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        public async void Show(string message)
        {
            string text = message;
            ToastDuration duration = ToastDuration.Short;
            double fontSize = 14;
            var toast = Toast.Make(text, duration, fontSize);
            await toast.Show(cancellationTokenSource.Token);
        }
    }

}
