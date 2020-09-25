using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Restauracja.ViewModel
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }


        protected void RaisePropertiesChanged(params string[] propertyNames)
        {
            if (propertyNames == null)
            {
                RaisePropertyChanged(string.Empty);
                return;
            }
            foreach (string propertyName in propertyNames)
            {
                RaisePropertyChanged(propertyName);
            }
        }

        protected bool SetProperty<T>(ref T backingField, T value, [CallerMemberName]string property = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingField, value))
                return false;

            backingField = value;
            RaisePropertyChanged(property);
            return true;
        }
    }
}
