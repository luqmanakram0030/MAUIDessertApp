using Desserts.Domain.Models;
using System.Threading.Tasks;

namespace Desserts.Domain.Services.Interface
{
    public interface INotificationService
    {
        Task ShowAsync(DessertModel model);
    }
}

