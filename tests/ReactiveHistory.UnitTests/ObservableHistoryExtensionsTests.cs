using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Xunit;

namespace ReactiveHistory.UnitTests;

public class ObservableHistoryExtensionsTests
{
    [Fact]
    [Trait("ReactiveHistory", "ObservableHistoryExtensions")]
    public void ObserveWithHistory_Skips_First_Value()
    {
        var target = new StackHistory();

        using (var subject = new Subject<int>())
        using (subject.AsObservable().ObserveWithHistory(x => { }, 0, target))
        {
            subject.OnNext(1);
            Assert.Empty(target.Undos);
        }
    }

    [Fact]
    [Trait("ReactiveHistory", "ObservableHistoryExtensions")]
    public void ObserveWithHistory_Creates_History_Snapshot()
    {
        var target = new StackHistory();

        using (var subject = new Subject<int>())
        using (subject.AsObservable().ObserveWithHistory(x => { }, 0, target))
        {
            subject.OnNext(1);
            subject.OnNext(2);
            subject.OnNext(3);

            Assert.Equal(2, target.Undos.Count);
        }
    }

    [Fact]
    [Trait("ReactiveHistory", "ObservableHistoryExtensions")]
    public void ObserveWithHistory_Does_Not_Create_History_Snapshot_When_IsPaused_True()
    {
        var target = new StackHistory();

        using (var subject = new Subject<int>())
        using (subject.AsObservable().ObserveWithHistory(x => { }, 0, target))
        {
            subject.OnNext(1);
            subject.OnNext(2);

            target.IsPaused = true;
            subject.OnNext(3);
            subject.OnNext(4);
            target.IsPaused = false;

            subject.OnNext(5);
            subject.OnNext(6);

            Assert.Equal(3, target.Undos.Count);
        }
    }

    [Fact]
    [Trait("ReactiveHistory", "ObservableHistoryExtensions")]
    public void ObserveWithHistory_Sets_CurrentValue()
    {
        var history = new StackHistory();

        using (var subject = new Subject<int>())
        {
            var target = new List<int>();
            var initialValue = 10;

            using (subject.AsObservable().ObserveWithHistory(x => target.Add(x), currentValue: initialValue, history: history))
            {
                subject.OnNext(initialValue); // empty -> 10 (the initial state of variable)
                subject.OnNext(2); // empty -> 10 -> 2
                subject.OnNext(3); // empty -> 10 -> 2 -> 3

                history.Undo(); // 3 -> 2
                history.Undo(); // 2 -> 10 (finally restores initial state)

                Assert.Empty(history.Undos);
                Assert.Equal(2, history.Redos.Count);
                Assert.Equal(new int[] { 2, 10 }, target);

                history.Redo(); // 10 -> 2
                history.Redo(); // 2 -> 3

                Assert.Equal(2, history.Undos.Count);
                Assert.Empty(history.Redos);
                Assert.Equal(new int[] { 2, 10, 2, 3 }, target);
            }
        }
    }
}