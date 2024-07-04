using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Map = Microsoft.Maui.Controls.Maps.Map;

namespace Desserts.Domain.Services.Interface
{
    public interface IGeolocationService
    {
        Task<string> GetCountryName();
        Task<Location> GetCurrentPositionAsync();
        void ShowErrorAsync(string message);
        void MoveMapToPosition(Map map, Location position, double kilometers);
    }
}
