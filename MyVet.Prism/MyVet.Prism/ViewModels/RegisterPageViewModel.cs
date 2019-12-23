using Prism.Navigation;

namespace MyVet.Prism.ViewModels
{
    public class RegisterPageViewModel : ViewModelBase
    {
        public RegisterPageViewModel(
            INavigationService navigationService) : base(navigationService)
        {
            Title = "Registrar nuevo usuario";
        }
    }
}
