// Copyright (c) Wiesław Šoltés. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ReactiveHistorySample.ViewModels;

namespace ReactiveHistorySample.Wpf.Controls
{
    public class LayerCanvas : Canvas
    {
        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            var layer = DataContext as LayerViewModel;
            if (layer != null)
            {
                foreach (var shape in layer.Shapes)
                {
                    if (shape is LineShapeViewModel)
                    {
                        var line = shape as LineShapeViewModel;
                        dc.DrawLine(
                            new Pen(Brushes.Red, 2.0),
                            new Point(line.Start.Value.X.Value, line.Start.Value.Y.Value),
                            new Point(line.End.Value.X.Value, line.End.Value.Y.Value));
                    }
                }
            }
        }
    }
}
