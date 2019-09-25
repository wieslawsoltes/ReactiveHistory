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

        public LineShape(object owner, string name, PointShape start, PointShape end) : base(owner, name)
        {
            _start = start;
            _end = end;
        }
    }
}
