using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ReactiveHistorySample.Avalonia.Views
{
    public class LineShapeView : UserControl
    {
        public LineShapeView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
