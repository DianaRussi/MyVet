using MyVet.Common.Helpers;
using MyVet.Common.Models;
using MyVet.Common.Models.Services;
using Prism.Commands;
using Prism.Navigation;
using System.Threading.Tasks;

namespace MyVet.Prism.ViewModels
{
    public class RememberPasswordPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private bool _isRunning;
        private bool _isEnabled;
        private DelegateCommand _recoverCommand;

        public RememberPasswordPageViewModel(
            INavigationService navigationService,
            IApiService apiService) : base(navigationService)
        {
            Title = "Recordar contraseña";
            IsEnabled = true;
            _navigationService = navigationService;
            _apiService = apiService;
        }

        public DelegateCommand RecoverCommand => _recoverCommand ?? (_recoverCommand = new DelegateCommand(Recover));

        public string Email { get; set; }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }

        private async void Recover()
        {
            var isValid = await ValidateData();
            if (!isValid)
            {
                return;
            }

            IsRunning = true;
            IsEnabled = false;

            var request = new EmailRequest
            {
                Email = Email
            };

            var url = App.Current.Resources["UrlAPI"].ToString();
            var response = await _apiService.RecoverPasswordAsync(
                url,
                "/api",
                "/Account/RecoverPassword",
                request);

            IsRunning = false;
            IsEnabled = true;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Aceptar");
                return;
            }

            await App.Current.MainPage.DisplayAlert(
                "Ok",
                response.Message,
                 "Aceptar");
            await _navigationService.GoBackAsync();
        }

        private async Task<bool> ValidateData()
        {
            if (string.IsNullOrEmpty(Email) || !RegexHelper.IsValidEmail(Email))
            {
                await App.Current.MainPage.DisplayAlert(
                    "Error",
                    "Debes ingresar un correo valido.",
                    "Aceptar");
                return false;
            }
            return true;
        }
    }
}