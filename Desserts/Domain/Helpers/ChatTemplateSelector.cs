using Desserts.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Desserts.Domain.Helpers
{
    public class ChatTemplateSelector : DataTemplateSelector, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private DataTemplate _IncomingMessageTemplate;
        public DataTemplate IncomingMessageTemplate
        {
            get { return _IncomingMessageTemplate; }
            set { _IncomingMessageTemplate = value; OnPropertyChanged(); }
        }
        private DataTemplate _OutgoingMessageTemplate;
        public DataTemplate OutgoingMessageTemplate
        {
            get { return _OutgoingMessageTemplate; }
            set { _OutgoingMessageTemplate = value; OnPropertyChanged(); }
        }
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is IncomingMessage)
                return IncomingMessageTemplate;
            else
                return OutgoingMessageTemplate;
        }
    }
}
