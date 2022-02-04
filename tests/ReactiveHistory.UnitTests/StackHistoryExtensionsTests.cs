using System.Collections.ObjectModel;
using Xunit;

namespace ReactiveHistory.UnitTests;

public class StackHistoryExtensionsTests
{
    [Fact]
    [Trait("ReactiveHistory", "StackHistoryExtensions")]
    public void AddWithHistory_Adds_Item_Empty_List()
    {
        var history = new StackHistory();
        var target = new ObservableCollection<Item>();
        var item0 = new Item("item0");

        target.AddWithHistory(item0, history);
        Assert.Single(target);
        Assert.Equal(item0, target[0]);

        history.Undo();
        Assert.Empty(target);

        history.Redo();
        Assert.Single(target);
        Assert.Equal(item0, target[0]);
    }

    [Fact]
    [Trait("ReactiveHistory", "StackHistoryExtensions")]
    public void AddWithHistory_Adds_Items_As_Last()
    {
        var history = new StackHistory();
        var target = new ObservableCollection<Item>();
        var item0 = new Item("item0");
        var item1 = new Item("item1");
        var item2 = new Item("item2");

        target.AddWithHistory(item0, history);
        target.AddWithHistory(item1, history);
        target.AddWithHistory(item2, history);
        Assert.Equal(3, target.Count);
        Assert.Equal(item0, target[0]);
        Assert.Equal(item1, target[1]);
        Assert.Equal(item2, target[2]);

        history.Undo();
        Assert.Equal(2, target.Count);
        Assert.Equal(item0, target[0]);
        Assert.Equal(item1, target[1]);

        history.Undo();
        Assert.Single(target);
        Assert.Equal(item0, target[0]);

        history.Undo();
        Assert.Empty(target);

        history.Redo();
        Assert.Single(target);
        Assert.Equal(item0, target[0]);

        history.Redo();
        Assert.Equal(2, target.Count);
        Assert.Equal(item0, target[0]);
        Assert.Equal(item1, target[1]);

        history.Redo();
        Assert.Equal(3, target.Count);
        Assert.Equal(item0, target[0]);
        Assert.Equal(item1, target[1]);
        Assert.Equal(item2, target[2]);
    }

    [Fact]
    [Trait("ReactiveHistory", "StackHistoryExtensions")]
    public void InsertWithHistory_Inserts_Item_Empty_List()
    {
        var history = new StackHistory();
        var target = new ObservableCollection<Item>();
        var item0 = new Item("item0");

        target.InsertWithHistory(0, item0, history);
        Assert.Single(target);
        Assert.Equal(item0, target[0]);

        history.Undo();
        Assert.Empty(target);

        history.Redo();
        Assert.Single(target);
        Assert.Equal(item0, target[0]);
    }

    [Fact]
    [Trait("ReactiveHistory", "StackHistoryExtensions")]
    public void InsertWithHistory_Inserts_Items_List_Head()
    {
        var history = new StackHistory();
        var target = new ObservableCollection<Item>();
        var item0 = new Item("item0");
        var item1 = new Item("item1");
        var item2 = new Item("item2");

        target.InsertWithHistory(0, item0, history);
        target.InsertWithHistory(0, item1, history);
        target.InsertWithHistory(0, item2, history);
        Assert.Equal(3, target.Count);
        Assert.Equal(item2, target[0]);
        Assert.Equal(item1, target[1]);
        Assert.Equal(item0, target[2]);

        history.Undo();
        Assert.Equal(2, target.Count);
        Assert.Equal(item1, target[0]);
        Assert.Equal(item0, target[1]);

        history.Undo();
        Assert.Single(target);
        Assert.Equal(item0, target[0]);

        history.Undo();
        Assert.Empty(target);

        history.Redo();
        Assert.Single(target);
        Assert.Equal(item0, target[0]);

        history.Redo();
        Assert.Equal(2, target.Count);
        Assert.Equal(item1, target[0]);
        Assert.Equal(item0, target[1]);

        history.Redo();
        Assert.Equal(3, target.Count);
        Assert.Equal(item2, target[0]);
        Assert.Equal(item1, target[1]);
        Assert.Equal(item0, target[2]);
    }

