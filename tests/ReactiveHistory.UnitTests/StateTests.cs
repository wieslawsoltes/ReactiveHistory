// Copyright (c) Wiesław Šoltés. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using Xunit;

namespace ReactiveHistory.UnitTests
{
    public class StateTests
    {
        [Fact]
        [Trait("ReactiveHistory", "State")]
        public void Constructor_Should_Set_Undo_And_Redo_Fields()
        {
            void undo()
            { }
            void redo()
            { }
            var target = new State(undo, redo, string.Empty, string.Empty);
            Assert.Equal(undo, target.Undo);
            Assert.Equal(redo, target.Redo);
        }
    }
}
