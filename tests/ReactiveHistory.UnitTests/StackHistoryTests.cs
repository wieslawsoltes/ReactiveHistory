// Copyright (c) Wiesław Šoltés. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System.Linq;
using Xunit;

namespace ReactiveHistory.UnitTests
{
    public class StackHistoryTests
    {
        [Fact]
        [Trait("ReactiveHistory", "StackHistory")]
        public void Undos_And_Redos_Shuould_Be_Initialized()
        {
            var target = new StackHistory();

            using (var helper = new HistoryHelper(target))
            {
                Assert.NotNull(target.Undos);
                Assert.NotNull(target.Redos);
                Assert.Equal(0, target.Undos.Count);
                Assert.Equal(0, target.Redos.Count);
                Assert.Equal(false, target.IsPaused);
                Assert.Equal(new bool[] { }, helper.CanUndos.ToArray());
                Assert.Equal(new bool[] { }, helper.CanRedos.ToArray());
                Assert.Equal(new bool[] { }, helper.CanClears.ToArray());
            }
        }

        [Fact]
        [Trait("ReactiveHistory", "StackHistory")]
        public void First_Snapshot_Should_Push_One_Undo_State()
        {
            var target = new StackHistory();

            using (var helper = new HistoryHelper(target))
            {
                target.Snapshot(() => { }, () => { });
                Assert.Equal(1, target.Undos.Count);
                Assert.Equal(0, target.Redos.Count);
                Assert.Equal(new bool[] { true }, helper.CanUndos.ToArray());
                Assert.Equal(new bool[] { }, helper.CanRedos.ToArray());
                Assert.Equal(new bool[] { true }, helper.CanClears.ToArray());
            }
        }

        [Fact]
        [Trait("ReactiveHistory", "StackHistory")]
        public void Snapshot_Should_Clear_Redos()
        {
            var target = new StackHistory();

            using (var helper = new HistoryHelper(target))
            {
                target.Snapshot(() => { }, () => { });
                Assert.Equal(1, target.Undos.Count);
                Assert.Equal(0, target.Redos.Count);

                var result = target.Undo();
                Assert.Equal(0, target.Undos.Count);
                Assert.Equal(1, target.Redos.Count);
                Assert.Equal(true, result);

                target.Snapshot(() => { }, () => { });
                Assert.Equal(1, target.Undos.Count);
                Assert.Equal(0, target.Redos.Count);
                Assert.Equal(new bool[] { true, false, true }, helper.CanUndos.ToArray());
                Assert.Equal(new bool[] { true, false }, helper.CanRedos.ToArray());
                Assert.Equal(new bool[] { true, true, true }, helper.CanClears.ToArray());
            }
        }

        [Fact]
        [Trait("ReactiveHistory", "StackHistory")]
        public void Invoking_Undo_Should_Not_Throw_When_Undos_Are_Empty()
        {
            var target = new StackHistory();

            using (var helper = new HistoryHelper(target))
            {
                var result = target.Undo();
                Assert.Equal(false, result);
                Assert.Equal(new bool[] { }, helper.CanUndos.ToArray());
                Assert.Equal(new bool[] { }, helper.CanRedos.ToArray());
                Assert.Equal(new bool[] { }, helper.CanClears.ToArray());
            }
        }

        [Fact]
        [Trait("ReactiveHistory", "StackHistory")]
        public void Invoking_Redo_Should_Not_Throw_When_Redos_Are_Empty()
        {
            var target = new StackHistory();

            using (var helper = new HistoryHelper(target))
            {
                var result = target.Redo();
                Assert.Equal(false, result);
                Assert.Equal(new bool[] { }, helper.CanUndos.ToArray());
                Assert.Equal(new bool[] { }, helper.CanRedos.ToArray());
                Assert.Equal(new bool[] { }, helper.CanClears.ToArray());
            }
        }

        [Fact]
        [Trait("ReactiveHistory", "StackHistory")]
        public void Invoking_Undo_Should_Invoke_Undo_Action_And_Push_State_To_Redos()
        {
            int undoCount = 0;
            int redoCount = 0;
            var target = new StackHistory();

            using (var helper = new HistoryHelper(target))
            {
                target.Snapshot(() => undoCount++, () => redoCount++);
                var undo = target.Undos.Peek();
                var result = target.Undo();
                Assert.Equal(1, undoCount);
                Assert.Equal(0, redoCount);
                Assert.Equal(0, target.Undos.Count);
                Assert.Equal(1, target.Redos.Count);
                Assert.Equal(true, result);
                Assert.Equal(new bool[] { true, false }, helper.CanUndos.ToArray());
                Assert.Equal(new bool[] { true }, helper.CanRedos.ToArray());
                Assert.Equal(new bool[] { true, true }, helper.CanClears.ToArray());

                var redo = target.Redos.Peek();
                Assert.Equal(undo, redo);
            }
        }

        [Fact]
        [Trait("ReactiveHistory", "StackHistory")]
        public void Invoking_Redo_Should_Invoke_Redo_Action_And_Push_State_To_Undos()
        {
            int undoCount = 0;
            int redoCount = 0;
            var target = new StackHistory();

            using (var helper = new HistoryHelper(target))
            {
                target.Snapshot(() => undoCount++, () => redoCount++);
                var undo1 = target.Undos.Peek();
                var result1 = target.Undo();
                var redo1 = target.Redos.Peek();
                var result2 = target.Redo();
                Assert.Equal(1, target.Undos.Count);
                Assert.Equal(0, target.Redos.Count);
                Assert.Equal(true, result1);
                Assert.Equal(true, result2);
                Assert.Equal(new bool[] { true, false, true }, helper.CanUndos.ToArray());
                Assert.Equal(new bool[] { true, false }, helper.CanRedos.ToArray());
                Assert.Equal(new bool[] { true, true, true }, helper.CanClears.ToArray());

                var undo2 = target.Undos.Peek();
                Assert.Equal(undo1, undo2);
                Assert.Equal(undo1, redo1);
                Assert.Equal(1, undoCount);
                Assert.Equal(1, redoCount);
                Assert.Equal(true, result1);
            }
        }

        [Fact]
        [Trait("ReactiveHistory", "StackHistory")]
        public void Undo_Sets_IsPaused_True_While_Invoking_Undo_Redo_State()
        {
            var target = new StackHistory();

            target.Snapshot(
                undo: () => 
                {
                    Assert.True(target.IsPaused);
                }, 
                redo: () => 
                {
                    Assert.True(target.IsPaused);
                });

            Assert.False(target.IsPaused);
            target.Undo();
            Assert.False(target.IsPaused);

            Assert.False(target.IsPaused);
            target.Redo();
            Assert.False(target.IsPaused);
        }
    }
}