    [Fact]
    [Trait("ReactiveHistory", "StackHistoryExtensions")]
    public void InsertWithHistory_Inserts_Items_List_Tail()
    {
        var history = new StackHistory();
        var target = new ObservableCollection<Item>();
        var item0 = new Item("item0");
        var item1 = new Item("item1");
        var item2 = new Item("item2");

        target.InsertWithHistory(0, item0, history);
        target.InsertWithHistory(1, item1, history);
        target.InsertWithHistory(2, item2, history);
        Assert.Equal(3, target.Count);
        Assert.Equal(item0, target[0]);
        Assert.Equal(item1, target[1]);
        Assert.Equal(item2, target[2]);

        history.Undo();
        Assert.Equal(2, target.Count);
        Assert.Equal(item0, target[0]);
        Assert.Equal(item1, target[1]);

        history.Undo();
        Assert.Single(target);
        Assert.Equal(item0, target[0]);

        history.Undo();
        Assert.Empty(target);

        history.Redo();
        Assert.Single(target);
        Assert.Equal(item0, target[0]);

        history.Redo();
        Assert.Equal(2, target.Count);
        Assert.Equal(item0, target[0]);
        Assert.Equal(item1, target[1]);

        history.Redo();
        Assert.Equal(3, target.Count);
        Assert.Equal(item0, target[0]);
        Assert.Equal(item1, target[1]);
        Assert.Equal(item2, target[2]);
    }

    [Fact]
    [Trait("ReactiveHistory", "StackHistoryExtensions")]
    public void InsertWithHistory_Inserts_Items_List_Middle()
    {
        var history = new StackHistory();
        var target = new ObservableCollection<Item>();
        var item0 = new Item("item0");
        var item1 = new Item("item1");
        var item2 = new Item("item2");

        target.InsertWithHistory(0, item0, history);
        target.InsertWithHistory(1, item1, history);
        target.InsertWithHistory(1, item2, history);
        Assert.Equal(3, target.Count);
        Assert.Equal(item0, target[0]);
        Assert.Equal(item2, target[1]);
        Assert.Equal(item1, target[2]);

        history.Undo();
        Assert.Equal(2, target.Count);
        Assert.Equal(item0, target[0]);
        Assert.Equal(item1, target[1]);

        history.Undo();
        Assert.Single(target);
        Assert.Equal(item0, target[0]);

        history.Undo();
        Assert.Empty(target);

        history.Redo();
        Assert.Single(target);
        Assert.Equal(item0, target[0]);

        history.Redo();
        Assert.Equal(2, target.Count);
        Assert.Equal(item0, target[0]);
        Assert.Equal(item1, target[1]);

        history.Redo();
        Assert.Equal(3, target.Count);
        Assert.Equal(item0, target[0]);
        Assert.Equal(item2, target[1]);
        Assert.Equal(item1, target[2]);
    }

    [Fact]
    [Trait("ReactiveHistory", "StackHistoryExtensions")]
    public void ReplaceWithHistory_Replaces_Item_Empty_List()
    {
        var history = new StackHistory();
        var target = new ObservableCollection<Item>();
        var item0 = new Item("item0");
        var item1 = new Item("item1");

        target.Add(item0);
        Assert.Single(target);
        Assert.Equal(item0, target[0]);

        target.ReplaceWithHistory(0, item1, history);
        Assert.Single(target);
        Assert.Equal(item1, target[0]);

        history.Undo();
        Assert.Single(target);
        Assert.Equal(item0, target[0]);

        history.Redo();
        Assert.Single(target);
        Assert.Equal(item1, target[0]);
    }

