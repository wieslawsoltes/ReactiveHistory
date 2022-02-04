
namespace ReactiveHistorySample.Models;

public abstract class BaseObject : ObservableObject
{
    private object _owner;
    private string _name;

    public object Owner
    {
        get { return _owner; }
        set { Update(ref _owner, value); }
    }

    public string Name
    {
        get { return _name; }
        set { Update(ref _name, value); }
    }

    public BaseObject(object owner, string name)
    {
        _owner = owner;
        _name = name;
    }
}