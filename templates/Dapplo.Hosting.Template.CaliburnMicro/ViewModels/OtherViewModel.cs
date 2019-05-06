using Caliburn.Micro;

namespace Dapplo.Hosting.CaliburnMicroTemplate.ViewModels
{
    public class OtherViewModel : Screen
    {
        public void Exit()
        {
            TryClose();
        }
    }
}