    [Fact]
    [Trait("ReactiveHistory", "StackHistoryExtensions")]
    public void ReplaceWithHistory_Replaces_Items_List_Head()
    {
        var history = new StackHistory();
        var target = new ObservableCollection<Item>();
        var item0 = new Item("item0");
        var item1 = new Item("item1");
        var item2 = new Item("item2");
        var item3 = new Item("item3");
        var replace1 = new Item("replace1");
        var replace2 = new Item("replace2");

        target.Add(item0);
        target.Add(item1);
        target.Add(item2);
        target.Add(item3);
        Assert.Equal(4, target.Count);
        Assert.Equal(item0, target[0]);
        Assert.Equal(item1, target[1]);
        Assert.Equal(item2, target[2]);
        Assert.Equal(item3, target[3]);

        target.ReplaceWithHistory(0, replace1, history);
        target.ReplaceWithHistory(1, replace2, history);
        Assert.Equal(4, target.Count);
        Assert.Equal(replace1, target[0]);
        Assert.Equal(replace2, target[1]);
        Assert.Equal(item2, target[2]);
        Assert.Equal(item3, target[3]);

        history.Undo();
        Assert.Equal(4, target.Count);
        Assert.Equal(replace1, target[0]);
        Assert.Equal(item1, target[1]);
        Assert.Equal(item2, target[2]);
        Assert.Equal(item3, target[3]);

        history.Undo();
        Assert.Equal(4, target.Count);
        Assert.Equal(item0, target[0]);
        Assert.Equal(item1, target[1]);
        Assert.Equal(item2, target[2]);
        Assert.Equal(item3, target[3]);

        history.Redo();
        Assert.Equal(4, target.Count);
        Assert.Equal(replace1, target[0]);
        Assert.Equal(item1, target[1]);
        Assert.Equal(item2, target[2]);
        Assert.Equal(item3, target[3]);

        history.Redo();
        Assert.Equal(4, target.Count);
        Assert.Equal(replace1, target[0]);
        Assert.Equal(replace2, target[1]);
        Assert.Equal(item2, target[2]);
        Assert.Equal(item3, target[3]);
    }

    [Fact]
    [Trait("ReactiveHistory", "StackHistoryExtensions")]
    public void ReplaceWithHistory_Replaces_Items_List_Tail()
    {
        var history = new StackHistory();
        var target = new ObservableCollection<Item>();
        var item0 = new Item("item0");
        var item1 = new Item("item1");
        var item2 = new Item("item2");
        var item3 = new Item("item3");
        var replace1 = new Item("replace1");
        var replace2 = new Item("replace2");

        target.Add(item0);
        target.Add(item1);
        target.Add(item2);
        target.Add(item3);
        Assert.Equal(4, target.Count);
        Assert.Equal(item0, target[0]);
        Assert.Equal(item1, target[1]);
        Assert.Equal(item2, target[2]);
        Assert.Equal(item3, target[3]);

        target.ReplaceWithHistory(3, replace1, history);
        target.ReplaceWithHistory(2, replace2, history);
        Assert.Equal(4, target.Count);
        Assert.Equal(item0, target[0]);
        Assert.Equal(item1, target[1]);
        Assert.Equal(replace2, target[2]);
        Assert.Equal(replace1, target[3]);

        history.Undo();
        Assert.Equal(4, target.Count);
        Assert.Equal(item0, target[0]);
        Assert.Equal(item1, target[1]);
        Assert.Equal(item2, target[2]);
        Assert.Equal(replace1, target[3]);

        history.Undo();
        Assert.Equal(4, target.Count);
        Assert.Equal(item0, target[0]);
        Assert.Equal(item1, target[1]);
        Assert.Equal(item2, target[2]);
        Assert.Equal(item3, target[3]);

        history.Redo();
        Assert.Equal(4, target.Count);
        Assert.Equal(item0, target[0]);
        Assert.Equal(item1, target[1]);
        Assert.Equal(item2, target[2]);
        Assert.Equal(replace1, target[3]);

        history.Redo();
        Assert.Equal(item0, target[0]);
        Assert.Equal(item1, target[1]);
        Assert.Equal(replace2, target[2]);
        Assert.Equal(replace1, target[3]);
    }

