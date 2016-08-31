// Copyright (c) Wiesław Šoltés. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace ReactiveHistorySample.Models
{
    public class LineShape : BaseShape
    {
        private PointShape _start;
        private PointShape _end;

        public PointShape Start
        {
            get { return _start; }
            set { Update(ref _start, value); }
        }

        public PointShape End
        {
            get { return _end; }
            set { Update(ref _end, value); }
        }

        public LineShape(object owner = null, string name = null)
        {
            this.Owner = owner;
            this.Name = name;
        }

        public LineShape(PointShape start, PointShape end, object owner = null, string name = null)
            : this(owner, name)
        {
            this.Start = start;
            this.End = end;
        }
    }
}
