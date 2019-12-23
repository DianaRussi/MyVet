﻿using MyVet.Common.Models.Services;
using Prism.Commands;
using Prism.Navigation;
using System.Threading.Tasks;

namespace MyVet.Prism.ViewModels
{
    public class ChangePasswordPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private bool _isRunning;
        private bool _isEnabled;
        private DelegateCommand _changePasswordCommand;

        public ChangePasswordPageViewModel(
            INavigationService navigationService,
            IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            _isEnabled = true;
            Title = "Cambiar contraseña";
        }

        public DelegateCommand ChangePasswordCommand => _changePasswordCommand ?? (_changePasswordCommand = new DelegateCommand(ChangePassword));

        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string PasswordConfirm { get; set; }
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

        private async void ChangePassword()
        {
            var isValid = await ValidateData();
            if (!isValid)
            {
                return;
            }
        }
        private async Task<bool> ValidateData()
        {
            if (string.IsNullOrEmpty(CurrentPassword))
            {
                await App.Current.MainPage.DisplayAlert(
                   "Error",
                    "Usted debe ingresar su contraseña actual",
                    "Aceptar");
                return false;
            }

            if (string.IsNullOrEmpty(NewPassword) || NewPassword?.Length < 6)
            {
                await App.Current.MainPage.DisplayAlert(
                    "Error",
                    "Debe ingresar una nueva contraseña de al menos seis caracteres.",
                    "Aceptar");
                return false;
            }

            if (string.IsNullOrEmpty(PasswordConfirm))
            {
                await App.Current.MainPage.DisplayAlert(
                    "Error",
                    "Debes ingresar la confirmación de la contraseña",
                     "Aceptar");
                return false;
            }

            if (!NewPassword.Equals(PasswordConfirm))
            {
                await App.Current.MainPage.DisplayAlert(
                    "Error",
                    "La nueva contraseña y la confirmación no son iguales",
                     "Aceptar");
                return false;
            }
            return true;
        }
    }
}