    [Fact]
    [Trait("ReactiveHistory", "StackHistoryExtensions")]
    public void ReplaceWithHistory_Replaces_Items_List_Middle()
    {
        var history = new StackHistory();
        var target = new ObservableCollection<Item>();
        var item0 = new Item("item0");
        var item1 = new Item("item1");
        var item2 = new Item("item2");
        var item3 = new Item("item3");
        var replace1 = new Item("replace1");
        var replace2 = new Item("replace2");

        target.Add(item0);
        target.Add(item1);
        target.Add(item2);
        target.Add(item3);
        Assert.Equal(4, target.Count);
        Assert.Equal(item0, target[0]);
        Assert.Equal(item1, target[1]);
        Assert.Equal(item2, target[2]);
        Assert.Equal(item3, target[3]);

        target.ReplaceWithHistory(1, replace1, history);
        target.ReplaceWithHistory(2, replace2, history);
        Assert.Equal(4, target.Count);
        Assert.Equal(item0, target[0]);
        Assert.Equal(replace1, target[1]);
        Assert.Equal(replace2, target[2]);
        Assert.Equal(item3, target[3]);

        history.Undo();
        Assert.Equal(4, target.Count);
        Assert.Equal(item0, target[0]);
        Assert.Equal(replace1, target[1]);
        Assert.Equal(item2, target[2]);
        Assert.Equal(item3, target[3]);

        history.Undo();
        Assert.Equal(4, target.Count);
        Assert.Equal(item0, target[0]);
        Assert.Equal(item1, target[1]);
        Assert.Equal(item2, target[2]);
        Assert.Equal(item3, target[3]);

        history.Redo();
        Assert.Equal(4, target.Count);
        Assert.Equal(item0, target[0]);
        Assert.Equal(replace1, target[1]);
        Assert.Equal(item2, target[2]);
        Assert.Equal(item3, target[3]);

        history.Redo();
        Assert.Equal(4, target.Count);
        Assert.Equal(item0, target[0]);
        Assert.Equal(replace1, target[1]);
        Assert.Equal(replace2, target[2]);
        Assert.Equal(item3, target[3]);
    }

    [Fact]
    [Trait("ReactiveHistory", "StackHistoryExtensions")]
    public void RemoveWithHistory_Removes_Item_Empty_List()
    {
        var history = new StackHistory();
        var target = new ObservableCollection<Item>();
        var item0 = new Item("item0");

        target.Add(item0);
        Assert.Single(target);
        Assert.Equal(item0, target[0]);

        target.RemoveWithHistory(item0, history);
        Assert.Empty(target);

        history.Undo();
        Assert.Single(target);
        Assert.Equal(item0, target[0]);

        history.Redo();
        Assert.Empty(target);
    }

    [Fact]
    [Trait("ReactiveHistory", "StackHistoryExtensions")]
    public void RemoveWithHistory_Removes_Item_List_Head()
    {
        var history = new StackHistory();
        var target = new ObservableCollection<Item>();
        var item0 = new Item("item0");
        var item1 = new Item("item1");
        var item2 = new Item("item2");

        target.Add(item0);
        target.Add(item1);
        target.Add(item2);
        Assert.Equal(3, target.Count);
        Assert.Equal(item0, target[0]);
        Assert.Equal(item1, target[1]);
        Assert.Equal(item2, target[2]);

        target.RemoveWithHistory(item0, history);
        Assert.Equal(2, target.Count);
        Assert.Equal(item1, target[0]);
        Assert.Equal(item2, target[1]);

        target.RemoveWithHistory(item1, history);
        Assert.Single(target);
        Assert.Equal(item2, target[0]);

        target.RemoveWithHistory(item2, history);
        Assert.Empty(target);

        history.Undo();
        Assert.Single(target);
        Assert.Equal(item2, target[0]);

        history.Undo();
        Assert.Equal(2, target.Count);
        Assert.Equal(item1, target[0]);
        Assert.Equal(item2, target[1]);

        history.Undo();
        Assert.Equal(3, target.Count);
        Assert.Equal(item0, target[0]);
        Assert.Equal(item1, target[1]);
        Assert.Equal(item2, target[2]);

        history.Redo();
        Assert.Equal(2, target.Count);
        Assert.Equal(item1, target[0]);
        Assert.Equal(item2, target[1]);

        history.Redo();
        Assert.Single(target);
        Assert.Equal(item2, target[0]);

        history.Redo();
        Assert.Empty(target);
    }

