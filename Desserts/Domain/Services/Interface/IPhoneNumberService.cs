using Desserts.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace Desserts.Domain.Services.Interface
{
    public interface IPhoneNumberService
    {
        Task<string> GetPhoneNumberFormat(string countryName);
        Task<ObservableCollection<CountryModel>> GetCountryCodeList();
        Task<CountryModel> GetCountryCode();
        string PhoneNumberMasking(string inputNumber);
        string RemovePhoneNumberMasking(string inputNumber);
        void DisplayErrorAlertAsync(string message);
    }
}
