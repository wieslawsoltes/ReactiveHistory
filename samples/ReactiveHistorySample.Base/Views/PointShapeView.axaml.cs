using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ReactiveHistorySample.Views;

public class PointShapeView : UserControl
{
    public PointShapeView()
    {
        this.InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
