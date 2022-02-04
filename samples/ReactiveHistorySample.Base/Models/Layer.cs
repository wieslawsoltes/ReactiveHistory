using System.Collections.ObjectModel;

namespace ReactiveHistorySample.Models;

public class Layer : BaseObject
{
    private ObservableCollection<LineShape> _shapes;

    public ObservableCollection<LineShape> Shapes
    {
        get { return _shapes; }
        set { Update(ref _shapes, value); }
    }

    public Layer(object owner, string name) : base(owner, name)
    {
        _shapes = new ObservableCollection<LineShape>();
    }
}