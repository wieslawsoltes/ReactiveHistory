// Copyright (c) Wiesław Šoltés. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ReactiveHistorySample.Models
{
    public abstract class ObservableObject : INotifyPropertyChanged
    {
#pragma warning disable CS8618
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore CS8618

        public void Notify([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool Update<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (!Equals(field, value))
            {
                field = value;
                Notify(propertyName);
                return true;
            }
            return false;
        }
    }
}
