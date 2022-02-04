using System;

namespace ReactiveHistory;

/// <summary>
/// Undo/redo action history contract.
/// </summary>
public interface IHistory
{
    /// <summary>
    /// Gets or sets flag indicating whether history is paused.
    /// </summary>
    bool IsPaused { get; set; }

    /// <summary>
    /// Gets or sets flag indicating whether undo action can execute.
    /// </summary>
    IObservable<bool> CanUndo { get; }

    /// <summary>
    /// Gets or sets flag indicating whether redo action can execute.
    /// </summary>
    IObservable<bool> CanRedo { get; }

    /// <summary>
    /// Gets or sets flag indicating whether clear action can execute.
    /// </summary>
    IObservable<bool> CanClear { get; }

    /// <summary>
    /// Makes undo/redo history snapshot.
    /// </summary>
    /// <param name="undo">The undo state action.</param>
    /// <param name="redo">The redo state action.</param>
    void Snapshot(Action undo, Action redo);

    /// <summary>
    /// Executes undo action.
    /// </summary>
    /// <returns>True if undo action was executed.</returns>
    bool Undo();

    /// <summary>
    /// Executes redo action.
    /// </summary>
    /// <returns>True if redo action was executed.</returns>
    bool Redo();

    /// <summary>
    /// Clears undo/redo actions history.
    /// </summary>
    void Clear();
}