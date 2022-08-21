using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;
using System.Reactive.Disposables;
using Reactive.Bindings.Extensions;
using ReactiveHistory;
using ReactiveHistorySample.Controls;
using ReactiveHistorySample.Models;
using ReactiveHistorySample.ViewModels;

namespace ReactiveHistorySample.Views;

public class MainView : UserControl
{
    private readonly CompositeDisposable _disposable = new CompositeDisposable();

    public MainView()
    {
        InitializeComponent();
        Setup();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        Cleanup();

        base.OnDetachedFromVisualTree(e);
    }

    private void Setup()
    {
        // Model

        var layer1 = CreateLayer();

        // ViewModel

        var history = new StackHistory().AddTo(_disposable);
        var layerViewModel = new LayerViewModel(layer1, history).AddTo(_disposable);

        // Window

        DataContext = layerViewModel;

        HandleEvents(layer1, history);
    }

    private void Cleanup()
    {
        _disposable.Dispose();
    }

    private Layer CreateLayer()
    {
        object owner = new object();

        var layer1 = new Layer(owner, "layer1");

        var start11 = new PointShape(owner, "start11", 100, 100);
        var end11 = new PointShape(owner, "end11", 200, 100);
        var line1 = new LineShape(layer1, "line1", start11, end11);
        start11.Owner = line1;
        end11.Owner = line1;
        layer1.Shapes.Add(line1);

        var start21 = new PointShape(owner, "start21", 100, 200);
        var end21 = new PointShape(owner, "end21", 200, 200);
        var line2 = new LineShape(layer1, "line2", start21, end21);
        start21.Owner = line2;
        end21.Owner = line2;
        layer1.Shapes.Add(line2);

        return layer1;
    }

    private void HandleEvents(Layer layer1, StackHistory history)
    {
        var layerCanvas = this.FindControl<LayerCanvas>("layerCanvas");
        if (layerCanvas is null)
        {
            return;
        }

        LineShape? line = null;

        layerCanvas.PointerPressed += (_, e) =>
        {
            if (e.GetCurrentPoint(layerCanvas).Properties.IsLeftButtonPressed)
            {
                var point = e.GetPosition(layerCanvas);
                if (line == null)
                {
                    var start = new PointShape(layer1.Owner, "start", point.X, point.Y);
                    var end = new PointShape(layer1.Owner, "end", point.X, point.Y);
                    line = new LineShape(layer1, "line", start, end);
                    line.Start.Owner = line;
                    line.End.Owner = line;
                    //layer1.Shapes.AddWithHistory(line, history);
                    layer1.Shapes.Add(line);
                    //history.IsPaused = true;
                    layerCanvas.InvalidateVisual();
                }
                else
                {
                    line.End.X = point.X;
                    line.End.Y = point.Y;
                    layer1.Shapes.Remove(line);
                    layer1.Shapes.AddWithHistory(line, history);
                    //history.IsPaused = false;
                    line = null;
                    layerCanvas.InvalidateVisual();
                }
            }
            else if (e.GetCurrentPoint(layerCanvas).Properties.IsRightButtonPressed)
            {
                if (line != null)
                {
                    layer1.Shapes.Remove(line);
                    line = null;
                    layerCanvas.InvalidateVisual();
                }
            }
        };

        layerCanvas.PointerMoved += (_, args) =>
        {
            var point = args.GetPosition(layerCanvas);
            if (line != null)
            {
                line.End.X = point.X;
                line.End.Y = point.Y;
                layerCanvas.InvalidateVisual();
            }
        };

        history.CanClear.Subscribe(_ => layerCanvas.InvalidateVisual()).AddTo(_disposable);
    }
}

