using System;
using System.Reactive.Disposables;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using ReactiveHistory;
using ReactiveHistorySample.Models;

namespace ReactiveHistorySample.ViewModels;

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