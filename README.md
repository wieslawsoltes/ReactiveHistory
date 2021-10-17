# ReactiveHistory

[![Gitter](https://badges.gitter.im/wieslawsoltes/ReactiveHistory.svg)](https://gitter.im/wieslawsoltes/ReactiveHistory?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge)

[![Build Status](https://dev.azure.com/wieslawsoltes/GitHub/_apis/build/status/wieslawsoltes.ReactiveHistory?branchName=master)](https://dev.azure.com/wieslawsoltes/GitHub/_build/latest?definitionId=102&branchName=master)
[![CI](https://github.com/wieslawsoltes/ReactiveHistory/actions/workflows/build.yml/badge.svg)](https://github.com/wieslawsoltes/ReactiveHistory/actions/workflows/build.yml)

[![NuGet](https://img.shields.io/nuget/v/ReactiveHistory.svg)](https://www.nuget.org/packages/ReactiveHistory)
[![NuGet](https://img.shields.io/nuget/dt/ReactiveHistory.svg)](https://www.nuget.org/packages/ReactiveHistory)
[![MyGet](https://img.shields.io/myget/reactivehistory-nightly/vpre/ReactiveHistory.svg?label=myget)](https://www.myget.org/gallery/reactivehistory-nightly) 

**ReactiveHistory** is an undo/redo framework for .NET. 

## About

The main design principle for `ReactiveHistory` is to provide easy to use  undo functionality inside your `view models` and allow separation from `data models` when following the [MVVM](https://en.wikipedia.org/wiki/Model-view-viewmodel) pattern for creation of modern and portable `GUI` applications. 

The `ReactiveHistory` framework enables easy implementation of undo history for observable properties and collections. To enable quick conversion from standard .NET properties and collection helper extension methods are provided. For example the `ObserveWithHistory` extension method is intended to be used for `IObservable<T>` properties and the `DeleteWithHistory` for `IList<T>` collections. 

## Example Usage

For examples of `ReactiveHistory` usage please check the `samples` folder. Currently there are available samples for [WPF](https://en.wikipedia.org/wiki/Windows_Presentation_Foundation) and [Avalonia](https://github.com/AvaloniaUI/Avalonia) frameworks. The samples follow `MVVM` pattern, the `view models` use [ReactiveProperty](https://github.com/runceel/ReactiveProperty) framework to implement observable properties and commands. There are also available unit tests located in `tests` folder with all tests for each of the use cases.

### MVVM

[Model](https://github.com/wieslawsoltes/ReactiveHistory/tree/master/samples/ReactiveHistorySample.Models)

```C#
public class Layer : BaseObject
{
    private ObservableCollection<LineShape> _shapes;

    public ObservableCollection<LineShape> Shapes
    {
        get { return _shapes; }
        set { Update(ref _shapes, value); }
    }

    public Layer(object owner = null, string name = null)
    {
        this.Owner = owner;
        this.Name = name;
        this.Shapes = new ObservableCollection<LineShape>();
    }
}
```

[ViewModel](https://github.com/wieslawsoltes/ReactiveHistory/tree/master/samples/ReactiveHistorySample.ViewModels)

```C#
public class LayerViewModel : IDisposable
{
    private CompositeDisposable Disposable { get; set; }
    public ReactiveProperty<string> Name { get; set; }
    public ReadOnlyReactiveCollection<LineShapeViewModel> Shapes { get; set; }
    public ReactiveCommand UndoCommand { get; set; }
    public ReactiveCommand RedoCommand { get; set; }
    public ReactiveCommand ClearCommand { get; set; }

    public LayerViewModel(Layer layer, IHistory history)
    {
        Disposable = new CompositeDisposable();

        this.Name = layer.ToReactivePropertyAsSynchronized(l => l.Name)
            .SetValidateNotifyError(name => string.IsNullOrWhiteSpace(name) ? "Name can not be null or whitespace." : null)
            .AddTo(this.Disposable);

        this.Shapes = layer.Shapes
            .ToReadOnlyReactiveCollection(x => new LineShapeViewModel(x, history))
            .AddTo(this.Disposable);

        this.Name.ObserveWithHistory(name => layer.Name = name, layer.Name, history).AddTo(this.Disposable);
        
        UndoCommand = new ReactiveCommand(history.CanUndo, false);
        UndoCommand.Subscribe(_ => history.Undo()).AddTo(this.Disposable);

        RedoCommand = new ReactiveCommand(history.CanRedo, false);
        RedoCommand.Subscribe(_ => history.Redo()).AddTo(this.Disposable);

        ClearCommand = new ReactiveCommand(history.CanClear, false);
        ClearCommand.Subscribe(_ => history.Clear()).AddTo(this.Disposable);
    }

    public void Dispose()
    {
        this.Disposable.Dispose();
    }
}
```

[Initialization](https://github.com/wieslawsoltes/ReactiveHistory/tree/master/samples/ReactiveHistorySample.Wpf)

```C#
var layer1 = new Layer("layer1");

var line1 = new LineShape(layer1, "line1");
line1.Start = new PointShape(100, 100, line1, "start11");
line1.End = new PointShape(200, 100, line1, "end11");

var line2 = new LineShape(layer1, "line2");
line2.Start = new PointShape(100, 200, line2, "start21");
line2.End = new PointShape(200, 200, line2, "end21");

layer1.Shapes.Add(line1);
layer1.Shapes.Add(line2);

var history = new StackHistory();
var layerViewModel = new LayerViewModel(layer1, history).AddTo(disposable);
```

## Building ReactiveHistory

First, clone the repository or download the latest zip.
```
git clone https://github.com/wieslawsoltes/ReactiveHistory.git
git submodule update --init --recursive
```

## NuGet

ReactiveHistory is delivered as a NuGet package.

You can find the packages here [NuGet](https://www.nuget.org/packages/ReactiveHistory/) or by using nightly build feed:
* Add `https://www.myget.org/F/reactivehistory-nightly/api/v2` to your package sources
* Alternative nightly build feed `https://pkgs.dev.azure.com/wieslawsoltes/GitHub/_packaging/Nightly/nuget/v3/index.json`
* Update your package using `ReactiveHistory` feed

You can install the package like this:

`Install-Package ReactiveHistory -Pre`

### Package Sources

* https://api.nuget.org/v3/index.json

## License

ReactiveHistory is licensed under the [MIT license](LICENSE.TXT).
