
namespace ReactiveHistorySample.Models;

public class PointShape : BaseShape
{
    private double _x;
    private double _y;

    public double X
    {
        get { return _x; }
        set { Update(ref _x, value); }
    }

    public double Y
    {
        get { return _y; }
        set { Update(ref _y, value); }
    }

    public PointShape(object owner, string name, double x, double y)
        : base(owner, name)
    {
        _x = x;
        _y = y;
    }
}