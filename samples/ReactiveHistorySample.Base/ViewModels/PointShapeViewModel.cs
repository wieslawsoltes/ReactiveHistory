using System;
using System.Reactive.Disposables;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using ReactiveHistory;
using ReactiveHistorySample.Models;

namespace ReactiveHistorySample.ViewModels;

public class PointShapeViewModel : IDisposable
{
    private CompositeDisposable Disposable { get; set; }

    public ReactiveProperty<string> Name { get; set; }
    public ReactiveProperty<double> X { get; set; }
    public ReactiveProperty<double> Y { get; set; }

    public PointShapeViewModel(PointShape point, IHistory history)
    {
        Disposable = new CompositeDisposable();

        this.Name = point.ToReactivePropertyAsSynchronized(p => p.Name)
            .SetValidateNotifyError(name => string.IsNullOrWhiteSpace(name) ? "Name can not be null or whitespace." : null)
            .AddTo(this.Disposable);

        this.X = point.ToReactivePropertyAsSynchronized(p => p.X)
            .SetValidateNotifyError(x => double.IsNaN(x) || double.IsInfinity(x) ? "X can not be NaN or Infinity." : null)
            .AddTo(this.Disposable);

        this.Y = point.ToReactivePropertyAsSynchronized(p => p.Y)
            .SetValidateNotifyError(y => double.IsNaN(y) || double.IsInfinity(y) ? "Y can not be NaN or Infinity." : null)
            .AddTo(this.Disposable);

        this.Name.ObserveWithHistory(name => point.Name = name, point.Name, history).AddTo(this.Disposable);
        this.X.ObserveWithHistory(x => point.X = x, point.X, history).AddTo(this.Disposable);
        this.Y.ObserveWithHistory(y => point.Y = y, point.Y, history).AddTo(this.Disposable);
    }

    public void Dispose()
    {
        this.Disposable.Dispose();
    }
}