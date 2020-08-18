using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ReactiveHistorySample.Avalonia
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
