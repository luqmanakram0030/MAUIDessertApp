using Desserts.Domain.Models;
using Desserts.Domain.Services.Interface;
using Desserts.Services.Domain.Utils;
using PhoneNumbers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desserts.Domain.Services.Implementation
{
    public class PhoneNumberService : IPhoneNumberService
    {
        public ObservableCollection<CountryModel> phoneNumCodes { get; set; } = new ObservableCollection<CountryModel>();
        private readonly IGeolocationService _geolocationService;
        public PhoneNumberService()
        {
            _geolocationService = DependencyService.Get<IGeolocationService>() ?? throw new ArgumentNullException(nameof(IGeolocationService));
        }
        public async Task<ObservableCollection<CountryModel>> GetCountryCodeList()
        { 
           // var phoneNumCodes = new ObservableCollection<CountryModel>();
            var phoneNumberUtil = PhoneNumberUtil.GetInstance();
            var isoCountries = CountryUtils.GetCountriesByIso3166();
            foreach (var country in isoCountries)
            {
                phoneNumCodes.Add(new CountryModel()
                {
                    CountryCode = phoneNumberUtil.GetCountryCodeForRegion(country.TwoLetterISORegionName).ToString(),
                    CountryName = country.EnglishName,
                    //PhoneNoFormat = phoneNumberUtil.Format(phoneNumberUtil.GetExampleNumberForType(country.TwoLetterISORegionName, PhoneNumberType.MOBILE), PhoneNumbers.PhoneNumberFormat.INTERNATIONAL),
                });
            }
            return phoneNumCodes;
        }
        public async Task<CountryModel> GetCountryCode()
        {
            var countryName = await _geolocationService.GetCountryName();
            var countryData = CountryUtils.GetCountryModelByName(countryName);
            countryData.CountryCode = "+" + countryData.CountryCode;
            countryData.PhoneNoFormat = countryData.PhoneNoFormat.Replace(countryData.CountryCode + " ", "");
            countryData.MaskNumber = PhoneNumberMasking(countryData.PhoneNoFormat);
            return countryData;
        }
        public async Task<string> GetPhoneNumberFormat(string countryName)
        {
            var phoneNumberUtil = PhoneNumberUtil.GetInstance();
            var isoCountries = CountryUtils.GetCountriesByIso3166();
            var regionInfo = isoCountries.FirstOrDefault(c => c.EnglishName == countryName);
            return phoneNumberUtil.Format(phoneNumberUtil.GetExampleNumberForType(regionInfo.TwoLetterISORegionName, PhoneNumberType.MOBILE), PhoneNumberFormat.INTERNATIONAL);
        }
        public void DisplayErrorAlertAsync(string message)
        {
            DependencyService.Get<IToastService>().Show(message);
        }
        public string PhoneNumberMasking(string inputNumber)
        {
            string replacement = "A";
            StringBuilder resultBuilder = new StringBuilder();
            foreach (char c in inputNumber)
            {
                if (c == ' ')
                {
                    resultBuilder.Append(c);
                }
                else if (c == '-')
                {
                    resultBuilder.Append(c);
                }
                else
                {
                    resultBuilder.Append(replacement);
                }
            }
            return resultBuilder.ToString();
        }
        public string RemovePhoneNumberMasking(string inputNumber)
        {
            StringBuilder resultBuilder = new StringBuilder();
            foreach (char c in inputNumber)
            {
                if (c == ' ')
                {
                    resultBuilder.Append("");
                }
                else if (c == '-')
                {
                    resultBuilder.Append("");
                }
                else
                {
                    resultBuilder.Append(c);
                }
            }
            return resultBuilder.ToString();
        }
    }
}
