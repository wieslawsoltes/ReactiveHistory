using Avalonia.Web.Blazor;

namespace ReactiveHistorySample.Web;

public partial class App
{
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        WebAppBuilder.Configure<ReactiveHistorySample.App>()
            .SetupWithSingleViewLifetime();
    }
}
