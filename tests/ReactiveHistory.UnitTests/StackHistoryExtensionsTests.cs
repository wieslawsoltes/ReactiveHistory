﻿// Copyright (c) Wiesław Šoltés. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System.Collections.ObjectModel;
using Xunit;

namespace ReactiveHistory.UnitTests
{
    public class StackHistoryExtensionsTests
    {
        [Fact]
        [Trait("ReactiveHistory", "StackHistoryExtensions")]
        public void CreateWithHistory_Add_Item_Empty_List()
        {
            var history = new StackHistory();
            var target = new ObservableCollection<Item>();
            var item0 = new Item("item0");

            target.CreateWithHistory(item0, history);
            Assert.Equal(1, target.Count);
            Assert.Equal(item0, target[0]);

            history.Undo();
            Assert.Equal(0, target.Count);

            history.Redo();
            Assert.Equal(1, target.Count);
            Assert.Equal(item0, target[0]);
        }

        [Fact]
        [Trait("ReactiveHistory", "StackHistoryExtensions")]
        public void CreateWithHistory_Insert_Item_Empty_List()
        {
            var history = new StackHistory();
            var target = new ObservableCollection<Item>();
            var item0 = new Item("item0");

            target.CreateWithHistory(0, item0, history);
            Assert.Equal(1, target.Count);
            Assert.Equal(item0, target[0]);

            history.Undo();
            Assert.Equal(0, target.Count);

            history.Redo();
            Assert.Equal(1, target.Count);
            Assert.Equal(item0, target[0]);
        }

        [Fact]
        [Trait("ReactiveHistory", "StackHistoryExtensions")]
        public void UpdateWithHistory_Replace_Item_Empty_List()
        {
            var history = new StackHistory();
            var target = new ObservableCollection<Item>();
            var item0 = new Item("item0");
            var item1 = new Item("item0");

            target.Add(item0);
            Assert.Equal(1, target.Count);
            Assert.Equal(item0, target[0]);

            target.UpdateWithHistory(0, item1, history);
            Assert.Equal(1, target.Count);
            Assert.Equal(item1, target[0]);

            history.Undo();
            Assert.Equal(1, target.Count);
            Assert.Equal(item0, target[0]);

            history.Redo();
            Assert.Equal(1, target.Count);
            Assert.Equal(item1, target[0]);
        }

        [Fact]
        [Trait("ReactiveHistory", "StackHistoryExtensions")]
        public void DeleteWithHistory_Remove_Item_Empty_List()
        {
            var history = new StackHistory();
            var target = new ObservableCollection<Item>();
            var item0 = new Item("item0");

            target.Add(item0);
            Assert.Equal(1, target.Count);
            Assert.Equal(item0, target[0]);

            target.DeleteWithHistory(item0, history);
            Assert.Equal(0, target.Count);

            history.Undo();
            Assert.Equal(1, target.Count);
            Assert.Equal(item0, target[0]);

            history.Redo();
            Assert.Equal(0, target.Count);
        }

        [Fact]
        [Trait("ReactiveHistory", "StackHistoryExtensions")]
        public void DeleteWithHistory_Remove_Index_Empty_List()
        {
            var history = new StackHistory();
            var target = new ObservableCollection<Item>();
            var item0 = new Item("item0");

            target.Add(item0);
            Assert.Equal(1, target.Count);
            Assert.Equal(item0, target[0]);

            target.DeleteWithHistory(0, history);
            Assert.Equal(0, target.Count);

            history.Undo();
            Assert.Equal(1, target.Count);
            Assert.Equal(item0, target[0]);

            history.Redo();
            Assert.Equal(0, target.Count);
        }

        class Item
        {
            public string Name { get; set; }

            public Item(string name)
            {
                Name = name;
            }
        }
    }
}