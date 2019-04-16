using Caliburn.Micro;

namespace Dapplo.Hosting.Sample.CaliburnMicroDemo.ViewModels
{
    public class OtherViewModel : Screen
    {
        public void Exit()
        {
            TryClose();
        }
    }
}
