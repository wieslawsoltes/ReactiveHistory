// Copyright (c) Wiesław Šoltés. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace ReactiveHistorySample.Models
{
    public abstract class BaseObject : ObservableObject
    {
        private object _owner;
        private string _name;

        public object Owner
        {
            get { return _owner; }
            set { Update(ref _owner, value); }
        }

        public string Name
        {
            get { return _name; }
            set { Update(ref _name, value); }
        }
    }
}
