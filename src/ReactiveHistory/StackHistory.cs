using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace ReactiveHistory;

/// <summary>
/// Undo/redo stack based action history.
/// </summary>
public class StackHistory : IHistory, IDisposable
{
    private readonly Subject<bool> _canUndo;
    private readonly Subject<bool> _canRedo;
    private readonly Subject<bool> _canClear;
    private volatile bool _isPaused;

    /// <summary>
    /// Gets or sets undo states stack.
    /// </summary>
    public Stack<State> Undos { get; set; }

    /// <summary>
    /// Gets or sets redo states stack.
    /// </summary>
    public Stack<State> Redos { get; set; }

    /// <inheritdoc/>
    public bool IsPaused
    {
        get { return _isPaused; }
        set { _isPaused = value; }
    }

    /// <inheritdoc/>
    public IObservable<bool> CanUndo
    {
        get { return _canUndo.AsObservable(); }
    }

    /// <inheritdoc/>
    public IObservable<bool> CanRedo
    {
        get { return _canRedo.AsObservable(); }
    }

    /// <inheritdoc/>
    public IObservable<bool> CanClear
    {
        get { return _canClear.AsObservable(); }
    }

    /// <summary>
    /// Initializes a new <see cref="StackHistory"/> instance.
    /// </summary>
    public StackHistory()
    {
        Undos = new Stack<State>();
        Redos = new Stack<State>();

        _isPaused = false;
        _canUndo = new Subject<bool>();
        _canRedo = new Subject<bool>();
        _canClear = new Subject<bool>();
    }

    /// <inheritdoc/>
    public void Snapshot(Action undo, Action redo)
    {
        if (undo == null)
            throw new ArgumentNullException(nameof(undo));

        if (redo == null)
            throw new ArgumentNullException(nameof(redo));

        if (Redos.Count > 0)
        {
            Redos.Clear();
            _canRedo.OnNext(false);
        }
        Undos.Push(new State(undo, redo, string.Empty, string.Empty));
        _canUndo.OnNext(true);
        _canClear.OnNext(true);
    }

    /// <inheritdoc/>
    public bool Undo()
    {
        if (Undos.Count > 0)
        {
            IsPaused = true;
            var state = Undos.Pop();
            if (Undos.Count == 0)
            {
                _canUndo.OnNext(false);
            }
            state.Undo.Invoke();
            Redos.Push(state);
            _canRedo.OnNext(true);
            _canClear.OnNext(true);
            IsPaused = false;
            return true;
        }
        return false;
    }

    /// <inheritdoc/>
    public bool Redo()
    {
        if (Redos.Count > 0)
        {
            IsPaused = true;
            var state = Redos.Pop();
            if (Redos.Count == 0)
            {
                _canRedo.OnNext(false);
            }
            state.Redo.Invoke();
            Undos.Push(state);
            _canUndo.OnNext(true);
            _canClear.OnNext(true);
            IsPaused = false;
            return true;
        }
        return false;
    }

    /// <inheritdoc/>
    public void Clear()
    {
        Undos.Clear();
        Redos.Clear();
        _canUndo.OnNext(false);
        _canRedo.OnNext(false);
        _canClear.OnNext(false);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Undos.Clear();
        Redos.Clear();
        _canUndo.Dispose();
        _canRedo.Dispose();
        _canClear.Dispose();
    }
}