    [Fact]
    [Trait("ReactiveHistory", "StackHistoryExtensions")]
    public void RemoveWithHistory_Removes_Item_List_Tail()
    {
        var history = new StackHistory();
        var target = new ObservableCollection<Item>();
        var item0 = new Item("item0");
        var item1 = new Item("item1");
        var item2 = new Item("item2");

        target.Add(item0);
        target.Add(item1);
        target.Add(item2);
        Assert.Equal(3, target.Count);
        Assert.Equal(item0, target[0]);
        Assert.Equal(item1, target[1]);
        Assert.Equal(item2, target[2]);

        target.RemoveWithHistory(item2, history);
        Assert.Equal(2, target.Count);
        Assert.Equal(item0, target[0]);
        Assert.Equal(item1, target[1]);

        target.RemoveWithHistory(item1, history);
        Assert.Single(target);
        Assert.Equal(item0, target[0]);

        target.RemoveWithHistory(item0, history);
        Assert.Empty(target);

        history.Undo();
        Assert.Single(target);
        Assert.Equal(item0, target[0]);

        history.Undo();
        Assert.Equal(2, target.Count);
        Assert.Equal(item0, target[0]);
        Assert.Equal(item1, target[1]);

        history.Undo();
        Assert.Equal(3, target.Count);
        Assert.Equal(item0, target[0]);
        Assert.Equal(item1, target[1]);
        Assert.Equal(item2, target[2]);

        history.Redo();
        Assert.Equal(2, target.Count);
        Assert.Equal(item0, target[0]);
        Assert.Equal(item1, target[1]);

        history.Redo();
        Assert.Single(target);
        Assert.Equal(item0, target[0]);

        history.Redo();
        Assert.Empty(target);
    }

    [Fact]
    [Trait("ReactiveHistory", "StackHistoryExtensions")]
    public void RemoveWithHistory_Removes_Item_List_Middle()
    {
        var history = new StackHistory();
        var target = new ObservableCollection<Item>();
        var item0 = new Item("item0");
        var item1 = new Item("item1");
        var item2 = new Item("item2");

        target.Add(item0);
        target.Add(item1);
        target.Add(item2);
        Assert.Equal(3, target.Count);
        Assert.Equal(item0, target[0]);
        Assert.Equal(item1, target[1]);
        Assert.Equal(item2, target[2]);

        target.RemoveWithHistory(item1, history);
        Assert.Equal(2, target.Count);
        Assert.Equal(item0, target[0]);
        Assert.Equal(item2, target[1]);

        target.RemoveWithHistory(item2, history);
        Assert.Single(target);
        Assert.Equal(item0, target[0]);

        target.RemoveWithHistory(item0, history);
        Assert.Empty(target);

        history.Undo();
        Assert.Single(target);
        Assert.Equal(item0, target[0]);

        history.Undo();
        Assert.Equal(2, target.Count);
        Assert.Equal(item0, target[0]);
        Assert.Equal(item2, target[1]);

        history.Undo();
        Assert.Equal(3, target.Count);
        Assert.Equal(item0, target[0]);
        Assert.Equal(item1, target[1]);
        Assert.Equal(item2, target[2]);

        history.Redo();
        Assert.Equal(2, target.Count);
        Assert.Equal(item0, target[0]);
        Assert.Equal(item2, target[1]);

        history.Redo();
        Assert.Single(target);
        Assert.Equal(item0, target[0]);

        history.Redo();
        Assert.Empty(target);
    }

    [Fact]
    [Trait("ReactiveHistory", "StackHistoryExtensions")]
    public void RemoveWithHistory_Removes_Index_Empty_List()
    {
        var history = new StackHistory();
        var target = new ObservableCollection<Item>();
        var item0 = new Item("item0");

        target.Add(item0);
        Assert.Single(target);
        Assert.Equal(item0, target[0]);

        target.RemoveWithHistory(0, history);
        Assert.Empty(target);

        history.Undo();
        Assert.Single(target);
        Assert.Equal(item0, target[0]);

        history.Redo();
        Assert.Empty(target);
    }

