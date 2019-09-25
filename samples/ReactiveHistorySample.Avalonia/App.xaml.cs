// Copyright (c) Wiesław Šoltés. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Diagnostics;
using Avalonia.Input;
using Avalonia.Logging.Serilog;
using Avalonia.Markup.Xaml;
using Reactive.Bindings.Extensions;
using ReactiveHistory;
using ReactiveHistorySample.Avalonia.Controls;
using ReactiveHistorySample.Models;
using ReactiveHistorySample.ViewModels;

namespace ReactiveHistorySample.Avalonia
{
    class App : Application
    {
        static void Main(string[] args)
        {
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }

        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                         .UsePlatformDetect()
                         .LogToDebug();

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
            {
                var disposable = new CompositeDisposable();

                // Model

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
                line2.Owner = line2;
                layer1.Shapes.Add(line2);

                // ViewModel

                var history = new StackHistory().AddTo(disposable);
                var layerViewModel = new LayerViewModel(layer1, history).AddTo(disposable);

                // Window

                var mainWindow = new MainWindow()
                {
                    DataContext = layerViewModel
                };

                var layerCanvas = mainWindow.FindControl<LayerCanvas>("layerCanvas");

                LineShape? line = null;

                layerCanvas.PointerPressed += (sender, e) =>
                {
                    if (e.GetPointerPoint(layerCanvas).Properties.IsLeftButtonPressed)
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
                    else if (e.GetPointerPoint(layerCanvas).Properties.IsRightButtonPressed)
                    {
                        if (line != null)
                        {
                            layer1.Shapes.Remove(line);
                            line = null;
                            layerCanvas.InvalidateVisual();
                        }
                    }
                };

                layerCanvas.PointerMoved += (sender, args) =>
                {
                    var point = args.GetPosition(layerCanvas);
                    if (line != null)
                    {
                        line.End.X = point.X;
                        line.End.Y = point.Y;
                        layerCanvas.InvalidateVisual();
                    }
                };

                history.CanClear.Subscribe(_ => layerCanvas.InvalidateVisual()).AddTo(disposable);

                desktopLifetime.MainWindow = mainWindow;

                desktopLifetime.Exit += (sennder, e) =>
                {
                    disposable.Dispose();
                };
            }
            else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewLifetime)
            {
                //singleViewLifetime.MainView = new MainView();
            }
            base.OnFrameworkInitializationCompleted();
        }
    }
}
