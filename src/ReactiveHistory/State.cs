using System;

namespace ReactiveHistory;

/// <summary>
/// Undo/redo action pair.
/// </summary>
public struct State
{
    /// <summary>
    /// The undo state action.
    /// </summary>
    public readonly Action Undo;

    /// <summary>
    /// The redo state action.
    /// </summary>
    public readonly Action Redo;

    /// <summary>
    /// The undo state name.
    /// </summary>
    public readonly string UndoName;

    /// <summary>
    /// The redo state name.
    /// </summary>
    public readonly string RedoName;

    /// <summary>
    /// Initializes a new <see cref="State"/> instance.
    /// </summary>
    /// <param name="undo">The undo state action.</param>
    /// <param name="redo">The redo state action.</param>
    /// <param name="undoName">The undo state name.</param>
    /// <param name="redoName">The redo state name.</param>
    public State(Action undo, Action redo, string undoName, string redoName)
    {
        Undo = undo;
        Redo = redo;
        UndoName = undoName;
        RedoName = redoName;
    }
}