    [Fact]
    [Trait("ReactiveHistory", "StackHistoryExtensions")]
    public void RemoveWithHistory_Removes_Index_Item_List_Head()
    {
        var history = new StackHistory();
        var target = new ObservableCollection<Item>();
        var item0 = new Item("item0");
        var item1 = new Item("item1");
        var item2 = new Item("item2");

        target.Add(item0);
        target.Add(item1);
        target.Add(item2);
        Assert.Equal(3, target.Count);
        Assert.Equal(item0, target[0]);
        Assert.Equal(item1, target[1]);
        Assert.Equal(item2, target[2]);

        target.RemoveWithHistory(0, history);
        Assert.Equal(2, target.Count);
        Assert.Equal(item1, target[0]);
        Assert.Equal(item2, target[1]);

        target.RemoveWithHistory(0, history);
        Assert.Single(target);
        Assert.Equal(item2, target[0]);

        target.RemoveWithHistory(0, history);
        Assert.Empty(target);

        history.Undo();
        Assert.Single(target);
        Assert.Equal(item2, target[0]);

        history.Undo();
        Assert.Equal(2, target.Count);
        Assert.Equal(item1, target[0]);
        Assert.Equal(item2, target[1]);

        history.Undo();
        Assert.Equal(3, target.Count);
        Assert.Equal(item0, target[0]);
        Assert.Equal(item1, target[1]);
        Assert.Equal(item2, target[2]);

        history.Redo();
        Assert.Equal(2, target.Count);
        Assert.Equal(item1, target[0]);
        Assert.Equal(item2, target[1]);

        history.Redo();
        Assert.Single(target);
        Assert.Equal(item2, target[0]);

        history.Redo();
        Assert.Empty(target);
    }

    [Fact]
    [Trait("ReactiveHistory", "StackHistoryExtensions")]
    public void RemoveWithHistory_Removes_Index_Item_List_Tail()
    {
        var history = new StackHistory();
        var target = new ObservableCollection<Item>();
        var item0 = new Item("item0");
        var item1 = new Item("item1");
        var item2 = new Item("item2");

        target.Add(item0);
        target.Add(item1);
        target.Add(item2);
        Assert.Equal(3, target.Count);
        Assert.Equal(item0, target[0]);
        Assert.Equal(item1, target[1]);
        Assert.Equal(item2, target[2]);

        target.RemoveWithHistory(2, history);
        Assert.Equal(2, target.Count);
        Assert.Equal(item0, target[0]);
        Assert.Equal(item1, target[1]);

        target.RemoveWithHistory(1, history);
        Assert.Single(target);
        Assert.Equal(item0, target[0]);

        target.RemoveWithHistory(0, history);
        Assert.Empty(target);

        history.Undo();
        Assert.Single(target);
        Assert.Equal(item0, target[0]);

        history.Undo();
        Assert.Equal(2, target.Count);
        Assert.Equal(item0, target[0]);
        Assert.Equal(item1, target[1]);

        history.Undo();
        Assert.Equal(3, target.Count);
        Assert.Equal(item0, target[0]);
        Assert.Equal(item1, target[1]);
        Assert.Equal(item2, target[2]);

        history.Redo();
        Assert.Equal(2, target.Count);
        Assert.Equal(item0, target[0]);
        Assert.Equal(item1, target[1]);

        history.Redo();
        Assert.Single(target);
        Assert.Equal(item0, target[0]);

        history.Redo();
        Assert.Empty(target);
    }

