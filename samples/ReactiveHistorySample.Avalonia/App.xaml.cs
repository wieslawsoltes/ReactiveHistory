// Copyright (c) Wiesław Šoltés. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Diagnostics;
using Avalonia.Input;
using Avalonia.Logging.Serilog;
using Avalonia.Markup.Xaml;
using Reactive.Bindings.Extensions;
using ReactiveHistory;
using ReactiveHistorySample.Avalonia.Controls;
using ReactiveHistorySample.Models;
using ReactiveHistorySample.ViewModels;
using Serilog;

namespace ReactiveHistorySample.Avalonia
{
    class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
            base.Initialize();
        }

        static void Main(string[] args)
        {
            InitializeLogging();
            var app = new App();
            AppBuilder.Configure(app)
                .UsePlatformDetect()
                .SetupWithoutStarting();
            app.Start();
        }

        public void Start()
        {
            using (var disposable = new CompositeDisposable())
            {
                // Model

                var layer1 = new Layer("layer1");

                var line1 = new LineShape(layer1, "line1");
                line1.Start = new PointShape(100, 100, line1, "start11");
                line1.End = new PointShape(200, 100, line1, "end11");
                layer1.Shapes.Add(line1);

                var line2 = new LineShape(layer1, "line2");
                line2.Start = new PointShape(100, 200, line2, "start21");
                line2.End = new PointShape(200, 200, line2, "end21");
                layer1.Shapes.Add(line2);

                // ViewModel

                var history = new StackHistory();
                var layerViewModel = new LayerViewModel(layer1, history).AddTo(disposable);

                // Window

                var mainWindow = new MainWindow();
                var layerCanvas = mainWindow.FindControl<LayerCanvas>("layerCanvas");

                LineShape line = null;

                layerCanvas.PointerPressed += (sender, args) =>
                {
                    if (args.MouseButton == MouseButton.Left)
                    {
                        var point = args.GetPosition(layerCanvas);
                        if (line == null)
                        {
                            line = new LineShape(layer1, "line");
                            line.Start = new PointShape(point.X, point.Y, line1, "start");
                            line.End = new PointShape(point.X, point.Y, line1, "end");
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

                mainWindow.DataContext = layerViewModel;
                mainWindow.Show();
                Run(mainWindow);
            }
        }

        public static void AttachDevTools(Window window)
        {
#if DEBUG
            DevTools.Attach(window);
#endif
        }

        private static void InitializeLogging()
        {
#if DEBUG
            SerilogLogger.Initialize(new LoggerConfiguration()
                .MinimumLevel.Warning()
                .WriteTo.Trace(outputTemplate: "{Area}: {Message}")
                .CreateLogger());
#endif
        }
    }
}
