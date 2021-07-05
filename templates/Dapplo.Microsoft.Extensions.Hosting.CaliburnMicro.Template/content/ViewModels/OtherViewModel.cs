using System.Threading.Tasks;
using Caliburn.Micro;

namespace Dapplo.Microsoft.Extensions.Hosting.CaliburnMicro.Template.ViewModels
{
    public class OtherViewModel : Screen
    {
        public Task DoClose() => TryCloseAsync();
    }
}
