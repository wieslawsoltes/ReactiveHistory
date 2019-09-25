// Copyright (c) Wiesław Šoltés. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System.Collections.ObjectModel;

namespace ReactiveHistorySample.Models
{
    public class Layer : BaseObject
    {
        private ObservableCollection<LineShape> _shapes;

        public ObservableCollection<LineShape> Shapes
        {
            get { return _shapes; }
            set { Update(ref _shapes, value); }
        }

        public Layer(object owner, string name) : base(owner, name)
        {
            _shapes = new ObservableCollection<LineShape>();
        }
    }
}
