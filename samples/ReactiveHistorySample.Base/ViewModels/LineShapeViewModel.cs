using System;
using System.Reactive.Disposables;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using ReactiveHistory;
using ReactiveHistorySample.Models;

namespace ReactiveHistorySample.ViewModels;

public class LineShapeViewModel : IDisposable
{
    private CompositeDisposable Disposable { get; set; }

    public ReactiveProperty<string> Name { get; set; }
    public ReactiveProperty<PointShapeViewModel> Start { get; set; }
    public ReactiveProperty<PointShapeViewModel> End { get; set; }

    public ReactiveCommand DeleteCommand { get; set; }

    public ReactiveCommand UndoCommand { get; set; }
    public ReactiveCommand RedoCommand { get; set; }
    public ReactiveCommand ClearCommand { get; set; }

    public LineShapeViewModel(LineShape line, IHistory history)
    {
        Disposable = new CompositeDisposable();

        var lineHistoryScope = new StackHistory().AddTo(this.Disposable);

        this.Name = line.ToReactivePropertyAsSynchronized(l => l.Name)
            .SetValidateNotifyError(name => string.IsNullOrWhiteSpace(name) ? "Name can not be null or whitespace." : null)
            .AddTo(this.Disposable);

        var startInitialValue = new PointShapeViewModel(line.Start, lineHistoryScope).AddTo(this.Disposable);
        this.Start = new ReactiveProperty<PointShapeViewModel>(startInitialValue)
            .SetValidateNotifyError(start => start == null ? "Point can not be null." : null)
            .AddTo(this.Disposable);

        var endInitialValue = new PointShapeViewModel(line.End, lineHistoryScope).AddTo(this.Disposable);
        this.End = new ReactiveProperty<PointShapeViewModel>(endInitialValue)
            .SetValidateNotifyError(end => end == null ? "Point can not be null." : null)
            .AddTo(this.Disposable);

        this.Name.ObserveWithHistory(name => line.Name = name, line.Name, lineHistoryScope).AddTo(this.Disposable);

        this.DeleteCommand = new ReactiveCommand();
        this.DeleteCommand.Subscribe((x) => Delete(line, history)).AddTo(this.Disposable);

        UndoCommand = new ReactiveCommand(lineHistoryScope.CanUndo, false);
        UndoCommand.Subscribe(_ => lineHistoryScope.Undo()).AddTo(this.Disposable);

        RedoCommand = new ReactiveCommand(lineHistoryScope.CanRedo, false);
        RedoCommand.Subscribe(_ => lineHistoryScope.Redo()).AddTo(this.Disposable);

        ClearCommand = new ReactiveCommand(lineHistoryScope.CanClear, false);
        ClearCommand.Subscribe(_ => lineHistoryScope.Clear()).AddTo(this.Disposable);
    }

    private void Delete(LineShape line, IHistory history)
    {
        if (line.Owner != null && line.Owner is Layer layer)
        {
            layer.Shapes.RemoveWithHistory(line, history);
        }
    }

    public void Dispose()
    {
        this.Disposable.Dispose();
    }
}