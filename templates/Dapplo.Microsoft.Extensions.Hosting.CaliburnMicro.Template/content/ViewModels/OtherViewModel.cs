using Caliburn.Micro;

namespace Dapplo.Microsoft.Extensions.Hosting.CaliburnMicro.Template.ViewModels
{
    public class OtherViewModel : Screen
    {
        public Task Close() => TryCloseAsync();
    }
}
