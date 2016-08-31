// Copyright (c) Wiesław Šoltés. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace ReactiveHistorySample.Models
{
    public class PointShape : BaseShape
    {
        private double _x;
        private double _y;

        public double X
        {
            get { return _x; }
            set { Update(ref _x, value); }
        }

        public double Y
        {
            get { return _y; }
            set { Update(ref _y, value); }
        }

        public PointShape(object owner = null, string name = null)
        {
            this.Owner = owner;
            this.Name = name;
        }

        public PointShape(double x, double y, object owner = null, string name = null)
            : this(owner, name)
        {
            this.X = x;
            this.Y = y;
        }
    }
}
