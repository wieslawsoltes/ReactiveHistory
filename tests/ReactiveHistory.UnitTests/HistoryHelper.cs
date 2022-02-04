using System;
using System.Collections.Generic;
using System.Reactive.Disposables;

namespace ReactiveHistory.UnitTests;

internal class HistoryHelper : IDisposable
{
    CompositeDisposable _disposable;
    IList<bool> _canUndos;
    IList<bool> _canRedos;
    IList<bool> _canClears;
    public IList<bool> CanUndos { get { return _canUndos; } }
    public IList<bool> CanRedos { get { return _canRedos; } }
    public IList<bool> CanClears { get { return _canClears; } }

    public HistoryHelper(IHistory target)
    {
        _disposable = new CompositeDisposable();
        _canUndos = new List<bool>();
        _canRedos = new List<bool>();
        _canClears = new List<bool>();
        _disposable.Add(target.CanUndo.Subscribe(x => _canUndos.Add(x)));
        _disposable.Add(target.CanRedo.Subscribe(x => _canRedos.Add(x)));
        _disposable.Add(target.CanClear.Subscribe(x => _canClears.Add(x)));
    }

    public void Dispose()
    {
        _disposable.Dispose();
    }
}