using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using ReactiveHistorySample.ViewModels;

namespace ReactiveHistorySample.Controls;

public class LayerCanvas : Canvas
{
    public override void Render(DrawingContext context)
    {
        base.Render(context);

        var layer = DataContext as LayerViewModel;
        if (layer != null)
        {
            foreach (var shape in layer.Shapes)
            {
                if (shape is LineShapeViewModel)
                {
                    var line = shape as LineShapeViewModel;
                    context.DrawLine(
                        new Pen(Brushes.Red, 2.0),
                        new Point(line.Start.Value.X.Value, line.Start.Value.Y.Value),
                        new Point(line.End.Value.X.Value, line.End.Value.Y.Value));
                }
            }
        }
    }
}