    [Fact]
    [Trait("ReactiveHistory", "StackHistoryExtensions")]
    public void RemoveWithHistory_Removes_Index_Item_List_Middle()
    {
        var history = new StackHistory();
        var target = new ObservableCollection<Item>();
        var item0 = new Item("item0");
        var item1 = new Item("item1");
        var item2 = new Item("item2");

        target.Add(item0);
        target.Add(item1);
        target.Add(item2);
        Assert.Equal(3, target.Count);
        Assert.Equal(item0, target[0]);
        Assert.Equal(item1, target[1]);
        Assert.Equal(item2, target[2]);

        target.RemoveWithHistory(1, history);
        Assert.Equal(2, target.Count);
        Assert.Equal(item0, target[0]);
        Assert.Equal(item2, target[1]);

        target.RemoveWithHistory(1, history);
        Assert.Single(target);
        Assert.Equal(item0, target[0]);

        target.RemoveWithHistory(0, history);
        Assert.Empty(target);

        history.Undo();
        Assert.Single(target);
        Assert.Equal(item0, target[0]);

        history.Undo();
        Assert.Equal(2, target.Count);
        Assert.Equal(item0, target[0]);
        Assert.Equal(item2, target[1]);

        history.Undo();
        Assert.Equal(3, target.Count);
        Assert.Equal(item0, target[0]);
        Assert.Equal(item1, target[1]);
        Assert.Equal(item2, target[2]);

        history.Redo();
        Assert.Equal(2, target.Count);
        Assert.Equal(item0, target[0]);
        Assert.Equal(item2, target[1]);

        history.Redo();
        Assert.Single(target);
        Assert.Equal(item0, target[0]);

        history.Redo();
        Assert.Empty(target);
    }

    [Fact]
    [Trait("ReactiveHistory", "StackHistoryExtensions")]
    public void ClearWithHistory_Does_Nothing_Empty_List()
    {
        var history = new StackHistory();
        var target = new ObservableCollection<Item>();
        Assert.Empty(target);
        Assert.Empty(history.Undos);
        Assert.Empty(history.Redos);

        target.ClearWithHistory(history);
        Assert.Empty(target);
        Assert.Empty(history.Undos);
        Assert.Empty(history.Redos);
    }

    [Fact]
    [Trait("ReactiveHistory", "StackHistoryExtensions")]
    public void ClearWithHistory_Removes_All_Items_Single_Item()
    {
        var history = new StackHistory();
        var target = new ObservableCollection<Item>();
        var item0 = new Item("item0");

        target.Add(item0);
        Assert.Single(target);
        Assert.Empty(history.Undos);
        Assert.Empty(history.Redos);

        target.ClearWithHistory(history);
        Assert.Empty(target);
        Assert.Single(history.Undos);
        Assert.Empty(history.Redos);

        history.Undo();
        Assert.Single(target);
        Assert.Equal(item0, target[0]);
        Assert.Empty(history.Undos);
        Assert.Single(history.Redos);

        history.Redo();
        Assert.Empty(target);
        Assert.Single(history.Undos);
        Assert.Empty(history.Redos);
    }

    [Fact]
    [Trait("ReactiveHistory", "StackHistoryExtensions")]
    public void ClearWithHistory_Removes_All_Items_Multiple_Item()
    {
        var history = new StackHistory();
        var target = new ObservableCollection<Item>();
        var item0 = new Item("item0");
        var item1 = new Item("item1");
        var item2 = new Item("item2");

        target.Add(item0);
        target.Add(item1);
        target.Add(item2);
        Assert.Equal(3, target.Count);
        Assert.Equal(item0, target[0]);
        Assert.Equal(item1, target[1]);
        Assert.Equal(item2, target[2]);
        Assert.Empty(history.Undos);
        Assert.Empty(history.Redos);

        target.ClearWithHistory(history);
        Assert.Empty(target);
        Assert.Single(history.Undos);
        Assert.Empty(history.Redos);

        history.Undo();
        Assert.Equal(3, target.Count);
        Assert.Equal(item0, target[0]);
        Assert.Equal(item1, target[1]);
        Assert.Equal(item2, target[2]);
        Assert.Empty(history.Undos);
        Assert.Single(history.Redos);

        history.Redo();
        Assert.Empty(target);
        Assert.Single(history.Undos);
        Assert.Empty(history.Redos);
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