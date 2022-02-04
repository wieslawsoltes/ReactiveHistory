using System;
using System.Reactive.Linq;

namespace ReactiveHistory;

/// <summary>
/// Observable extension methods for the generic observable implementations.
/// </summary>
public static class IObservableExtensions
{
    /// <summary>
    /// Observe property changes with history.
    /// </summary>
    /// <param name="source">The property value observable.</param>
    /// <param name="update">The property update action.</param>
    /// <param name="currentValue">The property current value.</param>
    /// <param name="history">The history object.</param>
    /// <returns>The property value changes subscription.</returns>
    public static IDisposable ObserveWithHistory<T>(this IObservable<T> source, Action<T> update, T currentValue, IHistory history)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));

        if (update == null)
            throw new ArgumentNullException(nameof(update));

        if (history == null)
            throw new ArgumentNullException(nameof(history));

        var previous = currentValue;

        return source.Skip(1).Subscribe(
            next =>
            {
                if (!history.IsPaused)
                {
                    var undoValue = previous;
                    var redoValue = next;
                    void undo() => update(undoValue);
                    void redo() => update(redoValue);
                    history.Snapshot(undo, redo);
                }
                previous = next;
            });
    }
}