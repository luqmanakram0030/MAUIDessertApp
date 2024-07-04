using Desserts.Domain.Models;
using Desserts.Domain.Services.Interface;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Location = Microsoft.Maui.Devices.Sensors.Location;
using Map = Microsoft.Maui.Controls.Maps.Map;

namespace Desserts.Domain.Services.Implementation
{
    public class GeolocationService : IGeolocationService
    {
        public async Task<string> GetCountryName()
        {
            try
            {
                /*
                //IEnumerable<Placemark> placemarks = await Geocoding.Default.GetPlacemarksAsync(latitude, longitude);
                //Placemark placemark = placemarks?.FirstOrDefault();
                //if (placemark != null)
                //{
                //    return
                //        $"AdminArea:       {placemark.AdminArea}\n" +
                //        $"CountryCode:     {placemark.CountryCode}\n" +
                //        $"CountryName:     {placemark.CountryName}\n" +
                //        $"FeatureName:     {placemark.FeatureName}\n" +
                //        $"Locality:        {placemark.Locality}\n" +
                //        $"PostalCode:      {placemark.PostalCode}\n" +
                //        $"SubAdminArea:    {placemark.SubAdminArea}\n" +
                //        $"SubLocality:     {placemark.SubLocality}\n" +
                //        $"SubThoroughfare: {placemark.SubThoroughfare}\n" +
                //        $"Thoroughfare:    {placemark.Thoroughfare}\n";
                //}
                */
                string countryName = "";
                var position = await GetCurrentPositionAsync();
                if (position != null)
                {
                    var placemarks = await Geocoding.Default.GetPlacemarksAsync(position.Latitude, position.Longitude);
                    if (placemarks != null && placemarks.Any())
                    {
                        var placemark = placemarks.FirstOrDefault();
                        countryName = placemark.CountryName;
                    }
                }

                return countryName;
            }
            catch (Exception ex)
            {
                ShowErrorAsync($"Error getting location: {ex.Message}");
                return null;
            }
        }

        public async Task<Location> GetCurrentPositionAsync()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Best);
                var location = await Geolocation.GetLocationAsync(request);

                if (location != null)
                {
                    return new Location(location.Latitude, location.Longitude);
                }
                else
                {
                    ShowErrorAsync("Location not found");
                    return default(Location);
                }
            }
            catch (Exception ex)
            {
                ShowErrorAsync($"Error getting location: {ex.Message}");
                return default(Location);
            }
        }
        public void MoveMapToPosition(Map map, Location position, double kilometers)
        {
            map.MoveToRegion(MapSpan.FromCenterAndRadius(position, Distance.FromKilometers(kilometers)));
        }

        public void ShowErrorAsync(string message)
        {
            DependencyService.Get<IToastService>().Show(message);
        }
    }
}
