using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
namespace WpfTestApp.ViewModel
{
    public abstract class ViewModelBase:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void IPropertyChanged(string PropertyName)
        {
            PropertyChangedEventHandler Handler = this.PropertyChanged;
            if (Handler != null)
                Handler(this, new PropertyChangedEventArgs(PropertyName));
        }

        protected bool SetPropertry<T>
            (ref T Storage,T Value,[CallerMemberName] string Propertyname=null)
        {
            if (EqualityComparer<T>.Default.Equals(Storage, Value)) return false;
            Storage = Value;
            IPropertyChanged(Propertyname);
            return true;
        }

    }
}
