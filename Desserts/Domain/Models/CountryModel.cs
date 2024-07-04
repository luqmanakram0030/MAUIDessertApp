using Desserts.ViewModels.Base;
using PhoneNumbers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Desserts.Domain.Models
{
    public class CountryModel : INotifyPropertyChanged
    {
        #region Fields

        private string _flagUrl;
        private string _countryName;
        private string _countryCode;
        private string _phoneNoFormat;
        private string _maskNumber;

        #endregion Fields

        #region Properties

        public string FlagUrl
        {
            get => _flagUrl;
            set => SetProperty(ref _flagUrl, value);
        }

        public string CountryName
        {
            get => _countryName;
            set => SetProperty(ref _countryName, value);
        }

        public string CountryCode
        {
            get => _countryCode;
            set => SetProperty(ref _countryCode, value);
        }

        public string PhoneNoFormat
        {
            get => _phoneNoFormat;
            set => SetProperty(ref _phoneNoFormat, value);
        }
        public string MaskNumber
        {
            get => _maskNumber;
            set => SetProperty(ref _maskNumber, value);
        }

        #endregion Properties

        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "", Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

    }
}
