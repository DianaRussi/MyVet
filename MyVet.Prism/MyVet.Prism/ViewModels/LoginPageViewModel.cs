using MyVet.Common.Models;
using MyVet.Common.Models.Services;
using Prism.Commands;
using Prism.Navigation;

namespace MyVet.Prism.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private string _password;
        private bool _isRunning;
        private bool _isEnabled;
        private DelegateCommand _loginCommand;
        public LoginPageViewModel(
            INavigationService navigationService,
            IApiService apiService ) : base(navigationService)
        {
            Title = "Ingresar";
            IsEnabled = true;
            _navigationService = navigationService;
            _apiService = apiService;

            //TODO: Delete this lines
            Email = "dianarussi@yahoo.com";
            Password = "123456";
        }
        public string Email { get; set; }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

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

        public DelegateCommand LoginCommand => _loginCommand ?? (_loginCommand = new DelegateCommand(Login));

        private async void Login()
        {
            //validar que se haya digitado email y pass
            if (string.IsNullOrEmpty(Email))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Usted debe ingresar un email", "Accept");
                return;
            }
            if (string.IsNullOrEmpty(Password))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Usted debe ingresar una contraseña", "Accept");
                return;
            }
            //mientras opere
            IsRunning = true;
            IsEnabled = false;

            var url = App.Current.Resources["UrlAPI"].ToString(); //urlapi es como se llama en el app.xaml
            var connection = await _apiService.CheckConnection(url);
            if (!connection)
            {
                IsEnabled = true;
                IsRunning = false;
                await App.Current.MainPage.DisplayAlert("Error", "Revisa tu conexión a internet.", "Accept");
                return;
            }

            var request = new TokenRequest
            {
                Password = Password,
                Username = Email,
            };

           
            var response = await _apiService.GetTokenAsync(url, "/Account", "/CreateToken", request);

            if (!response.IsSuccess)
            {
                //cuando termine
                IsRunning = false;
                IsEnabled = true;
                await App.Current.MainPage.DisplayAlert("Error", "Usuario o contraseña incorrecta", "Accept");
                Password = string.Empty;
                return;
            }
            //consumir owner
            var token = response.Result;
            var response2 = await _apiService.GetOwnerByEmailAsync(url, "api", "/Owners/GetOwnerByEmail", "bearer",token.Token,Email);
            if (!response2.IsSuccess)
            {
                IsRunning = false;
                IsEnabled = true;
                await App.Current.MainPage.DisplayAlert("Error", "Este usuario tiene un problema. Llama a soporte técnico", "Accept");
                Password = string.Empty;
                return;
            }

            //descerialzar objeto usuario
            var owner = response2.Result;

            var parameters = new NavigationParameters
            {
                { "owner", owner }
            };
            IsRunning = false;
            IsEnabled = true;

            await _navigationService.NavigateAsync("PetsPage",parameters);
            Password = string.Empty;
        }
    }
}